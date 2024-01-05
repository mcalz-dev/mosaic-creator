using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class SourceImageSelectionPipeline
    {
        internal SourceImageSelectionPipeline(List<ISourceImageSelectionPipelineOperation> operations, ReuseCostFunction? reuseCostFunction, ICostFunction finalCostFunction)
        {
            Operations = operations;
            ReuseCostFunction = reuseCostFunction;
            FinalCostFunction = finalCostFunction;
        }

        public IReadOnlyList<ISourceImageSelectionPipelineOperation> Operations { get; }

        public ReuseCostFunction? ReuseCostFunction { get; }

        public ICostFunction FinalCostFunction { get; }
    }

    internal class SourceImageSelectionPipelineBuilder
    {
        private readonly List<ISourceImageSelectionPipelineOperation> _operations = new List<ISourceImageSelectionPipelineOperation>();
        private ReuseCostFunction? _reuseCostFunction;
        private ICostFunction _lastCostFunction = new ConstantCostFunction(0.0);
        private readonly IFilterFunction _defaultFilterFunction;
        private readonly Configuration _configuration;

        public SourceImageSelectionPipelineBuilder(Configuration configuration)
        {
            _configuration = configuration;
            _defaultFilterFunction = new PercentageRelativeToValuesFilterFunction(_configuration.MinimumCostFactorForContestantToSurviveRound); ;
        }

        public SourceImageSelectionPipelineBuilder RemoveRecentlyUsedImages(IFilterFunction? filterFunction = null)
        {
            _reuseCostFunction = new ReuseCostFunction();
            return FilterBy(_reuseCostFunction, filterFunction);
        }

        public SourceImageSelectionPipelineBuilder FilterBy(ICostFunction costFunction, IFilterFunction? filterFunction = null)
        {
            _operations.Add(new FilteringSourceImageSelectionPipelineOperation(costFunction, filterFunction != null ? filterFunction : _defaultFilterFunction));
            return this;
        }

        public SourceImageSelectionPipelineBuilder MutateImages()
        {
            _operations.Add(new MutatingSourceImageSelectionPipelineOperation());
            return this;
        }

        public SourceImageSelectionPipeline FinallyFilterBy(ICostFunction costFunction, IFilterFunction? filterFunction = null)
        {
            _lastCostFunction = costFunction;
            FilterBy(costFunction, filterFunction);
            return new SourceImageSelectionPipeline(_operations, _reuseCostFunction, _lastCostFunction);
        }
    }
}

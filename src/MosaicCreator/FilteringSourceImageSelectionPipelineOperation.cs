using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class FilteringSourceImageSelectionPipelineOperation : ISourceImageSelectionPipelineOperation
    {
        private readonly ICostFunction _costFunction;
        private readonly IFilterFunction _filterFunction;

        public FilteringSourceImageSelectionPipelineOperation(ICostFunction costFunction, IFilterFunction filterFunction)
        {
            _costFunction = costFunction;
            _filterFunction = filterFunction;
        }

        public IEnumerable<ISourceImage> Apply(IEnumerable<ISourceImage> sourceImages, ImageMetadata destinationMetadata)
        {
            var costPerContestant = new List<(ISourceImage Contestant, double Cost)>();
            foreach (var contestant in sourceImages)
            {
                costPerContestant.Add(new(contestant, _costFunction.GetCostForApplying(contestant.Metadata, destinationMetadata)));
            }

            var ordered = costPerContestant.OrderBy(x => x.Cost).ToList();
            return _filterFunction.Filter(ordered);
        }
    }
}

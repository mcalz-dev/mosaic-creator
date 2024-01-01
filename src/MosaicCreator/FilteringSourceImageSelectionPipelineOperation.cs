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
        private readonly double _minimumCostFactorForContestantToSurviveRound;

        public FilteringSourceImageSelectionPipelineOperation(ICostFunction costFunction, double minimumCostFactorForContestantToSurviveRound)
        {
            _costFunction = costFunction;
            _minimumCostFactorForContestantToSurviveRound = minimumCostFactorForContestantToSurviveRound;
        }

        public IEnumerable<ISourceImage> Apply(IEnumerable<ISourceImage> sourceImages, ImageMetadata destinationMetadata)
        {
            var costPerContestant = new List<(ISourceImage Contestant, double Cost)>();
            foreach (var contestant in sourceImages)
            {
                costPerContestant.Add(new(contestant, _costFunction.GetCostForApplying(contestant.Metadata, destinationMetadata)));
            }

            var ordered = costPerContestant.OrderBy(x => x.Cost).ToList();
            var best = ordered.First();
            var worst = ordered.Last();
            return ordered.TakeWhile(x => x.Cost <= best.Cost + ((worst.Cost - best.Cost) * _minimumCostFactorForContestantToSurviveRound)).Select(x => x.Contestant);
        }
    }
}

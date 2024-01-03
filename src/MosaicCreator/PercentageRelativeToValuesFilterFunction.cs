using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class PercentageRelativeToValuesFilterFunction : IFilterFunction
    {
        private readonly double _minimumCostFactorForContestantToSurviveRound;

        public PercentageRelativeToValuesFilterFunction(double minimumCostFactorForContestantToSurviveRound)
        {
            _minimumCostFactorForContestantToSurviveRound = minimumCostFactorForContestantToSurviveRound;
        }

        public IEnumerable<ISourceImage> Filter(IEnumerable<(ISourceImage Contestant, double Cost)> imagesSortedByCost)
        {
            var best = imagesSortedByCost.First();
            var worst = imagesSortedByCost.Last();
            return imagesSortedByCost.TakeWhile(x => x.Cost <= best.Cost + ((worst.Cost - best.Cost) * _minimumCostFactorForContestantToSurviveRound)).Select(x => x.Contestant);
        }
    }
}

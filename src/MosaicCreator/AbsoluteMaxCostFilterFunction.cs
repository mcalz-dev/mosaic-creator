using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class AbsoluteMaxCostFilterFunction : IFilterFunction
    {
        private readonly double _maxCost;

        public AbsoluteMaxCostFilterFunction(double maxCost)
        {
            _maxCost = maxCost;
        }

        public IEnumerable<ISourceImage> Filter(IEnumerable<(ISourceImage Contestant, double Cost)> imagesSortedByCost)
        {
            return imagesSortedByCost.TakeWhile(x => x.Cost <= _maxCost).Select(x => x.Contestant);
        }
    }
}

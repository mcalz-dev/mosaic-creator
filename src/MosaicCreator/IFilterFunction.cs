using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal interface IFilterFunction
    {
        public IEnumerable<ISourceImage> Filter(IEnumerable<(ISourceImage Contestant, double Cost)> imagesSortedByCost);
    }
}

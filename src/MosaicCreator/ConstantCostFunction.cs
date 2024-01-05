using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class ConstantCostFunction : ICostFunction
    {
        private readonly double _cost;

        public ConstantCostFunction(double cost)
        {
            _cost = cost;
        }

        public double GetCostForApplying(ImageMetadata source, ImageMetadata destinationSection)
        {
            return _cost;
        }
    }
}

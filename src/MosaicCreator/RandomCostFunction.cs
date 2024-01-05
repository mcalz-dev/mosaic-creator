using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class RandomCostFunction : ICostFunction
    {
        private readonly Random _random;

        public RandomCostFunction()
        {
            _random = new Random();
        }

        public double GetCostForApplying(ImageMetadata source, ImageMetadata destinationSection)
        {
            return _random.NextDouble();
        }
    }
}

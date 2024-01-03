using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class AspectRatioCostFunction : ICostFunction
    {
        public double GetCostForApplying(ImageMetadata source, ImageMetadata destinationSection)
        {
            var largerAspectRatio = Math.Max(source.AspectRatio, destinationSection.AspectRatio);
            var smallerAspectRatio = Math.Min(source.AspectRatio, destinationSection.AspectRatio);
            var aspectRatioFactor = smallerAspectRatio / largerAspectRatio;
            var cost = 1 - aspectRatioFactor;
            return Math.Pow(cost, 4); //Pow for faster divergence
        }
    }
}

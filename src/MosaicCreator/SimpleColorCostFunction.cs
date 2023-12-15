using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class SimpleColorCostFunction : ICostFunction
    {
        public double GetCostForApplying(ImageMetadata source, ImageMetadata destination)
        {
            var histogramOfSource = source.ColorHistogram;
            var histogramOfDestination = destination.ColorHistogram;
            var totalCost = 0.0;
            for (int i = 0; i < histogramOfSource.Size; i++)
            {
                totalCost += Math.Abs(histogramOfSource[i] - histogramOfDestination[i]);
            }

            return totalCost / 2;
        }

        public void HandleWinner(PreprocessedImageInfo winner)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class PictogramComparisonCostFunction : ICostFunction
    {
        public double GetCostForApplying(ImageMetadata source, ImageMetadata destinationSection)
        {
            var totalCost = 0.0;
            for (int i = 0; i < Pictogram.PixelCount; i++)
            {
                totalCost += GetDifference(source.Pictogram.Pixels[i], destinationSection.Pictogram.Pixels[i]);
            }

            return totalCost / Pictogram.PixelCount;
        }

        public void HandleWinner(ImageMetadata winner)
        {
            _ = winner;
        }

        private static double GetDifference(Color a, Color b)
        {
            var cost = (GetChannelDifference(a.R, b.R) + GetChannelDifference(a.G, b.G) + GetChannelDifference(a.B, b.B)) / 3;
            return cost;
        }

        private static double GetChannelDifference(byte a, byte b)
        {
            var cost = (double)((a - b) * (a - b)) / (byte.MaxValue * byte.MaxValue);
            return cost;
        }
    }
}

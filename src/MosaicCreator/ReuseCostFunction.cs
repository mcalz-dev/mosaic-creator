using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class ReuseCostFunction : ICostFunction
    {
        private readonly IList<ImageMetadata> _appliedImages = new List<ImageMetadata>();

        public double GetCostForApplying(ImageMetadata source, ImageMetadata destinationSection)
        {
            var index = _appliedImages.IndexOf(source);
            if (index < 0)
            {
                return 0.0;
            }

            return 1.0 - (index / _appliedImages.Count);
        }

        public void HandleWinner(ImageMetadata winner)
        {
            _appliedImages.Add(winner);
        }
    }
}

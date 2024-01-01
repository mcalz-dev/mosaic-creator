using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class MutatingSourceImageSelectionPipelineOperation : ISourceImageSelectionPipelineOperation
    {
        public IEnumerable<ISourceImage> Apply(IEnumerable<ISourceImage> sourceImages, ImageMetadata destinationMetadata)
        {
            var newSourceImages = new List<ISourceImage>();
            var mutatingSections = new List<RectangleF>()
            {
                new RectangleF(0.2f, 0.2f, 0.6f, 0.6f),
                new RectangleF(0.0f, 0.0f, 0.5f, 0.5f),
                new RectangleF(0.5f, 0.0f, 0.5f, 0.5f),
                new RectangleF(0.0f, 0.5f, 0.5f, 0.5f),
                new RectangleF(0.5f, 0.5f, 0.5f, 0.5f),
            };

            foreach (var image in sourceImages)
            {
                newSourceImages.Add(image);
                foreach (var section in mutatingSections)
                {
                    newSourceImages.Add(image.GetSection(section));
                }
            }

            return newSourceImages;
        }
    }
}

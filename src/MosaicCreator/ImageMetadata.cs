using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal struct ImageMetadata
    {
        private ImageMetadata(ColorHistogram colorHistogram)
        {
            ColorHistogram = colorHistogram;
        }

        internal ColorHistogram ColorHistogram { get; }

        internal static ImageMetadata Of(Bitmap image)
        {
            return new ImageMetadata(ColorHistogram.Of(image));
        }
    }
}

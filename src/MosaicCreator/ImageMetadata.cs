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
        private ImageMetadata(Size size, Pictogram pictogram, ColorHistogram colorHistogram)
        {
            Size = size;
            Pictogram = pictogram;
            ColorHistogram = colorHistogram;
        }

        internal Pictogram Pictogram { get; }

        internal ColorHistogram ColorHistogram { get; }

        internal Size Size { get; }

        internal static ImageMetadata Of(Bitmap image)
        {
            return new ImageMetadata(image.Size, Pictogram.Of(image), ColorHistogram.Of(image));
        }
    }
}

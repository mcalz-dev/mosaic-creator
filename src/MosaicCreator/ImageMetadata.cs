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
        private ImageMetadata(Pictogram pictogram, ColorHistogram colorHistogram)
        {
            Pictogram = pictogram;
            ColorHistogram = colorHistogram;
        }

        internal Pictogram Pictogram { get; }

        internal ColorHistogram ColorHistogram { get; }

        internal static ImageMetadata Of(Bitmap image)
        {
            return new ImageMetadata(Pictogram.Of(image), ColorHistogram.Of(image));
        }
    }
}

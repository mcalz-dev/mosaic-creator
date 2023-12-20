using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal struct Pictogram
    {
        public const int Height = 8;
        public const int Width = 8;
        public const int PixelCount = Height * Width;
        private readonly Color[] _pixels;
        private readonly double _originalAspectRatio;

        private Pictogram(Color[] pixels, double originalAspectRatio)
        {
            if (pixels.Length != PixelCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pixels), $"The length of the provided pixels array must exatly be {PixelCount}, but got array of length {pixels.Length}.");
            }
            _pixels = pixels;
            _originalAspectRatio = originalAspectRatio;
        }

        internal Color[] Pixels => _pixels;

        public double OriginalAspectRatio => _originalAspectRatio;

        public static Pictogram Of(Bitmap bitmap)
        {
            using var resizedBitmap = bitmap.Resize(new Size(Width, Height));
            return new Pictogram(resizedBitmap.GetPixels().ToArray(), bitmap.Size.GetAspectRatio());
        }
    }
}

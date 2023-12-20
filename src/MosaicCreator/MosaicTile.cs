using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class MosaicTile : IMosaicTile
    {
        private readonly PreprocessedImageInfo _sourceImageInfo;
        private readonly RectangleF _sourceImageSection;
        private readonly RectangleF _destinationImageSection;

        public MosaicTile(PreprocessedImageInfo sourceImage, RectangleF destinationImageSection)
        {
            _sourceImageInfo = sourceImage;
            _destinationImageSection = destinationImageSection;
            _sourceImageSection = new Rectangle(new Point(0, 0), new Size(1, 1));
        }

        public void DrawOn(Graphics graphics, Size graphicsSize)
        {
            var destinationSection = Scale(_destinationImageSection, graphicsSize);
            using var sourceImage = new Bitmap(_sourceImageInfo.OriginalImagePath);
            using var relevantSection = sourceImage.Clone(Scale(_sourceImageSection, sourceImage.Size), sourceImage.PixelFormat);
            using var resizedRelevantSection = relevantSection.Resize(destinationSection.Size);
            graphics.DrawImage(resizedRelevantSection, destinationSection.Location);
        }

        private Rectangle Scale(RectangleF inputRectangle, Size size)
        {
            return new Rectangle(
                (int)Math.Round(inputRectangle.X * size.Width),
                (int)Math.Round(inputRectangle.Y * size.Height),
                (int)Math.Round(inputRectangle.Width * size.Width),
                (int)Math.Round(inputRectangle.Height * size.Height));
        }
    }
}

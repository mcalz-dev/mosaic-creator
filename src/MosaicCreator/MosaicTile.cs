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
        private readonly ISourceImage _sourceImage;
        private readonly RectangleF _sourceImageSection;
        private readonly RectangleF _destinationImageSection;
        private readonly double _finalCost;

        public MosaicTile(ISourceImage sourceImage, RectangleF destinationImageSection, double finalCost)
        {
            _sourceImage = sourceImage;
            _destinationImageSection = destinationImageSection;
            _finalCost = finalCost;
            _sourceImageSection = new Rectangle(new Point(0, 0), new Size(1, 1));
        }

        public void DrawOn(Graphics graphics, Size graphicsSize)
        {
            var destinationSection = Scale(_destinationImageSection, graphicsSize);
            using var sourceImage = _sourceImage.Load();
            using var relevantSection = sourceImage.Clone(Scale(_sourceImageSection, sourceImage.Size), sourceImage.PixelFormat);
            using var resizedRelevantSection = relevantSection.Resize(destinationSection.Size);
            graphics.DrawImage(resizedRelevantSection, destinationSection.Location);
        }

        public double GetFinalCost()
        {
            return _finalCost;
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

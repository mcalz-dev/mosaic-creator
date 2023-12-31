﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class PartialSourceImage : ISourceImage
    {
        private readonly RectangleF _section;
        private ImageMetadata? _metadata;

        public PartialSourceImage(string originalImagePath, string reducedImagePath, RectangleF section)
        {
            OriginalImagePath = originalImagePath;
            ReducedImagePath = reducedImagePath;
            _section = section;
        }

        public ImageMetadata Metadata
        {
            get
            {
                _metadata ??= CalculateMetadata();
                return _metadata.Value;
            }
        }

        public string OriginalImagePath { get; }

        public string ReducedImagePath { get; }

        public ISourceImage GetSection(RectangleF section)
        {
            return new PartialSourceImage(OriginalImagePath, ReducedImagePath, GetSubSection(section));
        }

        public Bitmap Load()
        {
            using var fullImage = new Bitmap(OriginalImagePath);
            var section = Scale(_section, fullImage.Size);
            return fullImage.Clone(section, fullImage.PixelFormat);
        }

        private ImageMetadata CalculateMetadata()
        {
            using var fullImage = new Bitmap(ReducedImagePath);
            var section = Scale(_section, fullImage.Size);
            using var imageSection = fullImage.Clone(section, fullImage.PixelFormat);
            return ImageMetadata.Of(imageSection);

        }

        private Rectangle Scale(RectangleF inputRectangle, Size size)
        {
            var x = Math.Min(size.Width - 1, (int)Math.Round(inputRectangle.X * size.Width));
            var y = Math.Min(size.Height - 1, (int)Math.Round(inputRectangle.Y * size.Height));
            var width = Math.Min(size.Width - x, (int)Math.Round(inputRectangle.Width * size.Width));
            var height = Math.Min(size.Height - y, (int)Math.Round(inputRectangle.Height * size.Height));
            return new Rectangle(x, y, width, height);
        }

        private RectangleF GetSubSection(RectangleF subSection)
        {
            return new RectangleF(
                _section.X + subSection.X * _section.Width,
                _section.Y + subSection.Y * _section.Height,
                _section.Width * subSection.Width,
                _section.Height * subSection.Height);
        }
    }
}

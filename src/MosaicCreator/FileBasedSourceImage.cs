using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class FileBasedSourceImage : ISourceImage
    {
        private ImageMetadata? metadata;

        public FileBasedSourceImage(string originalImagePath, string reducedImagePath)
        {
            OriginalImagePath = originalImagePath;
            ReducedImagePath = reducedImagePath;
        }

        public string OriginalImagePath { get; }

        public string ReducedImagePath { get; }

        public ImageMetadata Metadata
        {
            get
            {
                metadata ??= CalculateMetadata();
                return metadata.Value;
            }
        }

        public Bitmap Load()
        {
            return new Bitmap(OriginalImagePath);
        }

        private ImageMetadata CalculateMetadata()
        {
            using var bitmap = new Bitmap(ReducedImagePath);
            return ImageMetadata.Of(bitmap);
        }
    }
}

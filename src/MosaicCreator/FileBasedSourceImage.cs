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
        private Dictionary<RectangleF, ISourceImage> _cachedSections;

        public FileBasedSourceImage(string originalImagePath, string reducedImagePath)
        {
            OriginalImagePath = originalImagePath;
            ReducedImagePath = reducedImagePath;
            _cachedSections = new Dictionary<RectangleF, ISourceImage>();
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

        public ISourceImage GetSection(RectangleF section)
        {
            if (_cachedSections.TryGetValue(section, out ISourceImage? cachedSection))
            {
                return cachedSection;
            }
            
            cachedSection = new PartialSourceImage(OriginalImagePath, ReducedImagePath, section);
            _cachedSections.Add(section, cachedSection);
            return cachedSection;
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

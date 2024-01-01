using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class PreprocessedImageInfo
    {
        private readonly ISourceImage _sourceImage;

        public PreprocessedImageInfo(string originalImagePath, string reducedImagePath, DateTime timestamp)
        {
            OriginalImagePath = Path.GetFullPath(originalImagePath);
            ReducedImagePath = Path.GetFullPath(reducedImagePath);
            Timestamp = timestamp;
            _sourceImage = new FileBasedSourceImage(OriginalImagePath, ReducedImagePath);
        }

        

        public string OriginalImagePath { get; }

        public string ReducedImagePath { get; }

        public DateTime Timestamp { get; }

        public ISourceImage Load()
        {
            return _sourceImage;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class PreprocessedImageInfo
    {
        public PreprocessedImageInfo(string originalImagePath, string reducedImagePath, DateTime timestamp)
        {
            OriginalImagePath = Path.GetFullPath(originalImagePath);
            ReducedImagePath = Path.GetFullPath(reducedImagePath);
            Timestamp = timestamp;
        }

        

        public string OriginalImagePath { get; }

        public string ReducedImagePath { get; }

        public DateTime Timestamp { get; }

        [JsonIgnore]
        public ColorHistogram Histogram { get; set; }
    }
}

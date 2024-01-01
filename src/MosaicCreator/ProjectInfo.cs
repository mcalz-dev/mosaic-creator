using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class ProjectInfo
    {
        public IList<PreprocessedImageInfo> PreprocessedImages { get; } = new List<PreprocessedImageInfo>();

        public void Upsert(PreprocessedImageInfo newImageInfo)
        {
            var preprocessedImage = PreprocessedImages.FirstOrDefault(x => x.OriginalImagePath == newImageInfo.OriginalImagePath);
            if (preprocessedImage != null)
            {
                var index = PreprocessedImages.IndexOf(preprocessedImage);
                PreprocessedImages[index] = newImageInfo;
            }
            else
            {
                PreprocessedImages.Add(newImageInfo);
            }
        }

        public void RemoveObsoleteFiles(IEnumerable<string> availableFiles)
        {
            var lookup = new HashSet<string>(availableFiles.Select(Path.GetFullPath));
            var obsoleteFiles = PreprocessedImages.Where(x => !lookup.Contains(Path.GetFullPath(x.OriginalImagePath))).ToList();
            foreach (var obsoleteFile in obsoleteFiles)
            {
                PreprocessedImages.Remove(obsoleteFile);
            }
        }
    }
}

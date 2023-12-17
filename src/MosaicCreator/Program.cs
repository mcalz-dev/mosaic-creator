using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Drawing;

namespace MosaicCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configurationProvider = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MosaicCreator"))
                .AddJsonFile("mosaicCreatorConfig.json")
                .Build();

            var configuration = new Configuration();
            configurationProvider.Bind(configuration);
            var projectInfoFile = Path.Combine(configuration.WorkingDirectory, "project.json");
            var projectInfo = BuildProjectInfo(configuration, projectInfoFile);
            using var originalImage = new Bitmap(configuration.InputImagePath);
            var processedImage = (Bitmap)originalImage.Clone();
            var tileSize = 64;
            var numberOfRuns = 100;
            var costFunction = new SimpleColorCostFunction();
            for (int i = 0; i < numberOfRuns; i++)
            {
                Console.WriteLine($"Run {i}");
                var x = Random.Shared.Next(originalImage.Width - tileSize);
                var y = Random.Shared.Next(originalImage.Height - tileSize);
                var sectionRectangle = new Rectangle(x, y, tileSize, tileSize);
                using var extractedSection = (Bitmap)originalImage.Clone(sectionRectangle, originalImage.PixelFormat);
                var destinationMetadata = ImageMetadata.Of(extractedSection);
                var best = projectInfo.PreprocessedImages.MinBy(x => costFunction.GetCostForApplying(x.ImageMetadata, destinationMetadata));
                if (best == null)
                {
                    return;
                }

                using var sourceImage = new Bitmap(best.ReducedImagePath);
                using (var graphics = Graphics.FromImage(processedImage))
                {
                    // Draw the replacement image onto the original image at the specified section
                    graphics.DrawImage(sourceImage, sectionRectangle);
                }
                processedImage.Save("Test.png");
            }
        }

        private static ProjectInfo BuildProjectInfo(Configuration configuration, string projectInfoFile)
        {
            var projectInfo = new ProjectInfo();
            if (File.Exists(projectInfoFile))
            {
                projectInfo = JsonConvert.DeserializeObject<ProjectInfo>(File.ReadAllText(projectInfoFile))!;
            }

            var preprocessedImageSize = 64;
            var maxTileSize = new Size(preprocessedImageSize, preprocessedImageSize);
            var imageFiles = EnumerateImageFiles(configuration.MosaicTilesDirectory);
            projectInfo.RemoveObsoleteFiles(imageFiles);
            foreach (var imageFile in imageFiles)
            {
                var imageFileInfo = new FileInfo(imageFile);
                var preprocessedImageInfo = projectInfo.PreprocessedImages.FirstOrDefault(x => Path.GetFullPath(x.OriginalImagePath) == Path.GetFullPath(imageFileInfo.FullName));
                if (preprocessedImageInfo == null || !File.Exists(preprocessedImageInfo.ReducedImagePath) || imageFileInfo.LastWriteTimeUtc > preprocessedImageInfo.Timestamp)
                {
                    var fingerprint = CalculateFingerprintOfFile(imageFile);
                    var processedFilePath = Path.Combine(configuration.WorkingDirectory, fingerprint + "_" + preprocessedImageSize + ".png");
                    if (!File.Exists(processedFilePath))
                    {
                        using var image = Image.FromFile(imageFile);
                        using var processedImage = image.Scale(maxTileSize);
                        processedImage.Save(processedFilePath);
                    }

                    preprocessedImageInfo = new PreprocessedImageInfo(imageFile, processedFilePath, imageFileInfo.LastWriteTimeUtc);
                    projectInfo.Upsert(preprocessedImageInfo);
                }

                using var reducedImage = new Bitmap(preprocessedImageInfo.ReducedImagePath);
                preprocessedImageInfo.ImageMetadata = ImageMetadata.Of(reducedImage);
            }

            File.WriteAllText(projectInfoFile, JsonConvert.SerializeObject(projectInfo));
            return projectInfo;
        }

        static IEnumerable<string> EnumerateImageFiles(string directory)
        {
            var extensions = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
            return Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories).Where(file => extensions.Any(extension => file.ToLowerInvariant().EndsWith(extension)));
        }

        static string CalculateFingerprintOfFile(string filePath)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
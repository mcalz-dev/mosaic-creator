using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace MosaicCreator
{
    internal class Program
    {
        private static Configuration configuration = new Configuration();
        static async Task Main(string[] args)
        {
            var configurationProvider = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MosaicCreator"))
                .AddJsonFile("mosaicCreatorConfig.json")
                .Build();

            configurationProvider.Bind(configuration);
            var projectInfoFile = Path.Combine(configuration.WorkingDirectory, "project.json");
            var projectInfo = BuildProjectInfo(configuration, projectInfoFile);
            var mosaicBuilder = new MosaicBuilder(configuration, projectInfo);
            var mosaic = await mosaicBuilder.CreateMosaic();
            using var processedImage = new Bitmap(configuration.InputImagePath);
            using (var graphics = Graphics.FromImage(processedImage))
            {
                foreach (var tile in mosaic)
                {
                    tile.DrawOn(graphics, processedImage.Size);
                }
            }

            processedImage.Save("Test.png");
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
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Runtime.Intrinsics.Arm;

namespace MosaicCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MosaicCreator"))
                .AddJsonFile("mosaicCreatorConfig.json")
                .Build();
            var inputImagePath = configuration["inputImagePath"]!;
            var outputImagePath = configuration["outputImagePath"]!;
            var mosaicTilesDirectory = configuration["mosaicTilesDirectory"]!;
            var workingDirectory = configuration["workingDirectory"]!;
            var preprocessedImageSize = 64;
            var maxTileSize = new Size(preprocessedImageSize, preprocessedImageSize);


            foreach (var imageFile in EnumerateImageFiles(mosaicTilesDirectory))
            {
                var fingerprint = CalculateFingerprintOfFile(imageFile);
                var processedFilePath = Path.Combine(workingDirectory, fingerprint + "_" + preprocessedImageSize + ".png");
                if (File.Exists(processedFilePath))
                {
                    continue;
                }

                using var image = Image.FromFile(imageFile);
                using var processedImage = image.Scale(maxTileSize);
                processedImage.Save(processedFilePath);
            }
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
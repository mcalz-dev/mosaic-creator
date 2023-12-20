﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Drawing;
using System.Linq;

namespace MosaicCreator
{
    internal class Program
    {
        private static object _lock = new object();
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
            var numberOfRuns = 10000;
            var costFunctions = new List<ICostFunction>() { new SimpleColorCostFunction(), new PictogramComparisonCostFunction() };
            using (var graphics = Graphics.FromImage(processedImage))
            {
                for (int i = 0; i < numberOfRuns; i++)
                {
                    Console.WriteLine($"Run {i}");
                    ProcessTile(projectInfo, originalImage, tileSize, costFunctions, graphics);

                }
            }
            processedImage.Save("Test.png");
        }

        private static void ProcessTile(ProjectInfo projectInfo, Bitmap originalImage, int tileSize, List<ICostFunction> costFunctions, Graphics graphics)
        {
            var x = Random.Shared.Next(originalImage.Width - tileSize);
            var y = Random.Shared.Next(originalImage.Height - tileSize);
            var sectionRectangle = new Rectangle(x, y, tileSize, tileSize);
            var extractedSection = (Bitmap)originalImage.Clone(sectionRectangle, originalImage.PixelFormat);
            var destinationMetadata = ImageMetadata.Of(extractedSection);
            IEnumerable<PreprocessedImageInfo> contestants = projectInfo.PreprocessedImages;
            foreach (var costFunction in costFunctions)
            {
                var orderedContestants = contestants.OrderBy(x => costFunction.GetCostForApplying(x.ImageMetadata, destinationMetadata)).ToList();
                contestants = orderedContestants.Take(Math.Max(10, (int)(orderedContestants.Count * 0.05))).ToList();
            }

            var best = contestants.First();
            var sourceImage = new Bitmap(best.ReducedImagePath);

            // Draw the replacement image onto the original image at the specified section
            lock (_lock)
            {
                graphics.DrawImage(sourceImage, sectionRectangle);
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
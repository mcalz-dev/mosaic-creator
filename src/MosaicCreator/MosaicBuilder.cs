using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class MosaicBuilder
    {
        private readonly Configuration _configuration;
        private readonly ProjectInfo _projectInfo;

        public MosaicBuilder(Configuration configuration, ProjectInfo projectInfo)
        {
            _configuration = configuration;
            _projectInfo = projectInfo;
        }

        public Task<List<IMosaicTile>> CreateMosaic()
        {
            var result = new List<IMosaicTile>();
            using var inputImage = new Bitmap(_configuration.InputImagePath);
            using var scaledDownInputImage = inputImage.Scale(new Size(1000, 1000));
            var numberOfRuns = 1000;
            var costFunctions = new List<ICostFunction>() { new SimpleColorCostFunction(), new PictogramComparisonCostFunction() };
            using (var graphics = Graphics.FromImage(inputImage))
            {
                for (int i = 0; i < numberOfRuns; i++)
                {
                    Console.WriteLine($"Run {i}");
                    result.Add(ProcessTile(scaledDownInputImage, costFunctions));

                }
            }

            return Task.FromResult(result);
        }

        private IMosaicTile ProcessTile(Bitmap inputImage, List<ICostFunction> costFunctions)
        {
            var tileSize = Math.Max((int)(inputImage.Width * _configuration.RelativeTileSize), (int)(inputImage.Height * _configuration.RelativeTileSize));
            var x = Random.Shared.Next(inputImage.Width - tileSize);
            var y = Random.Shared.Next(inputImage.Height - tileSize);
            var sectionRectangle = new Rectangle(x, y, tileSize, tileSize);
            var extractedSection = (Bitmap)inputImage.Clone(sectionRectangle, inputImage.PixelFormat);
            var destinationMetadata = ImageMetadata.Of(extractedSection);
            IEnumerable<PreprocessedImageInfo> contestants = _projectInfo.PreprocessedImages;
            foreach (var costFunction in costFunctions)
            {
                contestants = FilterContestants(contestants, costFunction, destinationMetadata).ToList();
            }

            var best = contestants.First();
            return new MosaicTile(best, new RectangleF((float)x / inputImage.Width, (float)y / inputImage.Height, (float)sectionRectangle.Width / inputImage.Width, (float)sectionRectangle.Height / inputImage.Height));
        }

        private IEnumerable<PreprocessedImageInfo> FilterContestants(IEnumerable<PreprocessedImageInfo> contestants, ICostFunction costFunction, ImageMetadata destinationMetadata)
        {
            var costPerContestant = new List<(PreprocessedImageInfo Contestant, double Cost)>();
            foreach (var contestant in contestants)
            {
                costPerContestant.Add(new(contestant, costFunction.GetCostForApplying(contestant.ImageMetadata, destinationMetadata)));
            }

            var ordered = costPerContestant.OrderBy(x => x.Cost).ToList();
            var best = ordered.First();
            var worst = ordered.Last();
            return ordered.TakeWhile(x => x.Cost <= best.Cost + ((worst.Cost - best.Cost) * _configuration.MinimumCostFactorForContestantToSurviveRound)).Select(x => x.Contestant);
        }
    }
}

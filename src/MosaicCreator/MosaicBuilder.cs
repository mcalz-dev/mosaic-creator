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
            var reuseCostFunction = new ReuseCostFunction();
            var pipeline = new List<ISourceImageSelectionPipelineOperation>()
            {
                new FilteringSourceImageSelectionPipelineOperation(reuseCostFunction, _configuration.MinimumCostFactorForContestantToSurviveRound),
                new FilteringSourceImageSelectionPipelineOperation(new SimpleColorCostFunction(), _configuration.MinimumCostFactorForContestantToSurviveRound),
                new MutatingSourceImageSelectionPipelineOperation(),
                new FilteringSourceImageSelectionPipelineOperation(new PictogramComparisonCostFunction(), _configuration.MinimumCostFactorForContestantToSurviveRound)
            };
            var finalCostFunction = new PictogramComparisonCostFunction();
            using (var graphics = Graphics.FromImage(inputImage))
            {
                for (int i = 0; i < numberOfRuns; i++)
                {
                    Console.WriteLine($"Run {i}");
                    result.Add(ProcessTile(scaledDownInputImage, pipeline, reuseCostFunction, finalCostFunction));

                }
            }


            return Task.FromResult(result.OrderByDescending(x => x.GetFinalCost()).ToList());
        }

        private IMosaicTile ProcessTile(Bitmap inputImage, List<ISourceImageSelectionPipelineOperation> pipeline, ReuseCostFunction reuseCostFunction, ICostFunction finalCostFunction)
        {
            var tileSize = Math.Max((int)(inputImage.Width * _configuration.RelativeTileSize), (int)(inputImage.Height * _configuration.RelativeTileSize));
            var x = Random.Shared.Next(inputImage.Width - tileSize);
            var y = Random.Shared.Next(inputImage.Height - tileSize);
            var sectionRectangle = new Rectangle(x, y, tileSize, tileSize);
            var extractedSection = (Bitmap)inputImage.Clone(sectionRectangle, inputImage.PixelFormat);
            var destinationMetadata = ImageMetadata.Of(extractedSection);
            IEnumerable<ISourceImage> contestants = _projectInfo.PreprocessedImages.Select(image => image.Load());
            foreach (var operation in pipeline)
            {
                contestants = operation.Apply(contestants, destinationMetadata);
            }

            contestants = contestants.OrderBy(x => finalCostFunction.GetCostForApplying(x.Metadata, destinationMetadata)).ToList();
            var best = contestants.First();
            foreach (var costFunction in pipeline)
            {
                reuseCostFunction.HandleWinner(best.Metadata);
            }

            var finalCost = finalCostFunction.GetCostForApplying(best.Metadata, destinationMetadata);

            return new MosaicTile(best, new RectangleF((float)x / inputImage.Width, (float)y / inputImage.Height, (float)sectionRectangle.Width / inputImage.Width, (float)sectionRectangle.Height / inputImage.Height), finalCost);
        }


    }
}

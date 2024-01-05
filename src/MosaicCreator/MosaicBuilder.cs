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

        public Task<List<IMosaicTile>> CreateMosaic(SourceImageSelectionPipeline pipeline)
        {
            var result = new List<IMosaicTile>();
            using var inputImage = new Bitmap(_configuration.InputImagePath);
            using var scaledDownInputImage = inputImage.Scale(new Size(1000, 1000));
            var numberOfRuns = _configuration.NumberOfTiles;
            for (int i = 0; i < numberOfRuns; i++)
            {
                Console.WriteLine($"Run {i}");
                result.Add(ProcessTile(scaledDownInputImage, pipeline));
            }

            return Task.FromResult(result.OrderByDescending(x => x.GetFinalCost()).ToList());
        }

        private IMosaicTile ProcessTile(Bitmap inputImage, SourceImageSelectionPipeline pipeline)
        {
            var tileAspectRatio = 0.0;
            while (tileAspectRatio < 0.5 || tileAspectRatio > 2)
            {
                tileAspectRatio = _projectInfo.PreprocessedImages[Random.Shared.Next(_projectInfo.PreprocessedImages.Count)].Load().Metadata.AspectRatio;
            }

            var tileWidth = Math.Max((int)(inputImage.Width * _configuration.RelativeTileSize), (int)(inputImage.Height * _configuration.RelativeTileSize));
            var tileHeight = tileWidth;
            if (tileAspectRatio < 1)
            {
                tileWidth = (int)(tileHeight * tileAspectRatio);
            }
            else
            {
                tileHeight = (int)(tileWidth / tileAspectRatio);
            }

            var x = Random.Shared.Next(inputImage.Width - tileWidth);
            var y = Random.Shared.Next(inputImage.Height - tileHeight);
            var sectionRectangle = new Rectangle(x, y, tileWidth, tileHeight);
            var extractedSection = (Bitmap)inputImage.Clone(sectionRectangle, inputImage.PixelFormat);
            var destinationMetadata = ImageMetadata.Of(extractedSection);
            IEnumerable<ISourceImage> contestants = _projectInfo.PreprocessedImages.Select(image => image.Load());
            foreach (var operation in pipeline.Operations)
            {
                contestants = operation.Apply(contestants, destinationMetadata);
            }

            contestants = contestants.OrderBy(x => pipeline.FinalCostFunction.GetCostForApplying(x.Metadata, destinationMetadata)).ToList();
            var best = contestants.First();
            pipeline.ReuseCostFunction?.HandleWinner(best.Metadata);
            var finalCost = pipeline.FinalCostFunction.GetCostForApplying(best.Metadata, destinationMetadata);
            return new MosaicTile(best, new RectangleF((float)x / inputImage.Width, (float)y / inputImage.Height, (float)sectionRectangle.Width / inputImage.Width, (float)sectionRectangle.Height / inputImage.Height), finalCost);
        }


    }
}

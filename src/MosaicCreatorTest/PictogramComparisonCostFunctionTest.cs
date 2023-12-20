using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MosaicCreator
{
    public class PictogramComparisonCostFunctionTest
    {
        private readonly PictogramComparisonCostFunction costFunction = new PictogramComparisonCostFunction();

        [Theory]
        [InlineData("TestData/Red.png")]
        [InlineData("TestData/Green.png")]
        [InlineData("TestData/Blue.png")]
        [InlineData("TestData/WidescreenImage.png")]
        public void SameImageShouldHaveNoCost(string file)
        {
            using var bitmap = new Bitmap(file);
            var metadata = ImageMetadata.Of(bitmap);

            var cost = costFunction.GetCostForApplying(metadata, metadata);

            Assert.Equal(0.0, cost);
        }

        [Fact]
        public void BlackAndWhiteShouldHaveMaximumCost()
        {
            using var bitmap1 = new Bitmap(1, 1);
            using var bitmap2 = new Bitmap(1, 1);
            bitmap1.SetPixel(0,0, Color.White);
            bitmap2.SetPixel(0,0, Color.Black);
            var metadata1 = ImageMetadata.Of(bitmap1);
            var metadata2 = ImageMetadata.Of(bitmap2);

            var cost = costFunction.GetCostForApplying(metadata1, metadata2);

            Assert.Equal(1.0, cost, 0.01);
        }

        [Theory]
        [InlineData("TestData/Green.png", "TestData/Red.png")]
        [InlineData("TestData/Green.png", "TestData/Blue.png")]
        [InlineData("TestData/Red.png", "TestData/Blue.png")]
        public void DifferentColorsShouldHaveHighCost(string image1, string image2)
        {
            using var bitmap1 = new Bitmap(image1);
            using var bitmap2 = new Bitmap(image2);
            var metadata1 = ImageMetadata.Of(bitmap1);
            var metadata2 = ImageMetadata.Of(bitmap2);

            var cost = costFunction.GetCostForApplying(metadata1, metadata2);

            Assert.True(cost > 0.5);
        }

        [Theory]
        [InlineData("TestData/Red.png")]
        [InlineData("TestData/Green.png")]
        [InlineData("TestData/Blue.png")]
        public void RealImageShouldHaveSomeCost(string file)
        {
            using var bitmap1 = new Bitmap(file);
            using var bitmap2 = new Bitmap("TestData/WidescreenImage.png");
            var metadata1 = ImageMetadata.Of(bitmap1);
            var metadata2 = ImageMetadata.Of(bitmap2);

            var cost = costFunction.GetCostForApplying(metadata1, metadata2);

            Assert.NotEqual(0.0, cost);
            Assert.NotEqual(1.0, cost);
        }
    }
}

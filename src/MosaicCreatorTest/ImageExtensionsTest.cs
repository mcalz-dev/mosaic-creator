using System.Drawing;

namespace MosaicCreator
{
    public class ImageExtensionsTest
    {
        [Fact]
        public void ScaleShouldWork()
        {
            const int maxHeightOrWidth = 100;
            using var image = Image.FromFile("TestData/WidescreenImage.png");
            var expectedAspectRatio = image.GetAspectRatio();

            using var newImage = image.Scale(new Size(maxHeightOrWidth, maxHeightOrWidth));

            Assert.Equal(maxHeightOrWidth, newImage.Width);
            Assert.True(newImage.Height < maxHeightOrWidth);
            Assert.Equal(expectedAspectRatio, newImage.GetAspectRatio(), 0.1);
        }

        [Fact]
        public void ScaleShouldWorkEvenIfRatioCanNoLongerBeMaintained()
        {
            var desiredSize = new Size(1, 1);
            using var image = Image.FromFile("TestData/WidescreenImage.png");

            using var newImage = image.Scale(desiredSize);

            Assert.Equal(1, newImage.Width);
            Assert.Equal(1, newImage.Height);
        }

        [Fact]
        public void ResizeShouldWork()
        {
             var desiredSize = new Size(100, 100);
            using var image = Image.FromFile("TestData/WidescreenImage.png");

            using var newImage = image.Resize(desiredSize);

            Assert.Equal(desiredSize.Width, newImage.Width);
            Assert.Equal(desiredSize.Height, newImage.Height);
        }

        [Fact]
        public void ShouldCalculateColorHistogramOfRealImage()
        {
            using var image = new Bitmap("TestData/WidescreenImage.png");

            image.GetColorHistogram();
        }

        [Theory]
        [InlineData("TestData/Red.png", 255,0,0)]
        [InlineData("TestData/Green.png", 0,255,0)]
        [InlineData("TestData/Blue.png", 0,0,255)]
        public void ShouldCalculateColorHistogramCorrectly(string file, int r, int g, int b)
        {
            var color = Color.FromArgb(r, g, b);
            using var image = new Bitmap(file);

            var histogram = image.GetColorHistogram();

            Assert.Equal(1.0, histogram.GetColorPercentage(color));
        }
    }
}
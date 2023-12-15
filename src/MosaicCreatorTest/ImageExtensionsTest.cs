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
        public void ResizeShouldWork()
        {
             var desiredSize = new Size(100, 100);
            using var image = Image.FromFile("TestData/WidescreenImage.png");

            using var newImage = image.Resize(desiredSize);

            Assert.Equal(desiredSize.Width, newImage.Width);
            Assert.Equal(desiredSize.Height, newImage.Height);
        }
    }
}
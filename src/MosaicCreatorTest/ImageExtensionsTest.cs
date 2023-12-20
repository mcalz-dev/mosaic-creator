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
        public void GetPixelsShouldWork()
        {
            using var bitmap = new Bitmap(2, 2);
            bitmap.SetPixel(0, 0, Color.Red);
            bitmap.SetPixel(1, 0, Color.Blue);
            bitmap.SetPixel(0, 1, Color.White);
            bitmap.SetPixel(1, 1, Color.Black);

            var pixels = bitmap.GetPixels();

            Assert.Equal(4, pixels.Count);
            CustomAssert.Equal(Color.Red, pixels[0]);
            CustomAssert.Equal(Color.Blue, pixels[1]);
            CustomAssert.Equal(Color.White, pixels[2]);
            CustomAssert.Equal(Color.Black, pixels[3]);
        }
    }
}
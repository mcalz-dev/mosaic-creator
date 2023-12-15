using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    public class ColorHistogramTest
    {
        [Fact]
        public void ShouldReturnCorrectColorPercentage()
        {
            var builder = ColorHistogram.GetBuilder();
            builder.IncrementColorCount(Color.Red);
            builder.IncrementColorCount(Color.Blue);
            builder.IncrementColorCount(Color.Red);
            builder.IncrementColorCount(Color.Red);

            var histogram = builder.Build();
            Assert.Equal(0.75, histogram.GetColorPercentage(Color.Red), 0.001);
            Assert.Equal(0.25, histogram.GetColorPercentage(Color.Blue), 0.001);
        }

        [Fact]
        public void ShouldCalculateColorHistogramOfRealImage()
        {
            using var image = new Bitmap("TestData/WidescreenImage.png");

            ColorHistogram.Of(image);
        }

        [Theory]
        [InlineData("TestData/Red.png", 255, 0, 0)]
        [InlineData("TestData/Green.png", 0, 255, 0)]
        [InlineData("TestData/Blue.png", 0, 0, 255)]
        public void ShouldCalculateColorHistogramCorrectly(string file, int r, int g, int b)
        {
            var color = Color.FromArgb(r, g, b);
            using var image = new Bitmap(file);

            var histogram = ColorHistogram.Of(image);

            Assert.Equal(1.0, histogram.GetColorPercentage(color));
        }
    }
}

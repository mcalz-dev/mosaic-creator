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
        public void ShouldReturnCorrectColorCount()
        {
            var colorHistogram = new ColorHistogram();
            colorHistogram.IncrementColorCount(Color.Red);
            colorHistogram.IncrementColorCount(Color.Blue);
            colorHistogram.IncrementColorCount(Color.Red);

            Assert.Equal(2, colorHistogram.GetColorCount(Color.Red));
            Assert.Equal(1, colorHistogram.GetColorCount(Color.Blue));
        }

        [Fact]
        public void ShouldReturnCorrectColorPercentage()
        {
            var colorHistogramm = new ColorHistogram();
            colorHistogramm.IncrementColorCount(Color.Red);
            colorHistogramm.IncrementColorCount(Color.Blue);
            colorHistogramm.IncrementColorCount(Color.Red);
            colorHistogramm.IncrementColorCount(Color.Red);

            Assert.Equal(0.75, colorHistogramm.GetColorPercentage(Color.Red), 0.001);
            Assert.Equal(0.25, colorHistogramm.GetColorPercentage(Color.Blue), 0.001);
        }
    }
}

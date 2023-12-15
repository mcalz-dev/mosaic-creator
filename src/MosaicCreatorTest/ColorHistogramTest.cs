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
    }
}

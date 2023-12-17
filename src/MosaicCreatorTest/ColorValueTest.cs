using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    public class ColorValueTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public void ShouldThrowIfArgumentIsOutOfRange(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ColorValue.FromValue(id));
        }

        [Fact]
        public void FirstHalfOfColorValueShouldCorrespondToHue()
        {
            var testData = new[] { (Color.Red, 0), (Color.Green, 120), (Color.Blue, 240) };
            foreach (var item in testData)
            {
                var colorValue = ColorValue.Of(item.Item1);
                Assert.Equal(item.Item2, (colorValue.AsInt * 360) / ((ColorValue.Max.AsInt + 1) / 2));
            }
        }

        [Fact]
        public void SecondHalfOfColorValueShouldCorrespondToBrightness()
        {
            var grayScaleOffset = (ColorValue.Max.AsInt + 1) / 2;
            var testData = new[] { (Color.Black, grayScaleOffset), (Color.Gray, (grayScaleOffset + ColorValue.Max.AsInt + 1) / 2), (Color.White, ColorValue.Max.AsInt) };
            foreach (var item in testData)
            {
                var colorValue = ColorValue.Of(item.Item1);
                Assert.Equal(item.Item2, colorValue.AsInt);
            }
        }
    }
}

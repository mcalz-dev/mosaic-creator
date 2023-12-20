using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    public class BitmapEnumeratorTest
    {
        [Fact]
        public void ShouldEnumerateOverAllPixelsFromLeftToRightTopDown()
        {
            using var bitmap = new Bitmap(2, 2);
            bitmap.SetPixel(0, 0, Color.Red);
            bitmap.SetPixel(1, 0, Color.Blue);
            bitmap.SetPixel(0, 1, Color.White);
            bitmap.SetPixel(1, 1, Color.Black);
            var pixels = new List<Color>();
            foreach (var pixel in bitmap)
            {
                pixels.Add(pixel);
            }

            Assert.Equal(4, pixels.Count);
            CustomAssert.Equal(Color.Red, pixels[0]);
            CustomAssert.Equal(Color.Blue, pixels[1]);
            CustomAssert.Equal(Color.White, pixels[2]);
            CustomAssert.Equal(Color.Black, pixels[3]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    public class ColorIdTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(256)]
        public void ShouldThrowIfArgumentIsOutOfRange(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ColorId.FromValue(id));
        }
    }
}

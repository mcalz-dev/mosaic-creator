using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    public class EnumerableExtensionsToBitmaskTest
    {
        [Theory]
        [InlineData(new[] {1, -1, 1, -1}, 0b_1000_0000_0000_0000_1000_0000_0000_0000)]
        [InlineData(new[] {1, 1, 1, 1, 1, 1, 1, 1 }, 0b_1000_1000_1000_1000_1000_1000_1000_1000)]
        public void ToBitmaskShouldWorkForSmallArrays(int[] ints, uint expectedBitmask)
        {
            var actualBitmask = ints.ToBitmask(x => x > 0);

            Assert.Equal(expectedBitmask, actualBitmask);
        }

        [Fact]
        public void ToBitmaskShouldWorkForLargeArrays()
        {
            var expectedBitmask = 0b_1000_1000_1000_1000_1000_1000_1000_1000;
            var data = Enumerable.Repeat(0, 64).ToArray();
            for (var i = 0; i < data.Length; i += 64 / 8)
            {
                data[i] = 1;
            }

            var actualBitmask = data.ToBitmask(x => x > 0);

            Assert.Equal(expectedBitmask, actualBitmask);
        }
    }
}

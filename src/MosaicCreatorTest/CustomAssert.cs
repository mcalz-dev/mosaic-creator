using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    public static class CustomAssert
    {
        public static void Equal(Color expected, Color actual)
        {
            Assert.Equal(expected.R, actual.R);
            Assert.Equal(expected.G, actual.G);
            Assert.Equal(expected.B, actual.B);
            Assert.Equal(expected.A, actual.A);
        }
    }
}

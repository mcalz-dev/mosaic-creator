using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal static class SizeExtensions
    {
        public static double GetAspectRatio(this Size size)
        {
            return (double)size.Width / size.Height;
        }
    }
}

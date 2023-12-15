using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal class ColorHistogram
    {
        private int[] _data;
        private long _count;

        public ColorHistogram()
        {
            _data = new int[ColorValue.Max.AsInt + 1];
        }

        public void IncrementColorCount(Color color)
        {
            _data[ColorValue.Of(color).AsInt]++;
            _count++;
        }

        public int GetColorCount(Color color)
        {
            return _data[ColorValue.Of(color).AsInt];
        }

        public double GetColorPercentage(Color color)
        {
            return GetColorCount(color) / (double)_count;
        }
    }
}

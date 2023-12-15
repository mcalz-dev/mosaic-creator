using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal struct ColorHistogram
    {
        private readonly float[] _data;

        private ColorHistogram(int[] colorCounts, long totalCount)
        {
            _data = new float[colorCounts.Length];
            for (int i = 0; i < colorCounts.Length; i++)
            {
                _data[i] = (float)colorCounts[i] / totalCount;
            }
        }

        public float GetColorPercentage(Color color)
        {
            return _data[ColorValue.Of(color).AsInt];
        }

        public static Builder GetBuilder() => new Builder();

        internal class Builder
        {
            private readonly int[] _data;
            private long _count;

            internal Builder()
            {
                _data = new int[ColorValue.Max.AsInt + 1];
            }

            public void IncrementColorCount(Color color)
            {
                _data[ColorValue.Of(color).AsInt]++;
                _count++;
            }

            public ColorHistogram Build()
            {
                return new ColorHistogram(_data, _count);
            }
        }
    }
}

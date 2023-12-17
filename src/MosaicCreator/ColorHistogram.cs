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
        private static readonly double BitmaskThreshold = 1.0 / ColorValue.Max.AsInt;
        private readonly float[] _data;
        private readonly uint _bitmask;

        private ColorHistogram(int[] colorCounts, long totalCount)
        {
            _data = new float[colorCounts.Length];
            for (int i = 0; i < colorCounts.Length; i++)
            {
                _data[i] = (float)colorCounts[i] / totalCount;
            }

            _bitmask = _data.ToBitmask(x => x >=  BitmaskThreshold);
        }

        public readonly int Size => _data.Length;

        public float this[int index] => _data[index];

        public uint AsBitmask => _bitmask;

        public float GetColorPercentage(Color color)
        {
            return _data[ColorValue.Of(color).AsInt];
        }

        public static Builder GetBuilder() => new Builder();

        public static ColorHistogram Of(Bitmap bitmap)
        {
            var colorHistogramBuilder = GetBuilder();
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    colorHistogramBuilder.IncrementColorCount(bitmap.GetPixel(x, y));
                }
            }

            return colorHistogramBuilder.Build();
        }

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

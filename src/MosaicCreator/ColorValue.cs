﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal struct ColorValue
    {
        public static readonly ColorValue Min = new ColorValue(0);
        public static readonly ColorValue Max = new ColorValue(35);

        private ColorValue(int value)
        {
            AsInt = value;
        }


        public static ColorValue FromValue(int value)
        {
            if (value < Min.AsInt | value > Max.AsInt)
            {
                throw new ArgumentOutOfRangeException($"Id {value} is out of range of allowed values [{Min.AsInt},{Max.AsInt}]");
            }

            return new ColorValue(value);
        }

        public static ColorValue Of(Color color)
        {
            var saturation = color.GetSaturation();
            if (saturation < 0.3)
            {
                var brightness = color.GetBrightness();
                var grayScaleOffset = (Max.AsInt + 1) / 2;
                var grayScaleSize = (Max.AsInt + 1) / 2;
                return FromValue(grayScaleOffset + Math.Min(grayScaleSize - 1, (int)(brightness * grayScaleSize)));
            }

            var hue = color.GetHue();
            var value = (int)((Max.AsInt + 1) * (hue / 360)) / 2;
            if (value == Max.AsInt + 1)
            {
                value = 0;
            }

            return FromValue(value);
        }

        public int AsInt { get; }
    }
}

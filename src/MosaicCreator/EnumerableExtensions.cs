using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicCreator
{
    internal static class EnumerableExtensions
    {
        public static uint ToBitmask<T>(this IEnumerable<T> values, Predicate<T> threshold)
            where T : struct, IConvertible
        {
            return ToBitmask(values, threshold, (a, b) => (T)Convert.ChangeType(Convert.ToDouble(a) + Convert.ToDouble(b), typeof(T)));
        }

        public static uint ToBitmask<T>(this IEnumerable<T> values, Predicate<T> threshold, Func<T,T,T> accumulatorFunction)
            where T : struct
        {
            const int NumberOfBits = 32;
            var size = values.Count();
            var valuesArray = values.ToArray();
            var bitmaskData = new T[NumberOfBits];
            for (var i = 0; i < NumberOfBits; i++)
            {
                bitmaskData[i] = default;
            }

            for (var i = 0; i < size; i++)
            {
                var index = (NumberOfBits * i) / size;
                bitmaskData[index] = accumulatorFunction(bitmaskData[index], valuesArray[i]);
            }

            uint result = 0;
            for (var i = 0; i < NumberOfBits; i++)
            {
                result <<= 1;
                if (threshold.Invoke(bitmaskData[i]))
                {
                    result += 1;
                }
            }

            return result;
        }
    }
}

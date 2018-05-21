// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for doubles
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Test for approximate equality between this double and another
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The number to check against</param>
        /// <param name="epsilon">The accuracy tolerance.  Values within this range of b will be treated as equal.</param>
        /// <returns>True if this value is within epsilon of b, else false</returns>
        public static bool Equals(this double a, double b, double epsilon)
        {
            return (a >= b - epsilon && a <= b + epsilon);
        }

        /// <summary>
        /// Gets the sign of the double, expressed as +1 for positive numbers
        /// and -1 for negative ones.  Zero is treated as being positive in this
        /// instance.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Sign(this double value)
        {
            if (value >= 0) return 1;
            else return -1;
        }

        /// <summary>
        /// Returns the magnitude of this double with a sign matching the specified value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toThis"></param>
        /// <returns></returns>
        public static double MatchSign(this double value, double toThis)
        {
            if (value.Sign() != toThis.Sign()) return -value;
            else return value;
        }

        /// <summary>
        /// Raise this number to a power.
        /// Shortcut for Math.Pow(x,y)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power">The power to raise this number to</param>
        /// <returns></returns>
        public static double Power(this double value, double power)
        {
            return Math.Pow(value, power);
        }

        /// <summary>
        /// Multiply this number by itself
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Squared(this double value)
        {
            return value * value;
        }

        /// <summary>
        /// Calculate the square root of this number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Root(this double value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Return the absolute (i.e. unsigned) value of this number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Round this double to the nearest whole increment
        /// </summary>
        /// <param name="value"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static double Round(this double value, double increment = 1)
        {
            return Math.Round(value / increment) * increment;
        }

        /// <summary>
        /// Returns the smallest integral value which is greater than or equal to this number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Ceiling(this double value)
        {
            return Math.Ceiling(value);
        }

        /// <summary>
        /// Returns the largest integer value which is lower than or equal to this number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Floor(this double value)
        {
            return Math.Floor(value);
        }

        /// <summary>
        /// Limit this double to the specified range.
        /// Returns the value if it lies within the range, or the
        /// minimum or maximum limit if it falls outside that range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Limit(this double value, double min, double max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        /// <summary>
        /// Is this value within the specified range?
        /// i.e. is it > min and < max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this double value, double min, double max)
        {
            return value > min && value < max;
        }

        /// <summary>
        /// Is this value within the specified range?
        /// i.e. is it >= min and =< max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRangeInclusive(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Is this number really small?
        /// (-0.00000001 > value > 0.00000001)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTiny(this double value)
        {
            return (value > -0.00000001 && value < 0.00000001);
        }

        /// <summary>
        /// Is this Not A Number?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>Same as double.IsNaN(double) but less typing!</remarks>
        public static bool IsNaN(this double value)
        {
            return double.IsNaN(value);
        }

        /// <summary>
        /// Utility function to add one double to another in a thread-safe way.
        /// Equivalent to toBeModified += value, and similar to the Interlocked.Add()
        /// function for integers.
        /// </summary>
        /// <param name="toBeModified"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double InterlockedAdd(ref double toBeModified, double value)
        {
#if !JS
            double newCurrentValue = 0;
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref toBeModified, newValue, currentValue);
                if (newCurrentValue == currentValue)
                    return newValue;
            }
#else
            // Since multithreading doesn't work in JS anyway...
            return toBeModified += value;
#endif
        }

        /// <summary>
        /// Increment this number by the smallest step that will produce a differentiable
        /// floating point number greater than this one
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double NextValidValue(this double value)
        {
            long bits = BitConverter.DoubleToInt64Bits(value);
            return BitConverter.Int64BitsToDouble(bits + 1);
        }

        /// <summary>
        /// Interpolate between this value and another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="towards">The value to interpolate towards</param>
        /// <param name="factor">The interpolation factor. 0 = this value, 1 = the 'towards' value</param>
        /// <returns></returns>
        public static double Interpolate(this double value, double towards, double factor)
        {
            return value + (towards - value) * factor;
        }

        /// <summary>
        /// Interpolate between this value and another
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1">The value to be interpolated towards</param>
        /// <param name="x0">The key value mapped to this value</param>
        /// <param name="x1">The key value mapped to the other value</param>
        /// <param name="x">The key value at the position to be interpolated</param>
        /// <returns></returns>
        public static double Interpolate(this double v0, double v1, double x0, double x1, double x)
        {
            return v0 + (v1 - v0) * (x - x0) / (x1 - x0);
        }

        /// <summary>
        /// Remap this number from it's relative position in one interval to the same relative
        /// position in another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromThis"></param>
        /// <param name="toThis"></param>
        /// <returns></returns>
        public static double Remap(this double value, Interval fromThis, Interval toThis)
        {
            return toThis.ValueAt(fromThis.ParameterOf(value));
        }

        /// <summary>
        /// Round this number to the specified number of significant figures
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double RoundToSignificantFigures(this double value, int digits)
        {
            if (value == 0) return 0;
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            return scale * Math.Round(value / scale, digits);
        }

        /// <summary>
        /// Calculate the arithmetic mean of the values in this list
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Mean(this IList<double> values)
        {
            double total = 0;
            foreach (double v in values)
            {
                total += v;
            }
            return total / values.Count;
        }

        /// <summary>
        /// Calculate the standard deviation - the root of the average
        /// squared distance from the mean of the values in this list.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double StandardDeviation(this IList<double> values)
        {
            double mean = values.Mean();
            double totalD = 0;
            foreach (double v in values)
            {
                totalD += (v - mean).Squared();
            }
            totalD /= values.Count;
            return totalD.Root();
        }

        /// <summary>
        /// Wrap this value until it falls into the specified interval
        /// </summary>
        /// <param name="value"></param>
        /// <param name="interval">The interval to wrap to.  Size should be > 0.</param>
        /// <returns></returns>
        public static double WrapTo(this double value, Interval interval)
        {
            double size = interval.Size;
            if (size > 0)
            {
                while (value > interval.Max) value -= size;
                while (value < interval.Min) value += size;
            }
            return value;
        }


    }
}

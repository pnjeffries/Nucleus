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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for integers
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Is this integer even?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Is this integer odd?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        /// <summary>
        /// Get the number of digits used in the decimal representation
        /// of this integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Digits(this int value)
        {
            if (value == 0) return 1;
            else return (int)Math.Floor(Math.Log10(Math.Abs(value)) + 1);
        }

        /// <summary>
        /// Get the absolute value of this integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Is this value within the specified range?
        /// i.e. is it >= min and &lt;= max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Has this integer value exceeded a limit.  The condition to test may
        /// optionally be controlled by specifying the sign of the increment -
        /// if positive will return true if the value is higher than the limit,
        /// if negative will return true if the value is lower.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="limit"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static bool Exceeded(this int value, int limit, int sign = 1)
        {
            if (sign < 0) return value < limit;
            else return value > limit;
        }

        /// <summary>
        /// Find and return the index of the value in this list closest
        /// but lower than the specified value
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ClosestIndexBelow(this IList<int> list, int value)
        {
            int iBest = -1;
            int vBest = int.MinValue;
            for (int i = 0; i < list.Count; i++)
            {
                int v = list[i];
                if (v < value && (iBest < 0 || v > vBest))
                {
                    iBest = i;
                    vBest = v;
                }
            }
            return iBest;
        }

        /// <summary>
        /// 'Clamp' the integer to the specified range between
        /// minimum and maximum values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="minimum">The minimum value</param>
        /// <param name="maximum">The maximum value</param>
        /// <returns></returns>
        public static int Clamp(this int value, int minimum, int maximum)
        {
            if (value < minimum) return minimum;
            else if (value > maximum) return maximum;
            else return value;
        }

        /// <summary>
        /// 'Clamp' this integer to only acceptable values for the indices of the
        /// specified list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int ClampToIndices(this int value, IList list)
        {
            return value.Clamp(0, list.Count - 1);
        }

        /// <summary>
        /// Gets the sign of the integer, expressed as +1 for positive numbers
        /// and -1 for negative ones.  Zero is treated as being positive in this
        /// instance.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Sign(this int value)
        {
            if (value >= 0) return 1;
            else return -1;
        }

        /// <summary>
        /// Modify this list of ints in-place by adding the specified value
        /// to all entries in the list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="addThis"></param>
        public static void AddToAll(this IList<int> list, int addThis)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] += addThis;
            }
        }

        /// <summary>
        /// Raise this integer to a specified power.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power">The exponent to raise this number to.</param>
        /// <remarks>Based on fast methodology from: 
        /// https://stackoverflow.com/questions/383587/how-do-you-do-integer-exponentiation-in-c</remarks>
        /// <returns></returns>
        public static int Power(this int value, uint power)
        {
            return (int)Math.Pow(value, power);

            int result = 1;
            while (power != 0)
            {
                if ((power & 1) == 1)
                    result *= value;
                value *= value;
                power >>= 1;
            }
            return result;
        }
    }
}

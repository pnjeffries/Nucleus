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
        /// i.e. is it >= min and <= max
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
        
    }
}

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
    /// Extension method for arrays
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Convert this two-dimensional array into a separated string
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="entrySeparator">The string to use to separate each value</param>
        /// <param name="lineSeparator">The string used to separate each line</param>
        /// <param name="startWrapper">The string to be placed before each entry</param>
        /// <param name="endWrapper">The string to be placed after each entry</param>
        /// <returns>A string formatted in the specified way.</returns>
        public static string ToString(this object[,] array, string entrySeparator, 
            string lineSeparator, string startWrapper = null, string endWrapper = null)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.GetLength(1); i++)
            {
                if (i > 0) sb.Append(lineSeparator);
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (j > 0) sb.Append(entrySeparator);
                    if (startWrapper != null) sb.Append(startWrapper);
                    object value = array[i, j];
                    if (value != null) sb.Append(value);
                    if (endWrapper != null) sb.Append(endWrapper);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Convert this two-dimensional array into a separated string.
        /// Each row of the array will be separated by a newline character.
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="entrySeparator">The string to use to separate each value</param>
        /// <param name="startWrapper">The string to be placed before each entry</param>
        /// <param name="endWrapper">The string to be placed after each entry</param>
        /// <returns>A string formatted in the specified way.</returns>
        public static string ToString(this object[,] array, string entrySeparator,
            string startWrapper = null, string endWrapper = null)
        {
            return array.ToString(entrySeparator, Environment.NewLine, startWrapper, endWrapper);
        }

        /// <summary>
        /// Convert this two-dimensional array into a comma-separated-value string.
        /// Each entry of each row will be separated by a comma and each row of the 
        /// array will be separated by a newline character.
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="startWrapper">The string to be placed before each entry</param>
        /// <param name="endWrapper">The string to be placed after each entry</param>
        /// <returns>A string formatted in the specified way.</returns>
        public static string ToCSV(this object[,] array, string startWrapper = null, string endWrapper = null)
        {
            return array.ToString(",", startWrapper, endWrapper);
        }
    }
}

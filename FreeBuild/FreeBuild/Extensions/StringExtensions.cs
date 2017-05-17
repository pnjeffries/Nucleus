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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a version of this string with the first character captalised
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CapitaliseFirst(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            else
                return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// Returns a version of this string with spaces automatically placed before CamelCase capitals
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AutoSpace(this string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (char.IsUpper(c) && i > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Does this string represent an integer value?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(this string str)
        {
            return str.All(char.IsDigit);
        }

        /// <summary>
        /// Does this string represent a floating-point or
        /// integer value?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str)
        {
            double value;
            return double.TryParse(str, out value) && !double.IsNaN(value);
        }

        /// <summary>
        /// Removes all non-numeric characters from the start and end of this string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A new string with characters that do not count as numbers removed from the 
        /// start and end, or an empty string if there are no numbers at all in the starting 
        /// string.</returns>
        public static string TrimNonNumeric(this string str)
        {
            int firstNum = -1;
            int lastNum = -1;

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(str[i]))
                {
                    if (firstNum < 0) firstNum = i;
                    lastNum = i;
                }
            }
            if (firstNum < 0) //No numeric characters found!
                return "";
            else return str.Substring(firstNum, lastNum + 1 - firstNum);
        }

        /// <summary>
        /// Convert this string to a double.  If the conversion cannot be made,
        /// the specified fallback value (default: NaN) will be retured.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fallbackValue">The value to be returned in the case that this string
        /// cannot be successfully parsed into a double.</param>
        /// <returns></returns>
        public static double ToDouble(this string str, double fallbackValue = double.NaN)
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return result;
            }
            else return fallbackValue;
        }

        /// <summary>
        /// Removes all non-letter characters from the start and end of this string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A new string with characters that do not count as letters removed from the 
        /// start and end, or an empty string if there are no letters at all in the starting 
        /// string.  </returns>
        public static string TrimNonLetters(this string str)
        {
            int firstNum = -1;
            int lastNum = -1;

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsLetter(str[i]))
                {
                    if (firstNum < 0) firstNum = i;
                    lastNum = i;
                }
            }
            if (firstNum < 0) //No letter characters found!
                return "";
            else return str.Substring(firstNum, lastNum + 1 - firstNum);
        }

        /// <summary>
        /// Converts the string into a set of integer ID numbers.
        /// The string must consist only of numbers separated by spaces and the 'to' keyword,
        /// used to indicate continuous incementing ranges of 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ICollection<int> ToIDSet(this string str)
        {
            HashSet<int> result = new HashSet<int>();
            string[] tokens = str.Split(' ');
            int lastID = int.MaxValue;
            bool waitingForTo = false;
            foreach (string token in tokens)
            {
                string trimmed = token.Trim().Trim(new char[]{ ',' });
                if (!string.IsNullOrEmpty(trimmed))
                {
                    if (trimmed.IsInteger())
                    {
                        int parsed = int.Parse(trimmed);
                        if (waitingForTo)
                        {
                                for (int i = Math.Min(lastID, parsed); i < Math.Max(parsed, lastID); i++)
                                {
                                    if (!result.Contains(i)) result.Add(i);
                                }
                            
                        }
                        if (!result.Contains(parsed)) result.Add(parsed);
                        lastID = parsed;
                        waitingForTo = false;
                    }
                    else
                    {
                        int iTo = trimmed.IndexOf("to", StringComparison.CurrentCultureIgnoreCase);
                        if (iTo > 0)
                        {
                            //There is something before the 'to' - possibly the starting number?
                            string beforeTo = trimmed.Substring(0, iTo);
                            if (beforeTo.IsInteger()) lastID = int.Parse(beforeTo);
                            else
                                throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                        }
                        if (iTo >= 0)
                        {
                            //A 'to' exists somewhere in the token
                            if (trimmed.Length > iTo + 2)
                            {
                                //There is something after the 'to' - possibly the ending number
                                string afterTo = trimmed.Substring(iTo + 2);
                                if (afterTo.IsInteger())
                                {
                                    int endID = int.Parse(afterTo);
                                    for (int i = lastID; i <= endID; i++)
                                    {
                                        if (!result.Contains(i)) result.Add(i);
                                    }
                                }
                                else
                                    throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                            }
                            else waitingForTo = true;
                        }
                        else
                            throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether this string and another are equal, ignoring differences in case
        /// </summary>
        /// <param name="thisString"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string thisString, string other)
        {
            if (thisString == null) return (other == null);
            return thisString.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Count the number of non-null-or-whitespace entries in this array of strings
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static int NonEmptyCount(this IEnumerable<string> strings)
        {
            int count = 0;
            foreach (string value in strings)
            {
                if (!string.IsNullOrWhiteSpace(value)) count++;
            }
            return count;
        }

        /// <summary>
        /// Trim an occurrences of the specified suffix from the end of this string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="suffixToRemove"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {

            if (input != null && suffixToRemove != null)
            {
                while (input.EndsWith(suffixToRemove, comparisonType))
                {
                    input = input.Substring(0, input.Length - suffixToRemove.Length);
                }
            }
            return input;
        }

        /// <summary>
        /// Shorten this string to within the set maximum number of characters
        /// by truncating the middle of the string with an ellipsis
        /// </summary>
        /// <param name="input"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string TruncateMiddle(this string input, int maxChars, string separator = "...")
        {
            if (input.Length > maxChars)
            {
                int startLength = Math.Max((maxChars - separator.Length) / 2, 0);
                int endLength = Math.Max(maxChars - separator.Length - startLength, 0);
                return input.Substring(0, startLength) + separator + (input.Substring(input.Length - endLength));
            }
            else return input;
        }

        /// <summary>
        /// Get the chunk of text that starts from the specified index and proceeds
        /// up to the next found instance of the specified character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index">The start index of the chunk.  This will be updated to the
        /// start position of the next chunk</param>
        /// <param name="toChar"></param>
        /// <returns></returns>
        public static string NextChunk(this string str, ref int index, params char[] toChar)
        {
            int startIndex = index;
            index = str.IndexOfAny(toChar, startIndex) + 1;
            if (index <= 0)
            {
                index = str.Length;
                return str.Substring(startIndex);
            }
            else return str.Substring(startIndex, index - startIndex - 1);
        }
    }
}

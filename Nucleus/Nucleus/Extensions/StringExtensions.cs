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

namespace Nucleus.Extensions
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
        /// Are all of the strings in this set numeric?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(this IList<string> str)
        {
            foreach (string s in str) if (!s.IsNumeric()) return false;
            return true;
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
        /// the specified fallback value (default: NaN) will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fallbackValue">The value to be returned in the case that this string
        /// cannot be successfully parsed into a double.</param>
        /// <returns></returns>
        public static double ToDouble(this string str, double fallbackValue = (0.0/0.0))
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return result;
            }
            else return fallbackValue;
        }

        /// <summary>
        /// Convert this string to an integer.  If the conversion cannot be made,
        /// the specified fallback value (default: 0) will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fallbackValue">The value to be returned in the case that this string
        /// cannot be successfully parsed into an integer</param>
        /// <returns></returns>
        public static int ToInteger(this string str, int fallbackValue = 0)
        {
            int result;
            if (int.TryParse(str, out result)) return result;
            else return fallbackValue;
        }

        /// <summary>
        /// Convert this string to a Long.  If the conversion cannot be made,
        /// the specified fallback value (default: 0) will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fallbackValue">The value to be returned in the case that this string
        /// cannot be successfully parsed into a long</param>
        /// <returns></returns>
        public static long ToLong(this string str, long fallbackValue = 0)
        {
            long result;
            if (long.TryParse(str, out result)) return result;
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
        /// Get the set of contiguous letter characters from the start of this string up
        /// to the first space or non-letter character
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StartingLetters(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsLetter(str[i]))
                    return str.Substring(0, i);
            }
            return str;
        }

        /// <summary>
        /// Split this string using the separator characters ' ' and ','.
        /// String literals enclosed by '"' will be kept intact.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IList<string> TokeniseIgnoringLiterals(this string str)
        {
            var result = new List<string>();

            int iStart = 0;
            bool literal = false;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (literal)
                {
                    if (c == '"')
                    {
                        // End string literal
                        ExtractToken(str, ref iStart, i, i + 1, result);
                        literal = false;
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        // Start string literal
                        ExtractToken(str, ref iStart, i - 1, i, result);
                        literal = true;
                    }
                    else if (c == ' ' || c == ',') // Separator
                        ExtractToken(str, ref iStart, i - 1, i + 1, result);
                }
            }
            ExtractToken(str, ref iStart, str.Length - 1, 0, result); // Capture last token
            return result;

            // TODO: Brackets?
        }

        /// <summary>
        /// Extract a token from a string and add it to an output list
        /// </summary>
        /// <param name="str"></param>
        /// <param name="iStart"></param>
        /// <param name="i"></param>
        /// <param name="nexti"></param>
        /// <param name="output"></param>
        private static void ExtractToken(string str, ref int iStart, int i, int nexti, IList<string> output)
        {
            i++;
            if (i > iStart)
            {
                var subStr = str.Substring(iStart, i - iStart).Trim();
                if (subStr.Length > 0)
                    output.Add(subStr);
            }
            iStart = nexti;
        }

        /// <summary>
        /// Converts the string into a set of string IDs and names.
        /// Numerical entries separated by the keyword 'to' will be resolved
        /// to intermediate sequential numbers.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IList<string> TokeniseResolvingIDSequences(this string str)
        {
            var result = new List<string>();
            HashSet<long> intHash = new HashSet<long>();
            var tokens = str.TokeniseIgnoringLiterals();
            long lastID = long.MaxValue;
            bool waitingForTo = false;
            foreach (string token in tokens)
            {
                string trimmed = token.Trim().Trim(new char[] { ',' });
                if (!string.IsNullOrEmpty(trimmed))
                {
                    if (trimmed.IsInteger())
                    {
                        long parsed = long.Parse(trimmed);
                        if (waitingForTo)
                        {
                            for (long i = Math.Min(lastID, parsed); i < Math.Max(parsed, lastID); i++)
                            {
                                if (!intHash.Contains(i))
                                {
                                    intHash.Add(i);
                                    result.Add(i.ToString());
                                }
                            }

                        }
                        if (!intHash.Contains(parsed))
                        {
                            intHash.Add(parsed);
                            result.Add(token);
                        }
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
                                result.Add(token);
                                //throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
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
                                    long endID = long.Parse(afterTo);
                                    for (long i = lastID; i <= endID; i++)
                                    {
                                        if (!intHash.Contains(i))
                                        {
                                            intHash.Add(i);
                                            result.Add(i.ToString());
                                        }
                                    }
                                }
                                else
                                    result.Add(token);
                                    //throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                            }
                            else waitingForTo = true;
                        }
                        else
                            result.Add(token);
                            //throw new ArgumentException("The ID description was invalid.  Only integer ID numbers and the keyword 'to' are allowed.");
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Converts the string into a set of integer ID numbers.
        /// The string must consist only of numbers separated by spaces and the 'to' keyword,
        /// used to indicate continuous incementing ranges of ID numbers.
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
        /// Remove all whitespace characters from this string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveWhitespace(this string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
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

#if !JS

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
#endif

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
        /// Shorten this string to within the set maximum number of characters
        /// by compressing the non-capitalised parts of the string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string TruncatePascal(this string input, int maxChars)
        {
            if (input.Length > maxChars)
            {
                var sb = new StringBuilder();
                int count = input.CountUpper();
                if (count == 0) return input.Substring(0, maxChars); //All lower-case
                int segLength = maxChars / count;
                if (segLength < 1) segLength = 1;
                int i = 0;
                while ((i = input.IndexOfUpper(i)) >= 0 && sb.Length < maxChars)
                {
                    for (int j = 0; j < segLength; j++)
                    {
                        sb.Append(input[i + j]);
                    }
                    i += segLength;
                }
                return sb.ToString();
            }
            else return input;
        }

        /// <summary>
        /// Overwrite the ending characters of this string with the specified new ending
        /// </summary>
        /// <param name="input"></param>
        /// <param name="newEnd"></param>
        /// <returns></returns>
        public static string OverwriteEnd(this string input, string newEnd)
        {
            if (input.Length > newEnd.Length)
            {
                return input.Substring(0, input.Length - newEnd.Length) + newEnd;
            }
            else return newEnd;
        }

        /// <summary>
        /// Count the number of upper-case characters in this string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int CountUpper(this string input)
        {
            int count = 0;
            foreach (char c in input)
            {
                if (char.IsUpper(c)) count++;
            }
            return count;
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
            char c;
            return str.NextChunk(out c, ref index, toChar);
        }

        /// <summary>
        /// Get the chunk of text that starts from the specified index and proceeds
        /// up to the next found instance of the specified character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="terminator">The found character which terminates this chunk</param>
        /// <param name="index">The start index of the chunk.  This will be updated to the
        /// start position of the next chunk</param>
        /// <param name="toChar"></param>
        /// <returns></returns>
        public static string NextChunk(this string str, out char terminator, ref int index, params char[] toChar)
        {
            int startIndex = index;
            index = str.IndexOfAny(toChar, startIndex);
            if (index < 0)
            {
                terminator = default(char);
                index = str.Length;
                return str.Substring(startIndex);
            }
            else
            {
                terminator = str[index];
                index += 1;
                return str.Substring(startIndex, index - startIndex - 1);
            }
        }

        /// <summary>
        /// Get the first substring between matching open and close brackets in this string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startFrom"></param>
        /// <param name="openBracket"></param>
        /// <param name="closeBracket"></param>
        /// <returns></returns>
        public static string NextBracketed(this string str, char openBracket = '(', char closeBracket = ')')
        {
            int startFrom = 0;
            return NextBracketed(str, ref startFrom, openBracket, closeBracket);
        }

        /// <summary>
        /// Get the first substring between matching open and close brackets in this string
        /// after the specified position.  Returns null if a matching pair of brackets is not found.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex">The index to start searching from.  Will be modified to give
        /// the position of the opening bracket, or the last index in the string + 1 if nothing
        /// was found.</param>
        /// <param name="openBracket">The opening bracket character</param>
        /// <param name="closeBracket">The closing bracket character</param>
        /// <returns></returns>
        public static string NextBracketed(this string str, ref int startIndex, char openBracket = '(', char closeBracket = ')')
        {
            int s0 = str.IndexOf(openBracket, startIndex);
            if (s0 >= 0)
            {
                int bCount = 1;
                for (int i = s0 + 1; i < str.Length; i++)
                {
                    char c = str[i];
                    if (c == openBracket) bCount++;
                    if (c == closeBracket) bCount--;
                    if (bCount == 0) //Back to opening bracket level
                    {
                        startIndex = s0;
                        return str.Substring(s0 + 1, i - s0 - 1);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Return the portion of this string before the first instance
        /// of the specified character.  If the character is not present
        /// in the string then the entire original string will be returned.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Before(this string str, char character)
        {
            int index = str.IndexOf(character);
            if (index >= 0) return str.Substring(0, index);
            else return str;
        }

        /// <summary>
        /// Split up a text string to different hyperlinks and the fragments of text
        /// between them.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IList<string> SplitHyperlinks(this string str)
        {
            var result = new List<string>();
            int startIndex = 0;
            int lastEndIndex = 0;
            for (int i = 0; i <= str.Length; i++)
            {
                if (i == str.Length || char.IsWhiteSpace(str[i]) || str[i] == '(' || str[i] == ')')
                {
                    string word = str.Substring(startIndex, i - startIndex);
                    if (word.IsURI())
                    {
                        result.Add(str.Substring(lastEndIndex, startIndex - lastEndIndex));
                        result.Add(word);
                        lastEndIndex = i;
                    }
                    else if (i == str.Length)
                    {
                        result.Add(str.Substring(lastEndIndex));
                    }
                    startIndex = i + 1;
                }
            }
            return result;
        }

        /// <summary>
        /// Can this string be parsed as a valid URI?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsURI(this string str)
        {
            Uri uri;
#if !JS
            return Uri.TryCreate(str, UriKind.Absolute, out uri) && 
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
#else
            try
            {
                uri = new Uri(str);
            }
            catch 
            {
                return false;
            }
            return true;
#endif
        }

        /// <summary>
        /// Find the index of the first capital letter at or after the specified start index
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int IndexOfUpper(this string str, int startIndex)
        {
            for (int i = startIndex; i < str.Length; i++)
            {
                if (char.IsUpper(str[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Does this string contain the specified substring, starting from the specified
        /// index?
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AppearsAt(this string str, int startIndex, string value)
        {
            if (startIndex >= 0 && startIndex <= str.Length - value.Length)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (str[startIndex + i] != value[i]) return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the portion of this string before the last instance of the specified 
        /// character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string BeforeLast(this string str, char c)
        {
            int i = str.LastIndexOf(c);
            if (i > 0)
            {
                return str.Substring(0, i);
            }
            else return str;
        }

        /// <summary>
        /// Get the portion of this string after the last instance of the specified character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string AfterLast(this string str, char c)
        {
            int i = str.LastIndexOf(c);
            if (i > 0)
            {
                return str.Substring(i + 1);
            }
            else return str;
        }

        /// <summary>
        /// Explode this string to a list of strings where each entry is a separate character
        /// from the original string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IList<string> ToCharSubstrings(this string str)
        {
            var result = new List<string>(str.Length);
            foreach (var c in str)
            {
                result.Add(new string(new char[] { c }));
            }
            return result;
        }

        /// <summary>
        /// Return the block of text from the specified index to the next instance of one
        /// of the specified characters
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index">The index to start from.  This will be modified to give
        /// the index of the first found character</param>
        /// <returns></returns>
        public static string ToNext(this string str, ref int index, params char[] openChar)
        {
            int startIndex = index;
            index = str.IndexOfAny(openChar, index);
            return str.Substring(startIndex, index - startIndex);
        }

    }
}

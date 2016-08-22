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
            return str.All(Char.IsDigit);
        }

        /// <summary>
        /// Converts the string into a set of integer ID numbers.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ICollection<int> ToIDSet(this string str)
        {
            HashSet<int> result = new HashSet<int>();
            string[] tokens = str.Split(' ');
            foreach (string token in tokens)
            {
                string trimmed = token.Trim().Trim(new char[]{ ',' });
                if (!string.IsNullOrEmpty(trimmed))
                {
                    if (trimmed.IsInteger())
                    {
                        int parsed = int.Parse(trimmed);
                        if (!result.Contains(parsed)) result.Add(parsed);
                    }
                    else
                    {
                        throw new ArgumentException("The ID description was invalid.  Only integer ID numbers are allowed.");
                    }
                }
            }
            return result;
        }
    }
}

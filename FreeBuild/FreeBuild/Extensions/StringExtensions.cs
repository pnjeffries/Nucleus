using System;
using System.Collections.Generic;
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the StringBuilderClass
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends a copy of the specified string value to this instance,
        /// preceding it with the specified separator character if something has
        /// already been appended to this StringBuilder.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static StringBuilder Append(this StringBuilder sb, string value, string separator)
        {
            if (sb.Length > 0) sb.Append(separator);
            return sb.Append(value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// Extension methods for integers
    /// </summary>
    public static class IntExtensionscs
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
    }
}

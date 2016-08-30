using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// Extension methods for doubles
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Gets the sign of the double, expressed as +1 for positive numbers
        /// and -1 for negative ones.  Zero is treated as being positive in this
        /// instance.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Sign(this double value)
        {
            if (value > 0) return 1;
            else return -1;
        }
    }
}

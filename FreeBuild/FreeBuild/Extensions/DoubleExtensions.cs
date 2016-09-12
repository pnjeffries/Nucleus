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
            if (value >= 0) return 1;
            else return -1;
        }

        /// <summary>
        /// Raise this number to a power.
        /// Shortcut for Math.Pow(x,y)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power">The power to raise this number to</param>
        /// <returns></returns>
        public static double Power(this double value, double power)
        {
            return Math.Pow(value, power);
        }

        /// <summary>
        /// Multiply this number by itself
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Squared(this double value)
        {
            return value * value;
        }

        /// <summary>
        /// Calculate the square root of this number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Root(this double value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Return the absolute (i.e. unsigned) value of this number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }
    }
}

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
        public static int Sign(this double value)
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

        /// <summary>
        /// Round this double to the nearest whole increment
        /// </summary>
        /// <param name="value"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static double Round(this double value, double increment = 1)
        {
            return Math.Round(value / increment) * increment;
        }

        /// <summary>
        /// Limit this double to the specified range.
        /// Returns the value if it lies within the range, or the
        /// minimum or maximum limit if it falls outside that range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Limit(this double value, double min, double max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        /// <summary>
        /// Is this value within the specified range?
        /// i.e. is it > min and < max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this double value, double min, double max)
        {
            return value > min && value < max;
        }

        /// <summary>
        /// Is this value within the specified range?
        /// i.e. is it >= min and =< max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRangeInclusive(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Is this number really small?
        /// (-0.00000001 > value > 0.00000001)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTiny(this double value)
        {
            return (value > -0.00000001 && value < 0.00000001);
        }
    }
}

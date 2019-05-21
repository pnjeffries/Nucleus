using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the IComparable interface
    /// </summary>
    public static class IComparableExtensions
    {
        /// <summary>
        /// 'Clamp' the comparable object to the specified range between
        /// minimum and maximum values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="minimum">The minimum value</param>
        /// <param name="maximum">The maximum value</param>
        /// <returns></returns>
        public static T Clamp<T>(this T value, T minimum, T maximum)
            where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0) return minimum;
            if (value.CompareTo(maximum) > 0) return maximum;
            else return value;
        }
    }
}

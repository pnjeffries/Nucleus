using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the IDictionary interface
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Get the range of keys in this dictionary
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static Interval KeyRange<TValue>(this IDictionary<double, TValue> dictionary)
        {
            Interval result = Interval.Unset;
            foreach (KeyValuePair<double, TValue> kvp in dictionary)
            {
                if (!result.IsValid) result = new Maths.Interval(kvp.Key);
                else result = result.Include(kvp.Key);
            }
            return result;
        }
    }
}

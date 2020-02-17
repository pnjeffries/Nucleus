using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the short datatype
    /// </summary>
    public static class ShortExtensions
    {
        /// <summary>
        /// Convert this list of shorts to a list of ints
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<int> ToInts(this IList<short> list)
        {
            var result = new List<int>(list.Count);
            foreach (short shorty in list) result.Add(Convert.ToInt32(shorty));
            return result;
        }
    }
}

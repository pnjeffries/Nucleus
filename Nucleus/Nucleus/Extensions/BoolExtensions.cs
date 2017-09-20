using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for bool and bool? types
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Convert this nullable bool to a plain old
        /// bool.  If it does not have a value, that will be
        /// treated as false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this bool? value)
        {
            if (value.HasValue) return value == true;
            else return false;
        }
    }
}

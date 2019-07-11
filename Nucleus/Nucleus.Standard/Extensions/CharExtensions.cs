using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the standard Character type
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Is this char equal to another, ignoring the case of the character
        /// </summary>
        /// <param name="c"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this char c, char value)
        {
            //TODO: Can this be made more efficient?
            return c.ToString().EqualsIgnoreCase(value.ToString());
        }
    }
}

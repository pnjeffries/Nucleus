using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// ICollection extension methods
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Does the specified collection contain any item of the specified type?
        /// </summary>
        /// <param name="col"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ContainsType(this ICollection col, Type type)
        {
            foreach (object item in col)
            {
                if (type.IsAssignableFrom(item.GetType()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

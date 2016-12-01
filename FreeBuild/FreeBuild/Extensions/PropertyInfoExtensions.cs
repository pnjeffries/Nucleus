using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// Extension methods for PropertyInfo objects
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Does this property have an attribute of the specified type?
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static bool HasAttribute(this PropertyInfo property, Type attributeType)
        {
            return property.GetCustomAttribute(attributeType) != null;
        }
    }
}

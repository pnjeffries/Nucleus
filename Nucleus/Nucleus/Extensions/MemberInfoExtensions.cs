using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nucleus.Extensions
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Retrieves a custom attribute applied to this member
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            return Attribute.GetCustomAttribute(memberInfo, typeof(TAttribute)) as TAttribute;
        }
    }
}

// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using FreeBuild.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// Extension methods on types and collections of types
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The number of levels of inheritance between this type and a type that
        /// is somewhere in its inheritance chain.
        /// </summary>
        /// <param name="type">This type</param>
        /// <param name="ancestorType">A type which is an ancestor of this one</param>
        /// <returns>If the specified type is an ancestor of this one, the number of
        /// inheritance levels between the two types.  If the specified type is this 
        /// type, 0.  If the specified type cannot be found in the inheritance chain,
        /// -1.</returns>
        public static int InheritanceLevelsTo(this Type type, Type ancestorType)
        {
            int count = 0;
            while (type != null && type != ancestorType)
            {
                count++;
                type = type.BaseType;
            }
            if (type == ancestorType) return count;
            else return -1;
        }

        /// <summary>
        /// Find the type in this set of types which is the least number of
        /// inheritance levels above the specified type.
        /// </summary>
        /// <param name="forType">The type to seach for</param>
        /// <param name="inTypes">The collection of types to look within</param>
        /// <returns>The type in this collection that is closest in the inheritance
        /// hierarchy to the specified type.  Or, null if the type does not have an
        /// ancestor in the collection.</returns>
        public static Type ClosestAncestor(this IEnumerable<Type> inTypes, Type forType)
        {
            int minDist = -1;
            Type closest = null;
            foreach (Type ancestorType in inTypes)
            {
                int dist = forType.InheritanceLevelsTo(ancestorType);
                if (dist >= 0 && (minDist < 0 || dist < minDist))
                {
                    minDist = dist;
                    closest = ancestorType;
                }
            }
            return closest;
        }

        /// <summary>
        /// Find the type in this set of types which is the least number of
        /// inheritance levels below the specified type.
        /// </summary>
        /// <param name="forType">The type to seach for</param>
        /// <param name="inTypes">The collection of types to look within</param>
        /// <returns>The type in this collection that is closest in the inheritance
        /// hierarchy to the specified type.  Or, null if the type does not have a
        /// descendent in the collection.</returns>
        public static Type ClosestDescendent(this IEnumerable<Type> inTypes, Type forType)
        {
            int minDist = -1;
            Type closest = null;
            foreach (Type descendentType in inTypes)
            {
                int dist = descendentType.InheritanceLevelsTo(forType);
                if (dist >= 0 && (minDist < 0 || dist < minDist))
                {
                    minDist = dist;
                    closest = descendentType;
                }
            }
            return closest;
        }

        /// <summary>
        /// Is this a collection type? i.e. does it implement ICollection?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollection(this Type type)
        {
            return typeof(ICollection).IsAssignableFrom(type)
                   || typeof(ICollection<>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Is this an enumerable type?  i.e. does it implement IEnumerable?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type)
                   || typeof(IEnumerable<>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Is this a List type?  i.e. does it implement IList?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(this Type type)
        {
            return typeof(IList).IsAssignableFrom(type)
                   || typeof(IList<>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Extract all properties from this type that have been annotated with an AutoUIAttribute,
        /// sorted by their order.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<PropertyInfo> GetAutoUIProperties(this Type type)
        {
            SortedList<double, PropertyInfo> result = new SortedList<double, PropertyInfo>();
            PropertyInfo[] pInfos = type.GetProperties();
            foreach (PropertyInfo pInfo in pInfos)
            {
                object[] attributes = pInfo.GetCustomAttributes(typeof(AutoUIAttribute), true);
                if (attributes.Count() > 0)
                {
                    AutoUIAttribute aInput = (AutoUIAttribute)attributes[0];
                    double keyValue = aInput.Order;
                    while (result.ContainsKey(keyValue)) keyValue += 0.0000001;
                    result.Add(keyValue, pInfo);
                }
            }
            return result.Values.ToList<PropertyInfo>();
        }
    }
}

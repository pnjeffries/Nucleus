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

using FreeBuild.Extensions;
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
        /// Extract all members from this type that have been annotated with an AutoUIAttribute,
        /// sorted by their order.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<MemberInfo> GetAutoUIMembers(this Type type)
        {
            SortedList<double, MemberInfo> result = new SortedList<double, MemberInfo>();
            MemberInfo[] mInfos = type.GetMembers();
            foreach (MemberInfo mInfo in mInfos)
            {
                object[] attributes = mInfo.GetCustomAttributes(typeof(AutoUIAttribute), true);
                if (attributes.Count() > 0)
                {
                    AutoUIAttribute aInput = (AutoUIAttribute)attributes[0];
                    double keyValue = aInput.Order;
                    while (result.ContainsKey(keyValue)) keyValue = keyValue.NextValidValue();
                    result.Add(keyValue, mInfo);
                }
            }
            return result.Values.ToList();
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
                    while (result.ContainsKey(keyValue)) keyValue = keyValue.NextValidValue();
                    result.Add(keyValue, pInfo);
                }
            }
            return result.Values.ToList();
        }

        /// <summary>
        /// Extract all properties from this set of types that have been annotated with an
        /// AutoUIAttribute, sorted by their order.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<PropertyInfo> GetAutoUIProperties(this IEnumerable<Type> types)
        {
            SortedList<double, PropertyInfo> result = new SortedList<double, PropertyInfo>();
            foreach (Type type in types)
            {
                PropertyInfo[] pInfos = type.GetProperties();
                foreach (PropertyInfo pInfo in pInfos)
                {
                    object[] attributes = pInfo.GetCustomAttributes(typeof(AutoUIAttribute), true);
                    if (attributes.Count() > 0)
                    {
                        AutoUIAttribute aInput = (AutoUIAttribute)attributes[0];
                        double keyValue = aInput.Order;
                        while (result.ContainsKey(keyValue)) keyValue = keyValue.NextValidValue();
                        result.Add(keyValue, pInfo);
                    }
                }
            }
            return result.Values.ToList();
        }

        /// <summary>
        /// Extract all methods from this type that have been annotated with an AutoUIAttribute,
        /// sorted by their order.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<MethodInfo> GetAutoUIMethods(this Type type)
        {
            SortedList<double, MethodInfo> result = new SortedList<double, MethodInfo>();
            MethodInfo[] mInfos = type.GetMethods();
            foreach (MethodInfo mInfo in mInfos)
            {
                object[] attributes = mInfo.GetCustomAttributes(typeof(AutoUIAttribute), true);
                if (attributes.Count() > 0)
                {
                    AutoUIAttribute aInput = (AutoUIAttribute)attributes[0];
                    double keyValue = aInput.Order;
                    while (result.ContainsKey(keyValue)) keyValue = keyValue.NextValidValue();
                    result.Add(keyValue, mInfo);
                }
            }
            return result.Values.ToList();
        }

        /// <summary>
        /// Get a list of all the non-abstract types that derive from this type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="allAssemblies">If true, all loaded assembles will be checked, else only the assembly the 
        /// base type is defined in.</param>
        /// <returns></returns>
        public static IList<Type> GetSubTypes(this Type type, bool allAssemblies = false)
        {
            IList<Type> result = new List<Type>();
            if (allAssemblies)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        foreach (Type subType in assembly.GetTypes())
                        {
                            if (subType.IsSubclassOf(type) && !subType.IsAbstract) result.Add(subType);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                Assembly assembly = type.Assembly;
                foreach (Type subType in assembly.GetTypes())
                {
                    if (subType.IsSubclassOf(type) && !subType.IsAbstract) result.Add(subType);
                }
            }
            return result;
        }

        /// <summary>
        /// Get all fields of this type, including private ones inherited from base classes
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outFields">The collection of field infos to be populated</param>
        /// <param name="flags">A bitmask composed of one or more BindingFlags which specify 
        /// how the search is conduted</param>
        public static void GetAllFields(this Type type, ICollection<FieldInfo> outFields, BindingFlags flags)
        {
            foreach (var field in type.GetFields(flags))
            {
                // Ignore inherited fields.
                if (field.DeclaringType == type) //Necessary?
                    outFields.Add(field);
            }

            var baseType = type.BaseType;
            if (baseType != null)
                baseType.GetAllFields(outFields, flags);
        }

        /// <summary>
        /// Get all fields of this type, including private ones inherited from base classes
        /// </summary>
        /// <param name="type"></param>
        /// <param name="flags">A bitmask composed of one or more BindingFlags which specify 
        /// how the search is conduted</param>
        public static ICollection<FieldInfo> GetAllFields(this Type type, BindingFlags flags)
        {
            var result = new List<FieldInfo>();
            type.GetAllFields(result, flags);
            return result;
        }

        /// <summary>
        /// Searches for the specified field recursively.  If it cannot be found within this type,
        /// the base class hierarchy will be searched also.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">The name of the field to find</param>
        /// <param name="flags">>A bitmask composed of one or more BindingFlags which specify 
        /// how the search is conduted</param>
        /// <returns>The FieldInfo if found, else null</returns>
        public static FieldInfo GetBaseField(this Type type, string name, BindingFlags flags)
        {
            FieldInfo result = type.GetField(name, flags);
            if (result == null)
            {
                var baseType = type.BaseType;
                if (baseType != null)
                    result = baseType.GetBaseField(name, flags);
            }
            return result;
        }

        /// <summary>
        /// Does this type posess a parameterless constructor
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasParameterlessConstructor(this Type type)
        {
            return type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                        null, Type.EmptyTypes, null) != null;
        }

        /// <summary>
        /// Get a collection of all types which this type relies on for its definition
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ignoreNonSerialised"></param>
        /// <returns></returns>
        public static ICollection<Type> GetDependencies(this Type type, bool ignoreNonSerialised = false)
        {
            HashSet<Type> result = new HashSet<Type>();
            GetDependencies(type, result, ignoreNonSerialised);
            return result;
        }

        public static void GetDependencies(this Type type, ICollection<Type> output, bool ignoreNonSerialised = false)
        {
            if (!output.Contains(type))
            {
                output.Add(type);
                if (type.BaseType != null) type.BaseType.GetDependencies(output, ignoreNonSerialised);
                foreach (FieldInfo fI in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!ignoreNonSerialised || fI.GetCustomAttribute(typeof(NonSerializedAttribute)) == null)
                    fI.FieldType.GetDependencies(output, ignoreNonSerialised);
                }
            }
        }

        /// <summary>
        /// Get the type of the items stored within this collection type
        /// </summary>
        /// <param name="collectionType"></param>
        /// <returns></returns>
        public static Type GetItemType(this Type collectionType)
        {
            Type iEnum = FindIEnumerable(collectionType);
            if (iEnum == null) return collectionType;
            else return iEnum.GetGenericArguments()[0];
        }

        /// <summary>
        /// Get the generic IEnumerable type of the specified collection type
        /// </summary>
        /// <param name="collectionType"></param>
        /// <returns></returns>
        private static Type FindIEnumerable(Type collectionType)
        {
            if (collectionType != null)
            {
                if (collectionType.IsGenericType)
                {
                    foreach (Type arg in collectionType.GetGenericArguments())
                    {
                        Type iEnum = typeof(IEnumerable<>).MakeGenericType(arg);
                        if (iEnum.IsAssignableFrom(collectionType)) return iEnum;
                    }
                }
                Type[] interfaces = collectionType.GetInterfaces();
                if (interfaces != null && interfaces.Length > 0)
                {
                    foreach (Type iface in interfaces)
                    {
                        Type iEnum = FindIEnumerable(iface);
                        if (iEnum != null) return iEnum;
                    }
                }
                if (collectionType.BaseType != null && collectionType.BaseType != typeof(object))
                    return FindIEnumerable(collectionType.BaseType);
            }
            return null;
        }

    }
}

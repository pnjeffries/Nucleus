﻿// Copyright (c) 2016 Paul Jeffries
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
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

        /// <summary>
        /// Retrieves a custom attribute applied to this property
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this PropertyInfo propertyInfo)
            where TAttribute : Attribute
        {
#if !JS
            return Attribute.GetCustomAttribute(propertyInfo, typeof(TAttribute)) as TAttribute;
#else
            var attributes = Attribute.GetCustomAttributes(propertyInfo, typeof(TAttribute));
            if (attributes.Length > 0) return attributes[0] as TAttribute;
            else return null;
#endif
        }

        /// <summary>
        /// Retrieves a custom attribute applied to this property
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Attribute GetCustomAttribute(this PropertyInfo propertyInfo, Type attributeType)
        {
#if !JS
            return Attribute.GetCustomAttribute(propertyInfo, attributeType);
#else
            var attributes = Attribute.GetCustomAttributes(propertyInfo, attributeType);
            if (attributes.Length > 0) return attributes[0];
            else return null;
#endif
        }
    }
}

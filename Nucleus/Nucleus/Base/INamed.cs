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

using Nucleus.Extensions;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Interface for objects which can be named
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// The name of this object
        /// </summary>
        [AutoUI(100)]
        string Name { get; }
    }

    /// <summary>
    /// Extension methods for INamed objects and collections thereof
    /// </summary>
    public static class INamedExtensions
    {
        /// <summary>
        /// Find an item in this collection by name.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="collection"></param>
        /// <param name="name">The name of the item to find.  Not case sensitive.</param>
        /// <returns>The first encountered item in this collection with the given name.</returns>
        public static TItem FindByName<TItem> (this IEnumerable<TItem> collection, string name) where TItem: class, INamed
        {
            foreach (TItem item in collection)
            {
                if (item.Name.EqualsIgnoreCase(name)) return item;
            }
            return null;
        }

        /// <summary>
        /// Create a string which lists all of the names of the objects in this collection
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="collection"></param>
        /// <param name="separator"></param>
        /// <param name="nullReplacement"></param>
        /// <returns></returns>
        public static string ToNames<TItem>(this IEnumerable<TItem> collection, string separator = ", ", string nullReplacement = "")
            where TItem: INamed
        {
            var sb = new StringBuilder();
            foreach (TItem item in collection)
            {
                if (sb.Length > 0) sb.Append(separator);
                if (item.Name == null) sb.Append(nullReplacement);
                sb.Append(item.Name);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get a list of all of the names of the objects in this enumerable
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IList<string> GetNamesList<TItem>(this IEnumerable<TItem> collection)
            where TItem: INamed
        {
            var result = new List<string>();
            foreach (TItem item in collection)
                result.Add(item.Name);
            return result;
        }
    }
}

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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for IEnumerable objects
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Get a collection of all the unique types currently contained within this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ContainedTypes<T>(this IEnumerable<T> collection)
        {
            HashSet<Type> set = new HashSet<Type>();
            foreach(T obj in collection)
            {
                Type type = obj.GetType();
                if (!set.Contains(type)) set.Add(type);
            }
            return set;
        }

        /// <summary>
        /// Does this enumerable contain only items of the specified type, or types
        /// which inherit from it?
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ContainsOnlyType(this IEnumerable collection, Type type)
        {
            foreach (var obj in collection)
            {
                if (!type.IsAssignableFrom(obj.GetType())) return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieve the combined value of a member of all items in this collection,
        /// expressed as a lambda function.  If the value of that member is consistent across all items
        /// in this collection then that value will be returned, else the specified value will be returned
        /// to indicate multiple values.
        /// </summary>
        /// <param name="propertyDelegate">A lambda function that returns a particular property for each item in the selection</param>
        /// <param name="multiValue">The value that should be returned to indicate multiple inconsistent values</param>
        /// <param name="nullValue">The value that should be returned if there are no items in this collection</param>
        /// <returns>The consistent value that is shared between all items, if that is the case, else the input multiValue</returns>
        public static TValue CombinedValue<TItem, TValue>(this IEnumerable<TItem> enumerable, Func<TItem, TValue> propertyDelegate, TValue multiValue = default(TValue), TValue nullValue = default(TValue))
        {
            IEnumerable<TValue> values = enumerable.Select(propertyDelegate);
            TValue combinedValue = nullValue;
            bool first = true;
            foreach (TValue value in values)
            {
                if (first)
                {
                    first = false;
                    combinedValue = value;
                }
                else if (!Equals(value, combinedValue))
                    return multiValue;
            }
            //if (combinedValue == null) combinedValue = nullValue; //?
            return combinedValue;
        }
    }
}

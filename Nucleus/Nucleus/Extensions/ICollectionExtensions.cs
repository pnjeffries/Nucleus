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
    /// ICollection extension methods
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Does this collection contain any item of the specified type?
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

        /// <summary>
        /// Does this collection contain only objects of the given type or its subclasses?
        /// </summary>
        /// <param name="col"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ContainsOnlyType(this ICollection col, Type type)
        {
            foreach (object item in col)
            {
                if (!type.IsAssignableFrom(item.GetType()))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Extract all of the objects of a specified type into a separate collection
        /// </summary>
        /// <typeparam name="T">The type of object to extract from this collection</typeparam>
        /// <param name="col"></param>
        /// <param name="toCollection">The collection to which the objects will be added</param>
        public static void ExtractAllOfType<T>(this ICollection col, ICollection<T> toCollection)
        {
            foreach (object item in col)
            {
                if (item is T)
                {
                    toCollection.Add((T)item);
                }
            }
        }

        /// <summary>
        /// Extract all of the objects of a specified type into a separate collection
        /// </summary>
        /// <typeparam name="T">The type of object to extract from this collection</typeparam>
        /// <param name="col"></param>
        /// <param name="toCollection">The collection to which the objects will be added</param>
        public static void ExtractAllOfType<T>(this ICollection<T> col, Type type, ICollection<T> toCollection)
        {
            foreach (object item in col)
            {
                if (item != null && type.IsAssignableFrom(item.GetType()))
                {
                    toCollection.Add((T)item);
                }
            }
        }

        /// <summary>
        /// Create a string containing the ToString() results of all objects in the collection
        /// separated by the optionally specified separator sequence.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="separator">The separator string to place between items</param>
        /// <returns></returns>
        public static string ToFlatString<T>(this ICollection<T> col, string separator = " ")
        {
            var sb = new StringBuilder();
            foreach (object obj in col)
            {
                if (obj != null)
                {
                    if (sb.Length > 0) sb.Append(separator);
                    sb.Append(obj.ToString());
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Create a string containing all of the integer values in the list, but with consecutive values shortened 
        /// to the form 'A to B'.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="separator"></param>
        /// <param name="bridge"></param>
        /// <returns></returns>
        public static string ToCompressedString(this ICollection<int> col, string separator = " ", string bridge = " to ")
        {
            var sb = new StringBuilder();
            int last = int.MaxValue;
            int rangeStart = 0;
            bool inSequence = false;
            foreach (int i in col)
            {
                if (last == i - 1)
                {
                    if (!inSequence)
                    {
                        rangeStart = last;
                        inSequence = true;
                    }
                }
                else
                {
                    if (inSequence)
                    {
                        if (last > rangeStart + 1) sb.Append(bridge);
                        else sb.Append(separator);
                        sb.Append(last);
                    }
                    if (sb.Length > 0) sb.Append(separator);
                    sb.Append(i);
                    inSequence = false;
                }
                last = i;
            }
            if (inSequence)
            {
                if (last > rangeStart + 1) sb.Append(bridge);
                else sb.Append(separator);
                sb.Append(last);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Create a string containing all of the integer values in the list, but with consecutive values shortened 
        /// to the form 'A to B'.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="separator"></param>
        /// <param name="bridge"></param>
        /// <returns></returns>
        public static string ToCompressedString(this ICollection<long> col, string separator = " ", string bridge = " to ")
        {
            var sb = new StringBuilder();
            long last = long.MaxValue;
            long rangeStart = 0;
            bool inSequence = false;
            foreach (long i in col)
            {
                if (last == i - 1)
                {
                    if (!inSequence)
                    {
                        rangeStart = last;
                        inSequence = true;
                    }
                }
                else
                {
                    if (inSequence)
                    {
                        if (last > rangeStart + 1) sb.Append(bridge);
                        else sb.Append(separator);
                        sb.Append(last);
                    }
                    if (sb.Length > 0) sb.Append(separator);
                    sb.Append(i);
                    inSequence = false;
                }
                last = i;
            }
            if (inSequence)
            {
                if (last > rangeStart + 1) sb.Append(bridge);
                else sb.Append(separator);
                sb.Append(last);
            }
            return sb.ToString();
        
        }
    }
}

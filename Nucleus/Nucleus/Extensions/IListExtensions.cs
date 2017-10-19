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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    public static class IListExtensions
    {
        /// <summary>
        /// Get the item at the specified index, automatically limiting
        /// the index to the bounds of the list.  Entering an index less than
        /// zero will return the first item.  Entering an index greater than the
        /// size of the array will return the last item.
        /// Optionally, the indexing may be reversed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index">The index.  Will be bounded to the list domain.</param>
        /// <param name="reverse">Optional.  If true the indexing will be reversed.</param>
        /// <returns></returns>
        public static T GetBounded<T>(this IList<T> list, int index, bool reverse = false)
        {
            if (reverse) index = list.Count - 1 - index;
            if (index >= list.Count) index = list.Count - 1;
            if (index < 0) index = 0;
            return list[index];
        }

        /// <summary>
        /// Get the item at the specified index, automatically wrapping if it is outside the
        /// bounds of the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetWrapped<T>(this IList<T> list, int index)
        {
            if (list.Count > 0)
            {
                while (index >= list.Count) index -= list.Count;
                while (index < 0) index += list.Count;
            }
            return list[index];
        }

        /// <summary>
        /// Remove all duplicate objects from this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveDuplicates<T>(this IList<T> list)
        {
            for (int i = list.Count - 2; i >= 0; i--)
            {
                T itemA = list[i];
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (itemA.Equals(list[j]))
                    {
                        list.RemoveAt(j);
                        list.RemoveAt(i);
                        j--;
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Return the sub-list of all items in this list between the two specified index parameters.
        /// If the end value is lower than the start value, the item selection will wrap.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start">The start of the index range to extract</param>
        /// <param name="end">The end of the index range to extract</param>
        /// <returns></returns>
        public static IList<T> AllBetween<T>(this IList<T> list, double start, double end)
        {
            IList<T> result = new List<T>();
            if (end > start)
            {
                for (int i = (int)Math.Ceiling(start); i < Math.Min(list.Count, end); i++)
                {
                    result.Add(list[i]);
                }
            }
            else
            {
                for (int i = (int)Math.Ceiling(start); i < list.Count; i++)
                {
                    result.Add(list[i]);
                }
                for (int i = 0; i < end; i++)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// Get the index of the last item in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int LastIndex<T>(this IList<T> list)
        {
            return list.Count - 1;
        }

        /// <summary>
        /// Remove the last item in this list, for
        /// safety checking first whether there are any items within
        /// the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveLast<T>(this IList<T> list)
        {
            int i = list.LastIndex();
            if (i >= 0) list.RemoveAt(i);
        }

        /// <summary>
        /// Remove the first item in this list,
        /// for safety checking first whether there are any items to remove.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveFirst<T>(this IList<T> list)
        {
            if (list.Count > 0) list.RemoveAt(0);
        }

        /// <summary>
        /// Get the sublist containing the item at the specified index and
        /// every subsequent item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IList<T> SubListFrom<T>(this IList<T> list, int index)
        {
            var result = new List<T>(list.Count - index);
            for (int i = index; i < list.Count; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }

    }
}

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

using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension functions for the SortedList standard type
    /// </summary>
    public static class SortedListExtensions
    {
        /// <summary>
        /// Determine a value at a specified key, even if an object with that key is not explicitly a
        /// member of this list, interpolating between values as necessary.  Note that the datatype
        /// held within this list must be interpolatable (i.e. it must implement an override of the '+',
        /// '-' and '*' operators) or else you're gonna have a bad time.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key">The key value for which to retrieve or interpolate a value</param>
        /// <param name="tweening">The tweening algorithm to be used for the interpolation.</param>
        /// <returns></returns>
        public static TValue InterpolatedValueAt<TValue>(this SortedList<double, TValue> list, double key, 
            Interpolation tweening = Interpolation.LINEAR)
        {
            if (list.Count > 0)
            {
                if (list.ContainsKey(key)) return list[key];
                else if (list.Count == 1) return list.Values[0];

                double lastKey = list.Keys[0];
                for (int i = 1; i < list.Count - 1; i++)
                {
                    double thisKey = list.Keys[i];
                    if (key < thisKey || i == list.Count - 1)
                    {
                        double t = (key - lastKey) / (thisKey - lastKey);
                        return tweening.Interpolate(list.Values[i - 1], list.Values[i], t);
                    }
                }
            }
            return default(TValue);
        }

        /// <summary>
        /// Get the stored value with the key after the specified key value.
        /// Returns the first item in this list with a key that compared greater than
        /// that specified.  If none can be found, returns null.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key">The key value to search for</param>
        /// <param name="nextKey">OUTPUT.  The key of the next value.</param>
        /// <param name="wrap">If true, the list is treated as wrapping - i.e. if no item with a key greater
        /// than that specified can be found the first item will be returned instead.</param>
        /// <returns></returns>
        public static TValue NextAfter<TKey, TValue>(this SortedList<TKey,TValue> list, 
            TKey key, out TKey nextKey, bool wrap = false)
            where TKey : IComparable
            where TValue : class
        {
            foreach (KeyValuePair<TKey,TValue> kvp in list)
            {
                if (kvp.Key.CompareTo(key) > 0)
                {
                    nextKey = kvp.Key;
                    return kvp.Value;
                }
            }
            if (wrap && list.Count > 0)
            {
                var kvp = list.First();
                nextKey = kvp.Key;
                return kvp.Value;
            }
            nextKey = default(TKey);
            return null;
        }

        /// <summary>
        /// Get the stored value with the key after the specified key value.
        /// Returns the first item in this list with a key that compared greater than
        /// that specified.  If none can be found, returns null.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key">The key value to search for</param>
        /// <param name="wrap">If true, the list is treated as wrapping - i.e. if no item with a key greater
        /// than that specified can be found the first item will be returned instead.</param>
        /// <returns></returns>
        public static TValue NextAfter<TKey, TValue>(this SortedList<TKey, TValue> list,
            TKey key, bool wrap = false)
            where TKey : IComparable
            where TValue : class
        {
            TKey outKey;
            return list.NextAfter(key, out outKey, wrap);
        }

        /// <summary>
        /// Get the stored value with the key before the specified key value.
        /// Returns the last item in this list with a key that compared lower than
        /// that specified.  If none can be found, returns null.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key">The key value to search for</param>
        /// <param name="lastKey">OUTPUT. The last key before the specified key.</param>
        /// <param name="wrap">If true, the list is treated as wrapping - i.e. if no item with a key lower
        /// than that specified can be found the last item will be returned instead.</param>
        /// <returns></returns>
        public static TValue LastBefore<TKey, TValue>(this SortedList<TKey, TValue> list,
            TKey key, out TKey lastKey, bool wrap = false)
            where TKey : IComparable
            where TValue : class
        {
            for(int i = list.Count -1; i >= 0; i--)
            {
                TKey thisKey = list.Keys[i];
                if (thisKey.CompareTo(key) < 0)
                {
                    lastKey = thisKey;
                    return list.Values[i];
                }
            }
            if (wrap && list.Count > 0)
            {
                var kvp = list.Last();
                lastKey = kvp.Key;
                return kvp.Value;
            }
            lastKey = default(TKey);
            return null;
        }

        /// <summary>
        /// Get the stored value with the key before the specified key value.
        /// Returns the last item in this list with a key that compared lower than
        /// that specified.  If none can be found, returns null.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key">The key value to search for</param>
        /// <param name="wrap">If true, the list is treated as wrapping - i.e. if no item with a key lower
        /// than that specified can be found the last item will be returned instead.</param>
        /// <returns></returns>
        public static TValue LastBefore<TKey, TValue>(this SortedList<TKey, TValue> list,
            TKey key, bool wrap = false)
            where TKey : IComparable
            where TValue : class
        {
            TKey outKey;
            return list.LastBefore(key, out outKey, wrap);
        }

        /// <summary>
        /// Adds an element with the specified key and value into the SortedList,
        /// automatically dealing with the case where the specified key already exists
        /// within the list by incrementing the key to the next valid value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddSafe<TValue>(this SortedList<double, TValue> list, 
            double key, TValue value)
        {
            while (list.ContainsKey(key))
                key = key.NextValidValue();
            list.Add(key, value);
        }
    }
}

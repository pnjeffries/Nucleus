using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// Extension functions for the SortedList standard type
    /// </summary>
    public static class SortedListExtensions
    {
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
        public static TValue NextAfter<TKey, TValue>(this SortedList<TKey,TValue> list, TKey key, bool wrap = false)
            where TKey : IComparable
            where TValue : class
        {
            foreach (KeyValuePair<TKey,TValue> kvp in list)
            {
                if (kvp.Key.CompareTo(key) > 0) return kvp.Value;
            }
            if (wrap && list.Count > 0) return list.First().Value;
            return null;
        }
    }
}

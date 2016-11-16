using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
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
    }
}

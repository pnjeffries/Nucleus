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

using Nucleus.Base;
using Nucleus.Maths;
using System;
using System.Collections;
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
        /// Get the item at the specified index, or the default value
        /// of the relevant type if the index is outside the bounds of the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetOrDefault<T>(this IList<T> list, int index)
        {
            if (index >= list.Count) return default(T);
            if (index < 0) return default(T);
            return list[index];
        }

        /// <summary>
        /// Get the item at the specified index, automatically wrapping if it is outside the
        /// bounds of the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="wrap">Wrapping toggle - if false the wrapping will not occur and the list
        /// will be accessed normally.</param>
        /// <returns></returns>
        public static T GetWrapped<T>(this IList<T> list, int index, bool wrap = true)
        {
            if (wrap && list.Count > 0)
            {
                while (index >= list.Count) index -= list.Count;
                while (index < 0) index += list.Count;
            }
            return list[index];
        }

        /// <summary>
        /// Get the item the specified number of places from the end of this
        /// list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T FromEnd<T>(this IList<T> list, int index)
        {
            return list[list.Count - 1 - index];
        }


        /// <summary>
        /// Remove duplicate objects from this collection, leaving
        /// the first discovered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveDuplicates<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                T itemA = list[i];
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (itemA.Equals(list[j]))
                    {
                        list.RemoveAt(j);
                    }
                }
            }
        }

        /// <summary>
        /// Remove *all* duplicate objects from this collection, including
        /// the first instance discovered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveAllDuplicates<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                T itemA = list[i];
                bool remove = false;
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (itemA.Equals(list[j]))
                    {
                        list.RemoveAt(j);
                        remove = true;
                    }
                }
                if (remove)
                {
                    list.RemoveAt(i);
                    i--;
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

        /// <summary>
        /// Find the maximum value of a property or method on the items in this list
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value of the property
        /// for each item in this list.</param>
        /// <returns></returns>
        public static TProperty MaxDelegateValue<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TProperty);
            TProperty result = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TProperty value = propertyDelegate.Invoke(list[i]);
                if (value.CompareTo(result) == 1) result = value; 
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the minimum value of a property or method
        /// defined by a delegate function.
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static TItem ItemWithMax<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TItem);
            TItem result = list[0];
            TProperty max = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(max) == 1)
                {
                    max = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the index of the item in this list which returns the minimum value of a property or
        /// method defined by a delegate function.
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static int IndexOfMax<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return -1;
            int result = 0;
            TProperty max = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(max) == 1)
                {
                    max = value;
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the minimum value of a property or method
        /// defined by a delegate and where a specified condition is true.
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <param name="ifDelegate">Delegate function which returns true if the item
        /// should be considered</param>
        /// <returns></returns>
        public static TItem ItemWithMaxWhere<TItem, TProperty>(this IList<TItem> list, 
            Func<TItem, TProperty> propertyDelegate,
            Func<TItem, bool> ifDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TItem);
            TItem result = list[0];
            TProperty max = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                if (ifDelegate.Invoke(item))
                {
                    TProperty value = propertyDelegate.Invoke(item);
                    if (value.CompareTo(max) == 1)
                    {
                        max = value;
                        result = item;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Find the minimum value of a property or method on the items in this list
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static TProperty MinDelegateValue<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TProperty);
            TProperty result = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TProperty value = propertyDelegate.Invoke(list[i]);
                if (value.CompareTo(result) == -1) result = value;
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the minimum value of a property or method
        /// defined by a delegate
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static TItem ItemWithMin<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TItem);
            TItem result = list[0];
            TProperty min = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(min) == -1)
                {
                    min = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the index of the minimum value in this list
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int IndexOfMin<TItem>(this IList<TItem> list)
            where TItem : IComparable<TItem>
        {
            if (list.Count == 0) return -1;
            TItem min = list[0];
            int result = 0;
            for (int i = 0; i < list.Count; i++)
            {
                TItem value = list[i];
                if (value.CompareTo(min) == -1)
                {
                    min = value;
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the index of the item in this list which returns the minimum value of a property or method
        /// defined by a delegate function.
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static int IndexOfMin<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return -1;
            int result = 0;
            TProperty min = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(min) == -1)
                {
                    min = value;
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the index of the item in this list which returns the next highest value of a property or method
        /// defined by a delegate after the specified value
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <param name="after">The value for which the subsequent value in order is being sought</param>
        /// <returns></returns>
        public static int IndexOfNext<TItem, TProperty>(this IList<TItem> list,
            Func<TItem, TProperty> propertyDelegate, TProperty after)
            where TProperty : IComparable<TProperty>
            where TItem : class
        {
            int result = -1;
            TProperty min = default(TProperty);
            for (int i = 0; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(after) == 1 &&
                    (result == -1 || value.CompareTo(min) == -1))
                {
                    min = value;
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the next highest value of a property or method
        /// defined by a delegate after the specified value
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <param name="after">The value for which the subsequent value in order is being sought</param>
        /// <returns></returns>
        public static TItem ItemWithNext<TItem, TProperty>(this IList<TItem> list, 
            Func<TItem, TProperty> propertyDelegate, TProperty after)
            where TProperty : IComparable<TProperty>
            where TItem : class
        {
            TItem result = null;
            TProperty min = default(TProperty);
            for (int i = 0; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(after) == 1 && 
                    (result == null ||value.CompareTo(min) == -1))
                {
                    min = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the index of the item in this list which returns the highest value of a property or method
        /// defined by a delegate before the specified value
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <param name="before">The value for which the subsequent value in order is being sought</param>
        /// <returns></returns>
        public static int IndexOfPrevious<TItem, TProperty>(this IList<TItem> list,
            Func<TItem, TProperty> propertyDelegate, TProperty before)
            where TProperty : IComparable<TProperty>
            where TItem : class
        {
            int result = -1;
            TProperty max = default(TProperty);
            for (int i = 0; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(before) == -1 &&
                    (result == -1 || value.CompareTo(max) == 1))
                {
                    max = value;
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// Get a list of ranges of indices in this list where the specifed condition
        /// delegate will return true.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IList<IntInterval> IndexRangesWhere<TItem>(this IList<TItem> list,
            Func<TItem, bool> condition)
        {
            var result = new List<IntInterval>();
            bool intervalStarted = false;
            IntInterval current = new IntInterval();
            for (int i = 0; i < list.Count; i++)
            {
                if (condition.Invoke(list[i])) // Test condition
                {
                    if (intervalStarted) current = current.WithEnd(i); // Extend current
                    else
                    {
                        current = new IntInterval(i); // Start a new interval
                        intervalStarted = true;
                    }
                }
                else
                {
                    if (intervalStarted)
                    {
                        // Close current interval
                        result.Add(current);
                        intervalStarted = false;
                    }
                }
            }

            //Add last one:
            if (intervalStarted) result.Add(current);

            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the highest value of a property or method
        /// defined by a delegate before the specified value
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <param name="before">The value for which the subsequent value in order is being sought</param>
        /// <returns></returns>
        public static TItem ItemWithPrevious<TItem, TProperty>(this IList<TItem> list,
            Func<TItem, TProperty> propertyDelegate, TProperty before)
            where TProperty : IComparable<TProperty>
            where TItem : class
        {
            TItem result = null;
            TProperty max = default(TProperty);
            for (int i = 0; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(before) == -1 && 
                    (result == null || value.CompareTo(max) == 1))
                {
                    max = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Shift all items in this list by the specified number of places.
        /// An items moving off the start or end of the list will 'wrap' to the other
        /// end of the list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="offset">The number of places to shift the objects in this list.
        /// +ve numbers are to the left (i.e items will move towards the start of the list)
        /// and -ve numbers are to the right (i.e. items move towards the end).</param>
        /// <param name="wrap">If true, items shifted off the start or end of the list
        /// will be added to the other end of the list.  If false, they will be lost and the
        /// size of the list will decrease.</param>
        public static void Shift<TItem>(this IList<TItem> list, int offset)
        {
            // Store items that move off the start or end of the list
            TItem[] original = list.ToArray();

            // Shift the list items
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = original.GetWrapped(i + offset);
            }
        }

        /// <summary>
        /// Find the average of a set of double values obtainable via a delegate function from the items
        /// in this list.
        /// </summary>
        /// <typeparam name="TItem">The type of item in this list.</typeparam>
        /// <param name="list"></param>
        /// <param name="valueDelegate">Delegate function which returns the value to be averaged for each list item.</param>
        /// <returns></returns>
        public static double AverageDelegateValue<TItem>(this IList<TItem> list, Func<TItem, double> valueDelegate)
        {
            double result = 0;
            for (int i = 0; i < list.Count; i++) result += valueDelegate.Invoke(list[i]);
            if (list.Count > 0) result /= list.Count;
            return result;
        }

        /// <summary>
        /// Find the total of a set of double values obtainable via a delegate function from the
        /// items in this list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="valueDelegate">Delegate function which returns the value to the summed.</param>
        /// <returns></returns>
        public static double TotalDelegateValue<TItem>(this IList<TItem> list, Func<TItem, double> valueDelegate)
        {
            double result = 0;
            for (int i = 0; i < list.Count; i++) result += valueDelegate.Invoke(list[i]);
            return result;
        }

        /// <summary>
        /// Find the total of a set of double values obtainable via a delegate function from the
        /// items in this list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="valueDelegate">Delegate function which returns the value to the summed.</param>
        /// <returns></returns>
        public static int TotalDelegateValue<TItem>(this IList<TItem> list, Func<TItem, int> valueDelegate)
        {
            int result = 0;
            for (int i = 0; i < list.Count; i++) result += valueDelegate.Invoke(list[i]);
            return result;
        }

        /// <summary>
        /// Get a value from this 2-dimensional jagged list, returning a fallback value in the case that the specified indices are
        /// out-of bounds
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="lists"></param>
        /// <param name="i">The index in the outer list</param>
        /// <param name="j">The index in the inner list</param>
        /// <param name="fallback">The value to be returned when the specified index doesn't exist in this dataset</param>
        /// <returns></returns>
        public static TItem GetSafe<TItem>(this IList<IList<TItem>> lists, int i, int j, TItem fallback = default(TItem))
        {
            if (i >= 0 && i < lists.Count)
            {
                if (j >= 0 && j < lists[i].Count) return lists[i][j];
            }
            return fallback;
        }

        /// <summary>
        /// Get a random item from this list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="rng">The random number generator to use to determine the item to be selected</param>
        /// <returns></returns>
        public static TItem GetRandom<TItem>(this IList<TItem> list, Random rng)
        {
            int i = rng.Next(0, list.Count);
            return list[i];
        }

        /// <summary>
        /// Set the value at the specified index if the list is long enough or if it is not
        /// extend the list up to that length, populating intervening indices with the
        /// default value of the appropriate type.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public static void SetAutoExtend<TItem>(this IList<TItem> list, int index, TItem item)
        {
            if (list.Count > index) list[index] = item;
            else
            { 
                while (list.Count < index)
                {
                    list.Add(default(TItem));
                }
                list.Add(item);
            }
        }

        /// <summary>
        /// Randomly change the order of the objects in this list.
        /// Uses the Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="rng"></param>
        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of this list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (items == null) return;

            if (list is List<T>) ((List<T>)list).AddRange(items);
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Construct a sorted list using this list as the items and another list of related items in the same
        /// order as the keys.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static SortedList<TKey,TValue> SortedBy<TKey,TValue>(this IList<TValue> items, IList<TKey> keys)
        {
            var result = new SortedList<TKey,TValue>(items.Count);
            int count = Math.Min(items.Count, keys.Count);
            for (int i = 0; i < items.Count; i++)
            {
                result.Add(keys[i], items[i]);
            }
            return result;
        }

        /// <summary>
        /// Construct a sorted list using this list as the items and another list of related items in the same
        /// order as the keys.  This override for doubles uses the AddSafe extension method to enable duplicate
        /// keys to be dealt with without throwing an exception.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static SortedList<double, TValue> SortedBy<TValue>(this IList<TValue> items, IList<double> keys)
        {
            var result = new SortedList<double, TValue>(items.Count);
            int count = Math.Min(items.Count, keys.Count);
            for (int i = 0; i < items.Count; i++)
            {
                result.AddSafe(keys[i], items[i]);
            }
            return result;
        }

        /// <summary>
        /// Construct a sorted list using the items in this list and obtaining the sorting key via a delegate
        /// function.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="keyDelegate"></param>
        /// <returns></returns>
        public static SortedList<TKey, TValue> SortedBy<TKey, TValue>(this IList<TValue> items, Func<TValue, TKey> keyDelegate)
        {
            var result = new SortedList<TKey, TValue>(items.Count);
            foreach (TValue item in items)
            {
                result.Add(keyDelegate.Invoke(item), item);
            }
            return result;
        }

        /// <summary>
        /// Construct a sorted list using the items in this list and obtaining the sorting key via a delegate
        /// function.  This override for doubles uses the AddSafe extension method to enable duplicate keys to
        /// be dealt with without throwing an exception.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="keyDelegate"></param>
        /// <returns></returns>
        public static SortedList<double, TValue> SortedBy<TValue>(this IList<TValue> items, Func<TValue, double> keyDelegate)
        {
            var result = new SortedList<double, TValue>(items.Count);
            foreach (TValue item in items)
            {
                result.AddSafe(keyDelegate.Invoke(item), item);
            }
            return result;
        }

        /// <summary>
        /// Remove all items from this collection for which the specified delegate function returns true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="removeIfTrue"></param>
        /// <returns></returns>
        public static int RemoveIf<T>(this IList<T> items, Func<T, bool> removeIfTrue)
        {
            int count = 0;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (removeIfTrue(items[i]))
                {
                    items.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Find and return the first object of the specified type in this list of items
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T1 FirstOfType<T1>(this IList items)
        {
            foreach (var obj in items)
            {
                if (obj is T1) return (T1)obj;
            }
            return default(T1);
        }

        /// <summary>
        /// Find and return the first object of the specified type in this list of items
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T1 FirstOfType<T1, T2>(this IList<T2> items)
            where T1:T2
        {
            foreach (var obj in items)
            {
                if (obj is T1) return (T1)obj;
            }
            return default(T1);
        }

        /// <summary>
        /// Extract all objects of the specified type from this collection
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IList<T1> AllOfType<T1, T2>(this IList<T2> items)
            where T1:T2
        {
            var result = new List<T1>();
            foreach (var obj in items)
            {
                if (obj is T1 t1) result.Add(t1);
            }
            return result;
        }

        /// <summary>
        /// Extract all objects of the specified type from this collection
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IList<T1> AllOfType<T1>(this IList items)
        {
            var result = new List<T1>();
            foreach (var obj in items)
            {
                if (obj is T1 t1) result.Add(t1);
            }
            return result;
        }

        /// <summary>
        /// Sort the IList in-place.
        /// Note: to work this list must implement the non-generic IList
        /// interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Sort<T>(this IList<T> list)
        {
            ArrayList.Adapter((IList)list).Sort();
        }

        /// <summary>
        /// Sort the IList in-place using the specified comparer.
        /// Note: to work this list must implement the non-generic IList
        /// interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparer"></param>
        public static void Sort<T>(this IList<T> list, IComparer comparer)
        {
            ArrayList.Adapter((IList)list).Sort(comparer);
        }

        /// <summary>
        /// Sort the IList in-place using the specified comparison delegate.
        /// Note: to work this list must implement the non-generic IList
        /// interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparison"></param>
        public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
        {
            ArrayList.Adapter((IList)list).Sort(new ComparisonComparer<T>(comparison));
        }

        /// <summary>
        /// Both remove and return the object at the specified index in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetAndRemove<T>(this IList<T> list, int index)
        {
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        /// <summary>
        /// Does this collection contain any of the specified values?
        /// </summary>
        /// <param name="col"></param>
        /// <param name="anyOf"></param>
        /// <returns></returns>
        public static bool ContainsAny(this IList col, IEnumerable anyOf)
        {
            foreach (var item in anyOf)
            {
                if (col.Contains(item)) return true;
            }
            return false;
        }

        /// <summary>
        /// Does this collection contain any of the specified values?
        /// </summary>
        /// <param name="col"></param>
        /// <param name="anyOf"></param>
        /// <returns></returns>
        public static bool ContainsAny<T>(this IList<T> col, IEnumerable<T> anyOf)
        {
            foreach (var item in anyOf)
            {
                if (col.Contains(item)) return true;
            }
            return false;
        }
    }
}

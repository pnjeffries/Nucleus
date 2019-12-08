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

namespace Nucleus.Base
{
    /// <summary>
    /// Interface for objects which can be 'deleted'.
    /// Deleted objects will remain within the object model but be marked for
    /// deletion and removed at some future point, allowing them to be easily
    /// restored via 'Undeletion'
    /// </summary>
    public interface IDeletable
    {
        #region Properties

        /// <summary>
        /// Get a boolean value indicating whether this object has been
        /// marked for deletion.  This flag indicates that the object should be
        /// ignored in any operation that acts only on the current state of the
        /// model and that it should be removed during the next cleanup sweep.
        /// </summary> 
        bool IsDeleted { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this object.
        /// The object itself will not be immediately removed from the model
        /// but will instead be flagged for future removal and ignored wherever
        /// appropriate.
        /// </summary>
        void Delete();

        /// <summary>
        /// Undelete this object.
        /// If the deletion flag on this object is set it will be unset and
        /// the object restored.
        /// </summary>
        void Undelete();

        #endregion
    }

    /// <summary>
    /// Extension methods to act on IDeletable objects and/or collections of same
    /// </summary>
    public static class IDeletableExtensions
    {
        /// <summary>
        /// Remove all deleted objects from this list
        /// </summary>
        /// <param name="list"></param>
        public static void RemoveDeleted<TItem>(this IList<TItem> list)
            where TItem : IDeletable
        {
            if (list != null)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].IsDeleted) list.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Extract the subset of objects from this collection that are not deleted,
        /// as a list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static TList Undeleted<TList,T>(this TList list)
            where TList : IList<T>, new()
            where T:IDeletable
        {
            TList result = new TList();
            foreach (T item in list)
            {
                if (!item.IsDeleted) result.Add(item);
            }
            return result;
        }

        
        /// <summary>
        /// Extract the subset of objects from this collection that are not deleted,
        /// as a list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> Undeleted<T>(this IList<T> list)
            where T:IDeletable
        {
            var result = new List<T>();
            foreach (T item in list)
            {
                if (!item.IsDeleted) result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Get the number of objects in this collection which are not deleted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int UndeletedCount<T>(this IEnumerable<T> list) where T:IDeletable
        {
            int count = 0;
            foreach (T item in list)
            {
                if (!item.IsDeleted) count++;
            }
            return count;
        }

        /// <summary>
        /// Delete all objects in this collection.
        /// The objects themselves will not be immediately removed from the model
        /// but will instead be flagged for future removal and ignored wherever
        /// appropriate.
        /// </summary>
        /// <param name="list"></param>
        public static void DeleteAll(this IEnumerable<IDeletable> list)
        {
            foreach(IDeletable item in list)
            {
                item.Delete();
            }
        }

        /// <summary>
        /// Undelete all objects in this collection.
        /// If the deletion flag on any object is set it will be unset and
        /// the object restored.
        /// </summary>
        /// <param name="list"></param>
        public static void UndeleteAll(this IEnumerable<IDeletable> list)
        {
            foreach (IDeletable item in list)
            {
                item.Undelete();
            }
        }

        /// <summary>
        /// Delete any objects in this collection which meet the specified delegate criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="deleteIfTrue">The delegate function which returns true if the given object is to be deleted</param>
        /// <param name="removeDeleted">Optional, default false.  If true, the objects which are deleted will also be removed
        /// from the collection.</param>
        public static void DeleteIf<T>(this IList<T> list, Func<T, bool> deleteIfTrue, bool removeDeleted = false)
            where T:IDeletable
        {
            if (list != null)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var obj = list[i];
                    if (deleteIfTrue.Invoke(obj))
                    {
                        obj.Delete();
                        if (removeDeleted) list.RemoveAt(i);
                    }
                }
            }
        }
    }

}

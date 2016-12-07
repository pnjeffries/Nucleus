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
using FreeBuild.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Extensible storage mechanism for adding tagged data to model objects
    /// </summary>
    /// <typeparam name="TData">The type of data that this store will contain</typeparam>
    [Serializable]
    public abstract class DataStore<TData> : Dictionary<Type, TData>
    {

        /// <summary>
        /// Get data of the specified generic type from this
        /// data store.  Note that the type must match exactly - sub-types are not included.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : class, TData
        {
            Type tType = typeof(T);
            if (ContainsKey(tType)) return this[tType] as T;
            //Finding sub-types disabled for the sake of speed:
            //else
            //{
            //    Type tSubType = Keys.ClosestDescendent(tType);
            //    if (tSubType != null) return this[tSubType] as T;
            //}
            return null;
        }

        /// <summary>
        /// Get data of the specified generic type from this
        /// data store.  If no data component of the specified type is found then optionally a
        /// new one will be created.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <param name="create">If true, a new data component of the specified type will
        /// be created and returned should one not already exist.</param>
        /// <returns></returns>
        public T GetData<T>(bool create) where T: class, TData, new()
        {
            Type tType = typeof(T);
            if (ContainsKey(tType)) return this[tType] as T;
            else
            {
                //Finding sub-types disabled for the sake of speed:
                //Type tSubType = Keys.ClosestDescendent(tType);
                //if (tSubType != null) return this[tSubType] as T;
                if (create)
                {
                    T newData = new T();
                    this[tType] = newData;
                    return newData;
                }
            }
            return null;
        }

        /// <summary>
        /// Get all data within this store that is of the specified generic type or which
        /// is assignable to that type.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public IList<T> GetAllData<T>() where T: TData
        {
            IList<T> result = new List<T>();
            Type tType = typeof(T);
            foreach (Type keyType in Keys)
            {
                if (tType.IsAssignableFrom(keyType)) result.Add((T)this[keyType]);
            }
            return result;
        }
        /// <summary>
        /// Get all data within this store that is of the specified generic type or which
        /// is assignable to that type and add it to the specififed collection.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public IList<T> GetAllData<T>(IList<T> addTo) where T:TData
        {
            Type tType = typeof(T);
            foreach (Type keyType in Keys)
            {
                if (tType.IsAssignableFrom(keyType)) addTo.Add((T)this[keyType]);
            }
            return addTo;
        }

        /// <summary>
        /// Add a new data object to this data store.  If this data store already contains a data
        /// object of the same type, it will be replaced.
        /// </summary>
        /// <param name="data"></param>
        public void Add(TData data)
        {
            if (data != null) this[data.GetType()] = data;
        }
    }

    /// <summary>
    /// Extensible storage mechanism for adding tagged data to model objects
    /// </summary>
    public class DataStore : DataStore<object>
    {

    }
}

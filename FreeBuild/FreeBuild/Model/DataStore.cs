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
        /// Get data of the specified generic type (or the closest available sub-type) from this
        /// data store.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : class, TData
        {
            Type tType = typeof(T);
            if (ContainsKey(tType)) return this[tType] as T;
            else
            {
                Type tSubType = Keys.ClosestDescendent(tType);
                if (tSubType != null) return this[tSubType] as T;
            }
            return null;
        }

        /// <summary>
        /// Get data of the specified generic type (or the closest available sub-type) from this
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
                Type tSubType = Keys.ClosestDescendent(tType);
                if (tSubType != null) return this[tSubType] as T;
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
        /// Add a new data object to this data store.  If this data store already contains a data
        /// object of the same type, it will be replaced.
        /// </summary>
        /// <param name="data"></param>
        public void Add(TData data)
        {
            if (data != null) this[data.GetType()] = data;
        }
    }
}

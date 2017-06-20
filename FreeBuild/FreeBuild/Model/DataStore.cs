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
using System.Runtime.Serialization;
using FreeBuild.Base;
using System.ComponentModel;

namespace FreeBuild.Model
{
    /// <summary>
    /// Extensible storage mechanism for adding tagged data to model objects
    /// </summary>
    /// <typeparam name="TData">The type of data that this store will contain</typeparam>
    [Serializable]
    public abstract class DataStore<TData, TOwner> : ObservableKeyedCollection<Type, TData>, IOwned<TOwner>
        where TData : class
        where TOwner : IDataOwner
    {

        #region Properties

        /// <summary>
        /// Private backing field for Owner property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private TOwner _Owner;

        /// <summary>
        /// The object that this data store belongs to
        /// </summary>
        public TOwner Owner { get { return _Owner; } }

        /// <summary>
        /// Get a data component within this store by it's type name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public TData this[string typeName]
        {
            get { return GetData(typeName); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataStore() : base()
        {

        }

        /// <summary>
        /// Initialise a new DataStore with the given owner
        /// </summary>
        /// <param name="owner"></param>
        public DataStore(TOwner owner) : base()
        {
            _Owner = owner;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the key for the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override Type GetKeyForItem(TData item)
        {
            return item.GetType();
        }

        protected void RegisterPropertyChanged(TData item)
        {
            if (Owner != null && item != null && item is INotifyPropertyChanged)
                ((INotifyPropertyChanged)item).PropertyChanged += Item_PropertyChanged;
        }

        protected void UnregisterPropertyChanged(TData item)
        {
            if (Owner != null && item != null && item is INotifyPropertyChanged)
                ((INotifyPropertyChanged)item).PropertyChanged -= Item_PropertyChanged;
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Owner != null) Owner.NotifyComponentPropertyChanged(sender, e.PropertyName);
        }

        private void NotifyOwnerItemChanged(Type type)
        {
            if (Owner != null) Owner.NotifyComponentPropertyChanged(null, type.Name);
        }

        protected override void SetItem(int index, TData item)
        {
            base.SetItem(index, item);
            RegisterPropertyChanged(item);
            NotifyOwnerItemChanged(item.GetType());
        }

        protected override void InsertItem(int index, TData item)
        {
            base.InsertItem(index, item);
            RegisterPropertyChanged(item);
            NotifyOwnerItemChanged(item.GetType());
        }

        protected override void RemoveItem(int index)
        {
            TData item = this[index];
            base.RemoveItem(index);
            UnregisterPropertyChanged(item);
            NotifyOwnerItemChanged(item.GetType());
        }

        protected override void ClearItems()
        {
            foreach (TData item in this)
            {
                UnregisterPropertyChanged(item);
                NotifyOwnerItemChanged(item.GetType());
            }
            base.ClearItems();      
        }

        /// <summary>
        /// Get a datacomponent from this store by it's type name.
        /// </summary>
        /// <param name="typeName">The name of the type of component to be searched for.
        /// This will first attempt to be resolved via the Type.GetType static function - if this fails the
        /// store will be searched to find a stored type with the specifed 
        /// <returns></returns>
        public TData GetData(string typeName)
        {
            Type tType = Type.GetType(typeName);
            if (tType == null || !Contains(tType))
            {
                //If the type name could not be resolved, search through the collection for anything with the specified name
                foreach (TData item in this)
                {
                    if (item.GetType().Name.EqualsIgnoreCase(typeName)) return item;
                }
            }
            else
            {
                return this[tType];
            }
            return null;
        }

        /// <summary>
        /// Get data of the specified generic type from this
        /// data store.  Note that the type must match exactly - sub-types are not included.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : class, TData
        {
            Type tType = typeof(T);
            if (Contains(tType)) return this[tType] as T;
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
            if (Contains(tType)) return this[tType] as T;
            else
            {
                //Finding sub-types disabled for the sake of speed:
                //Type tSubType = Keys.ClosestDescendent(tType);
                //if (tSubType != null) return this[tSubType] as T;
                if (create)
                {
                    T newData = new T();
                    SetData(newData);
                    return newData;
                }
            }
            return null;
        }

        /// <summary>
        /// Add the specified data component to this store.  This will replace
        /// any attached data component of the same type.
        /// </summary>
        /// <param name="component"></param>
        public void SetData(TData component)
        {
            Type type = component.GetType();
            if (Contains(type))
            {
                Remove(type);
            }
            Add(component);
        }

        /// <summary>
        /// Remove an attached data component of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool RemoveData<T>() where T : class, TData
        {
            return Remove(typeof(T));
        }

        /// <summary>
        /// Does this data store contain data of the specified type?
        /// </summary>
        /// <typeparam name="T">The type of data component to check for</typeparam>
        /// <returns></returns>
        public bool HasData<T>() where T : class, TData
        {
            Type tType = typeof(T);
            return Contains(tType);
        }

        /// <summary>
        /// Does this data store contain data of the specified type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasData(Type type)
        {
            return Contains(type);
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
            foreach (TData item in this)
            {
                if (item is T) result.Add((T)item);
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
            foreach (TData item in this)
            {
                if (item is T) addTo.Add((T)item);
            }
            return addTo;
        }

        #endregion

    }

    /// <summary>
    /// Base class for data stores which allow component types to be specified via an additional
    /// [XXX]DataType enum
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TTypeEnum"></typeparam>
    [Serializable]
    public abstract class DataStore<TData, TOwner, TTypeEnum> : DataStore<TData, TOwner>
        where TData : class
        where TOwner : IDataOwner
        where TTypeEnum : struct
    {
        #region 
        /// <summary>
        /// Get a data component of a standard built-in type
        /// </summary>
        /// <param name="typeEnum"></param>
        /// <returns></returns>
        public TData this[TTypeEnum typeEnum]
        {
            get { return this[GetRepresentedType(typeEnum)]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataStore() : base()
        {

        }

        /// <summary>
        /// Initialise a new DataStore with the given owner
        /// </summary>
        /// <param name="owner"></param>
        public DataStore(TOwner owner) : base(owner) { }

        #endregion

        #region Methods

        /// <summary>
        /// Get the type represented by the specified data type enum
        /// </summary>
        /// <param name="typeEnum"></param>
        /// <returns></returns>
        protected abstract Type GetRepresentedType(TTypeEnum typeEnum);

        /// <summary>
        /// Get a data component of a standard built-in type
        /// </summary>
        /// <param name="typeEnum"></param>
        /// <returns></returns>
        public TData GetData(TTypeEnum typeEnum)
        {
            return this[typeEnum];
        }

        /// <summary>
        /// Does this data store contain data of the specified type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasData(TTypeEnum typeEnum)
        {
            return HasData(GetRepresentedType(typeEnum));
        }

        #endregion
    }

    
}

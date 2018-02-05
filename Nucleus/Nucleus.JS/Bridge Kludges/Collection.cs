
namespace System.Collections.ObjectModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime;

    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class Collection<T> : IList<T>, IList, IReadOnlyList<T>
    {
        IList<T> items;
        [NonSerialized]
        private Object _syncRoot;

        public Collection()
        {
            items = new List<T>();
        }

        public Collection(IList<T> list)
        {
            items = list;
        }

        public int Count
        {
            get { return items.Count; }
        }

        protected IList<T> Items
        {
            get { return items; }
        }

        public T this[int index]
        {
            get { return items[index]; }
            set
            {

                if (index < 0 || index >= items.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                SetItem(index, value);
            }
        }

        public void Add(T item)
        {
            int index = items.Count;
            InsertItem(index, item);
        }

        public void Clear()
        {
            ClearItems();
        }

        public void CopyTo(T[] array, int index)
        {
            items.CopyTo(array, index);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {

            if (index < 0 || index > items.Count)
            {
                throw new IndexOutOfRangeException();
            }

            InsertItem(index, item);
        }

        public bool Remove(T item)
        {

            int index = items.IndexOf(item);
            if (index < 0) return false;
            RemoveItem(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new IndexOutOfRangeException();
            }

            RemoveItem(index);
        }

        protected virtual void ClearItems()
        {
            items.Clear();
        }

        protected virtual void InsertItem(int index, T item)
        {
            items.Insert(index, item);
        }

        protected virtual void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }

        protected virtual void SetItem(int index, T item)
        {
            items[index] = item;
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return items.IsReadOnly;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        
        void ICollection.CopyTo(Array array, int index)
        {

            T[] tArray = array as T[];
            if (tArray != null)
            {
                items.CopyTo(tArray, index);
            }
            else
            {
                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);

                //
                // We can't cast array of value type to object[], so we don't support 
                // widening of primitive types here.
                //
                object[] objects = array as object[];

                int count = items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        objects[index++] = items[i];
                    }

            }
        }

        object IList.this[int index]
        {
            get { return items[index]; }
            set
            {
                    this[index] = (T)value;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return items.IsReadOnly;
            }
        }

        int IList.Add(object value)
        {

            Add((T)value);

            return this.Count - 1;
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
            {
                return Contains((T)value);
            }
            return false;
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                return IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);

        }

        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
            {
                Remove((T)value);
            }
        }

        private static bool IsCompatibleObject(object value)
        {
            // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>. 
            return ((value is T) || (value == null && default(T) == null));
        }
    }
}

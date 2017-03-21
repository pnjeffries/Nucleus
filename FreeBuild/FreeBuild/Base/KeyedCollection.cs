using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace FreeBuild.Base
{
    /// <summary>
    /// A customised re-implementation of System.Collections.ObjectModel.KeyedCollection that
    /// marks the backing dictionary as nonserialisable in order to avoid storing redundant data
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    //[DebuggerTypeProxy(typeof(Mscorlib_KeyedCollectionDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public abstract class KeyedCollection<TKey, TItem> : Collection<TItem>
    {
        #region Fields

        /// <summary>
        /// The size at which a dictionary will be created
        /// </summary>
        protected const int DefaultThreshold = 2;

        int keyCount;
        int threshold;

        IEqualityComparer<TKey> comparer;

        #endregion

        #region Properties

        [NonSerialized]
        Dictionary<TKey, TItem> _Dictionary;

        /// <summary>
        /// The backing dictionary that allows for fast lookup by key
        /// </summary>
        protected IDictionary<TKey, TItem> Dictionary
        {
            get { return _Dictionary; }
        }

        public IEqualityComparer<TKey> Comparer
        {
            get
            {
                return comparer;
            }
        }

        public TItem this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (_Dictionary != null)
                {
                    return _Dictionary[key];
                }

                foreach (TItem item in Items)
                {
                    if (comparer.Equals(GetKeyForItem(item), key)) return item;
                }

                throw new KeyNotFoundException();
                //return default(TItem);
            }
        }

        #endregion

        #region Constructors

        protected KeyedCollection() : this(null, DefaultThreshold) { }

        protected KeyedCollection(IEqualityComparer<TKey> comparer) : this(comparer, DefaultThreshold) { }


        protected KeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TKey>.Default;
            }

            if (dictionaryCreationThreshold == -1)
            {
                dictionaryCreationThreshold = int.MaxValue;
            }

            if (dictionaryCreationThreshold < -1)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.comparer = comparer;
            this.threshold = dictionaryCreationThreshold;
        }

        #endregion

        #region Methods

        public bool Contains(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (_Dictionary != null)
            {
                return _Dictionary.ContainsKey(key);
            }

            if (key != null)
            {
                foreach (TItem item in Items)
                {
                    if (comparer.Equals(GetKeyForItem(item), key)) return true;
                }
            }
            return false;
        }

        private bool ContainsItem(TItem item)
        {
            TKey key;
            if ((_Dictionary == null) || ((key = GetKeyForItem(item)) == null))
            {
                return Items.Contains(item);
            }

            TItem itemInDict;
            bool exist = _Dictionary.TryGetValue(key, out itemInDict);
            if (exist)
            {
                return EqualityComparer<TItem>.Default.Equals(itemInDict, item);
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (_Dictionary != null)
            {
                if (_Dictionary.ContainsKey(key))
                {
                    return Remove(_Dictionary[key]);
                }

                return false;
            }

            if (key != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (comparer.Equals(GetKeyForItem(Items[i]), key))
                    {
                        RemoveItem(i);
                        return true;
                    }
                }
            }
            return false;
        }



        protected void ChangeItemKey(TItem item, TKey newKey)
        {
            if (!ContainsItem(item))
            {
                throw new ArgumentException("Item does not exist.");
            }

            TKey oldKey = GetKeyForItem(item);
            if (!comparer.Equals(oldKey, newKey))
            {
                if (newKey != null)
                {
                    AddKey(newKey, item);
                }

                if (oldKey != null)
                {
                    RemoveKey(oldKey);
                }
            }
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            if (_Dictionary != null)
            {
                _Dictionary.Clear();
            }

            keyCount = 0;
        }

        protected abstract TKey GetKeyForItem(TItem item);

        protected override void InsertItem(int index, TItem item)
        {
            TKey key = GetKeyForItem(item);
            if (key != null)
            {
                AddKey(key, item);
            }
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            TKey key = GetKeyForItem(Items[index]);
            if (key != null)
            {
                RemoveKey(key);
            }
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, TItem item)
        {
            TKey newKey = GetKeyForItem(item);
            TKey oldKey = GetKeyForItem(Items[index]);

            if (comparer.Equals(oldKey, newKey))
            {
                if (newKey != null && _Dictionary != null)
                {
                    _Dictionary[newKey] = item;
                }
            }
            else
            {
                if (newKey != null)
                {
                    AddKey(newKey, item);
                }

                if (oldKey != null)
                {
                    RemoveKey(oldKey);
                }
            }
            base.SetItem(index, item);
        }

        private void AddKey(TKey key, TItem item)
        {
            if (_Dictionary != null)
            {
                _Dictionary.Add(key, item);
            }
            else if (keyCount >= threshold)
            {
                CreateDictionary();
                _Dictionary.Add(key, item);
            }
            else
            {
                if (Contains(key))
                {
                    throw new ArgumentException("An item with the same key already exists in the collection");
                }

                keyCount++;
            }
        }

        private void CreateDictionary()
        {
            _Dictionary = new Dictionary<TKey, TItem>(comparer);
            foreach (TItem item in Items)
            {
                TKey key = GetKeyForItem(item);
                if (key != null)
                {
                    _Dictionary.Add(key, item);
                }
            }
        }

        private void RemoveKey(TKey key)
        {
            Contract.Assert(key != null, "key shouldn't be null!");
            if (_Dictionary != null)
            {
                _Dictionary.Remove(key);
            }
            else
            {
                keyCount--;
            }
        }

        #endregion
    }
}

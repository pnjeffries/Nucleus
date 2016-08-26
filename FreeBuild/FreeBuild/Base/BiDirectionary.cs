using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// A bi-directional dictionary that allows fast lookup of value by key or
    /// of key by value
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class BiDirectionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Fields

        /// <summary>
        /// The key to value dictionary
        /// </summary>
        protected IDictionary<TKey, TValue> _AtoB;

        /// <summary>
        /// The reversed, value to key dictionary
        /// </summary>
        protected IDictionary<TValue, TKey> _BtoA;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set a value by a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                return _AtoB[key];
            }

            set
            {
                _AtoB[key] = value;
                _BtoA[value] = key;
            }
        }

        /// <summary>
        /// Get or set a key by a value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public TKey this[TValue val]
        {
            get
            {
                return _BtoA[val];
            }
            set
            {
                _BtoA[val] = value;
                _AtoB[value] = val;
            }
        }

        /// <summary>
        /// Get the number of elements contained within the dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return _AtoB.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _AtoB.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _AtoB.Values;
            }
        }

        #endregion

        #region Methods

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _AtoB.Add(item.Key, item.Value);
            _BtoA.Add(item.Value, item.Key);
        }

        public void Add(TKey key, TValue value)
        {
            _AtoB.Add(key, value);
            _BtoA.Add(value, key);
        }

        public void Clear()
        {
            _AtoB.Clear();
            _BtoA.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _AtoB.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _AtoB.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _AtoB.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _AtoB.GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {

            _BtoA.Remove(new KeyValuePair<TValue, TKey>(item.Value, item.Key));
            return _AtoB.Remove(item);
        }

        public bool Remove(TKey key)
        {
            TValue val = _AtoB[key];
            _BtoA.Remove(val);
            return _AtoB.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _AtoB.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _AtoB.GetEnumerator();
        }

        #endregion
    }
}

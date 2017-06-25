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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A bi-directional dictionary that stores pairs of values and allows fast
    /// lookup of the second by the first or the first by the second.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first item in each pair</typeparam>
    /// <typeparam name="TSecond">The type of the second item in each pair</typeparam>
    [Serializable]
    public class BiDirectionary<TFirst, TSecond> : IDictionary<TFirst, TSecond>
    {
        #region Fields

        /// <summary>
        /// The dictionary mapping the first set of data as keys to the second
        /// </summary>
        protected IDictionary<TFirst, TSecond> _FirstToSecond;

        /// <summary>
        /// The reversed dictionary mapping the second set of data as keys to the first
        /// </summary>
        protected IDictionary<TSecond, TFirst> _SecondToFirst;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set a value by a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TSecond this[TFirst key]
        {
            get
            {
                return _FirstToSecond[key];
            }

            set
            {
                _FirstToSecond[key] = value;
                _SecondToFirst[value] = key;
            }
        }

        /// <summary>
        /// Get or set a key by a value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public TFirst this[TSecond val]
        {
            get
            {
                return _SecondToFirst[val];
            }
            set
            {
                _SecondToFirst[val] = value;
                _FirstToSecond[value] = val;
            }
        }

        /// <summary>
        /// Get the number of elements contained within the dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return _FirstToSecond.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<TFirst> Keys
        {
            get
            {
                return _FirstToSecond.Keys;
            }
        }

        public ICollection<TSecond> Values
        {
            get
            {
                return _FirstToSecond.Values;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new BiDirectionary.
        /// </summary>
        public BiDirectionary()
        {
            _FirstToSecond = new Dictionary<TFirst, TSecond>();
            _SecondToFirst = new Dictionary<TSecond, TFirst>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a linked pair of items to the BiDirectionary
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TFirst, TSecond> item)
        {
            _FirstToSecond.Add(item.Key, item.Value);
            _SecondToFirst.Add(item.Value, item.Key);
        }

        /// <summary>
        /// Adds the linked pair of items to the BiDirectionary
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Add(TFirst first, TSecond second)
        {
            _FirstToSecond.Add(first, second);
            _SecondToFirst.Add(second, first);
        }

        /// <summary>
        /// Set the linked pair of items within the BiDirectionary.
        /// This will override any existing stored relationship
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Set(TFirst first, TSecond second)
        {
            //TODO: Remove past relationships!
            _FirstToSecond[first] = second;
            _SecondToFirst[second] = first;
        }

        /// <summary>
        /// Removes all items from the BiDirectionary
        /// </summary>
        public void Clear()
        {
            _FirstToSecond.Clear();
            _SecondToFirst.Clear();
        }

        public bool Contains(KeyValuePair<TFirst, TSecond> item)
        {
            return _FirstToSecond.Contains(item);
        }

        public bool ContainsKey(TFirst key)
        {
            return _FirstToSecond.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        {
            _FirstToSecond.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            return _FirstToSecond.GetEnumerator();
        }

        public bool Remove(KeyValuePair<TFirst, TSecond> item)
        {

            _SecondToFirst.Remove(new KeyValuePair<TSecond, TFirst>(item.Value, item.Key));
            return _FirstToSecond.Remove(item);
        }

        public bool Remove(TFirst key)
        {
            TSecond val = _FirstToSecond[key];
            _SecondToFirst.Remove(val);
            return _FirstToSecond.Remove(key);
        }

        public bool TryGetValue(TFirst key, out TSecond value)
        {
            return _FirstToSecond.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _FirstToSecond.GetEnumerator();
        }

        /// <summary>
        /// Get the value within the first set keyed by the specified
        /// value from within the second.
        /// </summary>
        /// <param name="bySecond"></param>
        /// <returns></returns>
        public TFirst GetFirst(TSecond bySecond)
        {
            return _SecondToFirst[bySecond];
        }

        /// <summary>
        /// Get the value within the second set keyed by the specified
        /// value from within the first set
        /// </summary>
        /// <param name="byFirst"></param>
        /// <returns></returns>
        public TSecond GetSecond(TFirst byFirst)
        {
            return _FirstToSecond[byFirst];
        }

        /// <summary>
        /// Determines whether this BiDirectionary contains the value specified within
        /// the first set.
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        public bool ContainsFirst(TFirst first)
        {
            return _FirstToSecond.ContainsKey(first);
        }

        /// <summary>
        /// Determines whether this BiDirectionary contains the value specified within
        /// the second set.
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool ContainsSecond(TSecond second)
        {
            return _SecondToFirst.ContainsKey(second);
        }

        #endregion
    }
}

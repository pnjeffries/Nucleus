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

namespace FreeBuild.Base
{
    /// <summary>
    /// A collection of unique objects which may be owned
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TOwner"></typeparam>
    [Serializable]
    public abstract class OwnedCollection<TItem, TOwner> : UniquesCollection<TItem>
        where TItem : class, IUnique, IOwned<TOwner>
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Owner property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private TOwner _Owner;

        /// <summary>
        /// The owning geometry of this vertex collection
        /// </summary>
        public TOwner Owner
        {
            get { return _Owner; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a collection with no owner.
        /// </summary>
        protected OwnedCollection() : base() { }

        /// <summary>
        /// Owner constructor.
        /// </summary>
        /// <param name="owner">The object which owns this collection</param>
        public OwnedCollection(TOwner owner) : base()
        {
            _Owner = owner;
        }

        /// <summary>
        /// Collection combination constructor
        /// </summary>
        /// <param name="toBeCombined"></param>
        public OwnedCollection(IEnumerable<IEnumerable<TItem>> toBeCombined) : base(toBeCombined) { }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides SetItem to set item owner
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void SetItem(int index, TItem item)
        {
            TItem oldItem = this[index];
            ClearItemOwner(oldItem);
            SetItemOwner(item);
            base.SetItem(index, item);
        }

        /// <summary>
        /// Overrides InsertItem to set item owner
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, TItem item)
        {
            SetItemOwner(item);
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Overrides RemoveItem to clear item owner
        /// </summary>
        /// <param name="index"></param>
        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            ClearItemOwner(item);
            base.RemoveItem(index);
        }

        /// <summary>
        /// Overrides ClearItems to clear items owner
        /// </summary>
        protected override void ClearItems()
        {
            foreach (TItem item in this)
            {
                ClearItemOwner(item);
            }
            base.ClearItems();
        }

        /// <summary>
        /// Set the owner of the specified item
        /// </summary>
        /// <param name="item"></param>
        protected abstract void SetItemOwner(TItem item);

        /// <summary>
        /// Clear the owner of the specified item
        /// </summary>
        /// <param name="item"></param>
        protected abstract void ClearItemOwner(TItem item);

        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    public abstract class OwnedCollection<TItem, TOwner> : UniquesCollection<TItem>
        where TItem : IUnique, IOwned<TOwner>
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Owner property
        /// </summary>
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
        /// Owner constructor
        /// </summary>
        /// <param name="owner"></param>
        public OwnedCollection(TOwner owner) : base()
        {
            _Owner = owner;
        }

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

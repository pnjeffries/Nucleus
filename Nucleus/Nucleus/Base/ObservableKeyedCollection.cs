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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Nucleus.Base
{
    /// <summary>
    /// Implementation of an observable version of KeyedCollection.
    /// Based (roughly) on the code from:
    /// http://geekswithblogs.net/NewThingsILearned/archive/2010/01/12/make-keyedcollectionlttkey-titemgt-to-work-properly-with-wpf-data-binding.aspx
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The item type</typeparam>
    [Serializable]
    public abstract class ObservableKeyedCollection<TKey, TItem> : 
        KeyedCollection<TKey, TItem>, 
        INotifyCollectionChanged,
        INotifyPropertyChanged,
        IDuplicatable
    {
        #region Events

        /// <summary>
        /// CollectionChanged event implementation - raised when the collection is changed
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Event raised when a property of this object is changed
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Boolean flag to temporarily suppress CollectionChanged event raising
        /// </summary>
        private bool _SuppressNotifyCollectionChanged = false;

        /// <summary>
        /// Raise a PropertyChanged event for the specified property name
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raise an event
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="args"></param>
        private void RaiseEvent(NotifyCollectionChangedEventHandler handler, NotifyCollectionChangedEventArgs args)
        {
            handler?.Invoke(this, args);
        }

        /// <summary>
        /// Raise a CollectionChanged event, signalling that this collection has been modified.
        /// </summary>
        /// <param name="args"></param>
        protected void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (!_SuppressNotifyCollectionChanged)
            {
                OnCollectionChanged();
                RaiseEvent(CollectionChanged, args);
                if (args.Action != NotifyCollectionChangedAction.Replace) NotifyPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Raise a CollectionChanged event, signalling that this collection has been modified.
        /// </summary>
        protected void NotifyCollectionChanged(NotifyCollectionChangedAction action, TItem item, TItem oldItem, int index)
        {
            if (!_SuppressNotifyCollectionChanged)
            {
                OnCollectionChanged();
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, oldItem, index));
                if (action != NotifyCollectionChangedAction.Replace) NotifyPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Raise a CollectionChanged event, signalling that this collection has been modified.
        /// </summary>
        protected void NotifyCollectionChanged(NotifyCollectionChangedAction action, TItem item, int index)
        {
            if (!_SuppressNotifyCollectionChanged)
            {
                OnCollectionChanged();
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item, index));
                if (action != NotifyCollectionChangedAction.Replace) NotifyPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Raise a CollectionChanged event, signalling that this collection has been modified.
        /// </summary>
        protected void NotifyCollectionChanged(NotifyCollectionChangedAction action, TItem item)
        {
            if (!_SuppressNotifyCollectionChanged)
            {
                OnCollectionChanged();
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));
                if (action != NotifyCollectionChangedAction.Replace) NotifyPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Raise a CollectionChanged event, signalling that this collection has been modified.
        /// </summary>
        protected void NotifyCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (!_SuppressNotifyCollectionChanged)
            {
                OnCollectionChanged();
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));
                if (action != NotifyCollectionChangedAction.Replace) NotifyPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Raise a CollectionChanged event for multiple items at once.
        /// Includes a fix for WPF collectionviews throwing an exception when more than one item
        /// is added at once.
        /// See: http://geekswithblogs.net/NewThingsILearned/archive/2008/01/16/listcollectionviewcollectionview-doesnt-support-notifycollectionchanged-with-multiple-items.aspx
        /// </summary>
        /// <param name="e"></param>
        protected virtual void NotifyCollectionChangedMultiItem(NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged();
            NotifyCollectionChangedEventHandler handlers = CollectionChanged;
            if (handlers != null)
            {
                foreach (NotifyCollectionChangedEventHandler handler in handlers.GetInvocationList())
                {
                    if (IsCollectionView(handler.Target)) //is ICollectionView)
                        handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));//((ICollectionView)handler.Target).Refresh();
                    else
                        handler(this, e);
                }
            }
            if (e.Action != NotifyCollectionChangedAction.Replace) NotifyPropertyChanged("Count");
        }

#endregion

#region Constructors

        protected ObservableKeyedCollection() : base() { }

        protected ObservableKeyedCollection(IEnumerable<IEnumerable<TItem>> toBeCombined) : base()
        {
            foreach (IEnumerable<TItem> subCollection in toBeCombined)
            {
                if (subCollection != null) TryAddRange(subCollection);
            }
        }

#endregion

#region Methods

        /// <summary>
        /// Attempt to add a new item to the end of this collection, first checking whether the collection already
        /// contains an item with the specified key (and aborting if so)
        /// </summary>
        /// <param name="item">The object to be added to the end of the collection</param>
        /// <returns>True if the item was successfully added, false if an equivalent key already existed and it was not</returns>
        public bool TryAdd(TItem item)
        {
            if (item == null || Contains(GetKeyForItem(item))) return false;
            else
            {
                Add(item);
                return true;
            }
        }

        /// <summary>
        /// Add a collection of items to the end of this collection
        /// </summary>
        /// <param name="items">The items to be added to the end of the collection</param>
        public void AddRange<TItemSubType>(IEnumerable<TItemSubType> items)
            where TItemSubType : TItem
        {
            _SuppressNotifyCollectionChanged = true;
            var newItems = new List<TItem>(items.Count());
            foreach (var item in items)
            {
                Add(item);
                newItems.Add(item);
            }
            _SuppressNotifyCollectionChanged = false;
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems);
            NotifyCollectionChangedMultiItem(args);
        }

        /// <summary>
        /// Remove multiple items from this collection
        /// </summary>
        /// <param name="items"></param>
        public void RemoveRange(IEnumerable<TItem> items)
        {
            foreach (var item in items) Remove(item);
        }

        /// <summary>
        /// Attempt to add a collection of items to the end of this collection,
        /// checking whether each item has a matching key before adding it.
        /// </summary>
        /// <param name="items">The items to be added to the end of the collection</param>
        /// <returns>The sub-set of the input items that was successfully added</returns>
        public IList<TItem> TryAddRange(IEnumerable<TItem> items)
        {
            var added = new List<TItem>(items.Count());
            _SuppressNotifyCollectionChanged = true;
            foreach (var item in items)
            {
                if (TryAdd(item)) added.Add(item);
            }
            _SuppressNotifyCollectionChanged = false;
            NotifyCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, added));
            return added;
        }

        /// <summary>
        /// Overrides SetItem to raise a collection changed event when called
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void SetItem(int index, TItem item)
        {
            TItem oldItem = this[index];
            base.SetItem(index, item);
            NotifyCollectionChanged(NotifyCollectionChangedAction.Replace, item, oldItem, index);
        }

        /// <summary>
        /// Overrides InsertItem to raise a collection changed event when called
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            NotifyCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Overrides ClearItems to raise a collection changed event when called
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            NotifyCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        /// <summary>
        /// Overrides RemoveItem to raise a collection changed event when called
        /// </summary>
        /// <param name="index"></param>
        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            base.RemoveItem(index);
            NotifyCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }

        /// <summary>
        /// Protected function called when the collection is changed.
        /// Used by subclasses to synchronise cached data.
        /// </summary>
        protected virtual void OnCollectionChanged() { }

        /// <summary>
        /// Retrieve the combined value of a member of all items in this collection,
        /// expressed as a lambda function.  If the value of that member is consistent across all items
        /// in this collection then that value will be returned, else the specified value will be returned
        /// to indicate multiple values.
        /// </summary>
        /// <param name="propertyDelegate">A lambda function that returns a particular property for each item in the selection</param>
        /// <param name="multiValue">The value that should be returned to indicate multiple inconsistent values</param>
        /// <param name="nullValue">The value that should be returned if there are no items in this collection</param>
        /// <returns>The consistent value that is shared between all items, if that is the case, else the input multiValue</returns>
        public TValue CombinedValue<TValue>(Func<TItem, TValue> propertyDelegate, TValue multiValue = default(TValue), TValue nullValue = default(TValue))
        {
            IEnumerable<TValue> values = this.Select(propertyDelegate);
            TValue combinedValue = nullValue;
            bool first = true;
            foreach (TValue value in values)
            {
                if (first)
                {
                    first = false;
                    combinedValue = value;
                }
                else if (!Equals(value, combinedValue))
                    return multiValue;
            }
            //if (combinedValue == null) combinedValue = nullValue; //?
            return combinedValue;
        }


        /// <summary>
        /// Move the item with the specified key to the last position in the collection
        /// </summary>
        /// <param name="key"></param>
        public void MoveToLast(TKey key)
        {
            if (Contains(key))
            {
                TItem item = this[key];
                Remove(key);
                Add(item);
            }
        }

        /// <summary>
        /// Replace the object in this collection with the same key, at the position of the original.
        /// If no object with the same key exists, the specified value will be added to the end of the list.
        /// </summary>
        /// <param name="item"></param>
        public void Replace(TItem item)
        {
            TKey key = GetKeyForItem(item);
            if (Contains(key))
            {
                int index = IndexOf(this[key]);
                SetItem(index, item);
            }
            else
                Add(item);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Is the specified object potentially a collectionview?
        /// Used to enable a workaround for CollectionView crashing
        /// on multiple update.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsCollectionView(object obj)
        {
            if (obj is INotifyCollectionChanged && obj is INotifyPropertyChanged) return true; //TODO: Get more specific
            return false;
        }

        #endregion
    }
}

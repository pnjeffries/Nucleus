using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FreeBuild.Base
{
    /// <summary>
    /// Implementation of an observable version of KeyedCollection.
    /// Based (roughly) on the code from:
    /// http://geekswithblogs.net/NewThingsILearned/archive/2010/01/12/make-keyedcollectionlttkey-titemgt-to-work-properly-with-wpf-data-binding.aspx
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The item type</typeparam>
    public abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged
    {
        #region Events

        /// <summary>
        /// CollectionChanged event implementation - raised when the collection is changed
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Boolean flag to temporarily suppress CollectionChanged event raising
        /// </summary>
        private bool _SuppressNotifyCollectionChanged = false;

        /// <summary>
        /// Raise an event
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="args"></param>
        private void RaiseEvent(NotifyCollectionChangedEventHandler handler, NotifyCollectionChangedEventArgs args)
        {
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Raise a CollectionChanged event, signalling that this collection has been modified.
        /// </summary>
        /// <param name="args"></param>
        protected void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (!_SuppressNotifyCollectionChanged) RaiseEvent(CollectionChanged, args);
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
            NotifyCollectionChangedEventHandler handlers = CollectionChanged;
            if (handlers != null)
            {
                foreach (NotifyCollectionChangedEventHandler handler in handlers.GetInvocationList())
                {
                    if (handler.Target is CollectionView)
                        ((CollectionView)handler.Target).Refresh();
                    else
                        handler(this, e);
                }
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
            if (Contains(GetKeyForItem(item))) return false;
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
        public void AddRange(IEnumerable<TItem> items)
        {
            _SuppressNotifyCollectionChanged = true;
            foreach (var item in items) Add(item);
            _SuppressNotifyCollectionChanged = false;
            NotifyCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<TItem>(items)));
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
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
        }

        /// <summary>
        /// Overrides InsertItem to raise a collection changed event when called
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Overrides ClearItems to raise a collection changed event when called
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Overrides RemoveItem to raise a collection changed event when called
        /// </summary>
        /// <param name="index"></param>
        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            base.RemoveItem(index);
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        /// <summary>
        /// Retrieve the combined value of a member of all items in this collection,
        /// expressed as a lambda function.  If the value of that member is consistent across all items
        /// in this collection then that value will be returned, else the specified value will be returned
        /// to indicate multiple values.
        /// </summary>
        /// <param name="propertyDelegate">A lambda function that returns a particular property for each item in the selection</param>
        /// <param name="multiValue">The value that should be returned to indicate multiple inconsistent values</param>
        /// <returns>The consistent value that is shared between all items, if that is the case, else the input multiValue</returns>
        public object CombinedValue(Func<TItem, object> propertyDelegate, object multiValue)
        {
            IEnumerable<object> values = this.Select(propertyDelegate);
            object combinedValue = null;
            bool first = true;
            foreach (object value in values)
            {
                if (first)
                {
                    first = false;
                    combinedValue = value;
                }
                else if (!Equals(value, combinedValue))
                    return multiValue;
            }
            return combinedValue;
        }

        #endregion
    }
}

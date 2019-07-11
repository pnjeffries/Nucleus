using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UI
{
    /// <summary>
    /// A specialised form of DataBinding which permits binding
    /// to an observable collection
    /// </summary>
    [Serializable]
    public class ItemsSourceBinding : DataBinding
    {
        #region Properties

        /// <summary>
        /// Cached reference to the latest bound collection
        /// </summary>
        private INotifyCollectionChanged _Collection = null;

        private List<object> _AddedItems = null;

        /// <summary>
        /// Items which have been recently added.
        /// Null if all added items have been processed.
        /// </summary>
        public List<object> AddedItems
        {
            get { return _AddedItems; }
            set { _AddedItems = value; }
        }

        private IList<object> _RemovedItems = null;

        /// <summary>
        /// Items which have been recently removed.
        /// Null if all removed items have been processed.
        /// </summary>
        public IList<object> RemovedItems
        {
            get { return _RemovedItems; }
            set { _RemovedItems = value; }
        }

        /// <summary>
        /// Get a boolean value indicating whether it is necessary to refresh
        /// items generated as a result of this binding
        /// </summary>
        public bool ItemsRefreshRequired
        {
            get
            {
                return (_AddedItems != null && _AddedItems.Count > 0) ||
                    (_RemovedItems != null && _RemovedItems.Count > 0);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Rebuild the binding chain to establish property change monitoring,
        /// additionally setting up collection change event listening on the
        /// source object (if it is a collection)
        /// </summary>
        public override void RefreshBinding()
        {
            base.RefreshBinding();

            object newValue = GetBoundValue();
            if (newValue != _Collection)
            {
                // Remove collectionchanged event watcher:
                if (_Collection != null)
                {
                    _Collection.CollectionChanged -= CollectionChanged;
                    if (_Collection is IEnumerable)
                    {
                        // Remove items from the old collection:
                        var enumerable = (IEnumerable)_Collection;
                        if (RemovedItems == null) RemovedItems = new List<object>();
                        foreach (var item in enumerable) RemovedItems.Add(item);
                    }
                }

                // Add collectionchanged event watcher:
                if (newValue != null && newValue is INotifyCollectionChanged)
                {
                    var newCollection = (INotifyCollectionChanged)newValue;
                    newCollection.CollectionChanged += CollectionChanged;
                    _Collection = newCollection;
                    if (newCollection is IEnumerable)
                    {
                        // Add items from the new collection:
                        var enumerable = (IEnumerable)newCollection;
                        if (AddedItems == null) AddedItems = new List<object>();
                        foreach (var item in enumerable) AddedItems.Add(item);
                    }
                }
                else _Collection = null;
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                if (AddedItems == null) AddedItems = new List<object>(e.NewItems.Count);
                foreach (var item in e.NewItems) AddedItems.Add(item);
            }
            if (e.OldItems != null)
            {
                if (RemovedItems == null) RemovedItems = new List<object>(e.OldItems.Count);
                foreach (var item in e.OldItems) RemovedItems.Add(item);
            }   
        }

        #endregion
    }
}

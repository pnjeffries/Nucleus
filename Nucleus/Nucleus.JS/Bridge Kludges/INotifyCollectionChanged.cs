using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace System.Collections.Specialized
{
    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    public enum NotifyCollectionChangedAction
    {
        Add,
        Remove,
        Replace,
        Move,
        Reset,
    }

    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        private NotifyCollectionChangedAction _action;
        private IList _newItems, _oldItems;
        private int _newStartingIndex = -1;
        private int _oldStartingIndex = -1;

        /// <summary>
        /// The action that caused the event.
        /// </summary>
        public NotifyCollectionChangedAction Action
        {
            get { return _action; }
        }

        /// <summary>
        /// The items affected by the change.
        /// </summary>
        public IList NewItems
        {
            get { return _newItems; }
        }

        /// <summary>
        /// The old items affected by the change (for Replace events).
        /// </summary>
        public IList OldItems
        {
            get { return _oldItems; }
        }

        /// <summary>
        /// The index where the change occurred.
        /// </summary>
        public int NewStartingIndex
        {
            get { return _newStartingIndex; }
        }

        /// <summary>
        /// The old index where the change occurred (for Move events).
        /// </summary>
        public int OldStartingIndex
        {
            get { return _oldStartingIndex; }
        }

        private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
        {
            _action = action;
            {
                _newItems = newItems;
            }
            _newStartingIndex = newStartingIndex;
        }

        private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            if (action == NotifyCollectionChangedAction.Add)
                InitializeAdd(action, changedItems, startingIndex);
            else if (action == NotifyCollectionChangedAction.Remove)
                InitializeRemove(action, changedItems, startingIndex);
        }

        private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
        {
            _action = action;
            _oldItems = NewItems;
            _oldStartingIndex = oldStartingIndex;
        }

        private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
        {
            InitializeAdd(action, newItems, startingIndex);
            InitializeRemove(action, oldItems, oldStartingIndex);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        {
            InitializeAdd(action, null, -1);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
        {
            InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
        { 

            if (action == NotifyCollectionChangedAction.Reset)
            {
                InitializeAdd(action, null, -1);
            }
            else
            {
                InitializeAddOrRemove(action, new object[] { changedItem }, -1);
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
        {

            if (action == NotifyCollectionChangedAction.Reset)
            {
                InitializeAdd(action, null, -1);
            }
            else
            {
                InitializeAddOrRemove(action, new object[] { changedItem }, index);
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
        {
            
            if (action == NotifyCollectionChangedAction.Reset)
            {
                InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                    throw new ArgumentNullException("changedItems");

                InitializeAddOrRemove(action, changedItems, -1);
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            int oldStartingIndex = index;
            InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, index, oldStartingIndex);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            if (action == NotifyCollectionChangedAction.Reset)
            {
                InitializeAdd(action, null, -1);
            }
            else
            {
                InitializeAddOrRemove(action, changedItems, startingIndex);
            }
        }
    }

}

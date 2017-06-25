using Nucleus.Base;
using Nucleus.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Undo
{
    /// <summary>
    /// Class for general undo/redo management
    /// </summary>
    public class UndoManager : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Private backing field for the MaxStackSize property
        /// </summary>
        private int _MaxStackSize = 25;

        /// <summary>
        /// The maximum allowable size of the undo/redo stacks
        /// </summary>
        public int MaxStackSize
        {
            get { return _MaxStackSize; }
            set
            {
                _MaxStackSize = value;
                NotifyPropertyChanged("MaxStackSize");
                //TODO: Limit stacks
                LimitUndoStackSize();
                LimitRedoStackSize();
            }
        }

        /// <summary>
        /// Private backing field for Active property
        /// </summary>
        private bool _Active = true;

        /// <summary>
        /// Gets or sets whether the undo manager is active.
        /// If false, undo states will not be stored.
        /// </summary>
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        /// <summary>
        /// Private backing field for the UndoStack property
        /// </summary>
        private UndoStageCollection _UndoStack = new UndoStageCollection();

        /// <summary>
        /// The stack of undo operations
        /// </summary>
        public UndoStageCollection UndoStack
        {
            get { return _UndoStack; }
        }

        /// <summary>
        /// Private backing field for the RedoStack property
        /// </summary>
        private UndoStageCollection _RedoStack = new UndoStageCollection();

        /// <summary>
        /// The stack of Redo operations
        /// </summary>
        public UndoStageCollection RedoStack
        {
            get { return _RedoStack; }
        }

        /// <summary>
        /// Private backing field for the ActiveStage property
        /// </summary>
        private UndoStage _ActiveStage = null;

        /// <summary>
        /// The currently active undo stage.
        /// New undo states will be added to this stage.
        /// </summary>
        public UndoStage ActiveStage
        {
            get { return _ActiveStage; }
            set { _ActiveStage = value; }
        }

        /// <summary>
        /// Private backing field for Locked property
        /// </summary>
        private bool _Locked = false;

        /// <summary>
        /// Lock flag.  When locked, new states may not be added to the stack.
        /// This is automatically engaged when undoing a stage so as to avoid the undo
        /// operation itself being recorded.
        /// </summary>
        public bool Locked
        {
            get { return _Locked; }
            private set { _Locked = value; }
        }

        #region Methods

        /// <summary>
        /// Add an undo state to the current stage (if one is active) or directly 
        /// to the stack as a new stage (if one isn't). 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="clearRedo">If true, the redo stack will be invalidated</param>
        public void AddUndoState(UndoState state, bool clearRedo = true)
        {
            if (!Locked && Active)
            {
                if (state != null && state.IsValid)
                {
                    if (ActiveStage != null)// && !ActiveStage.Contains(state))
                    {
                        ActiveStage.Add(state);
                        if (!UndoStack.Contains(ActiveStage)) AddUndoStage(ActiveStage);
                    }
                    else
                    {
                        UndoStage stage = new UndoStage();
                        stage.Add(state);
                        AddUndoStage(stage);
                    }
                }
                if (clearRedo) RedoStack.Clear();
            }
        }

        /// <summary>
        /// Add a new stage to the undo stack
        /// </summary>
        /// <param name="stage"></param>
        private void AddUndoStage(UndoStage stage, bool clearRedo = true)
        {
            if (stage != null && stage.Count > 0)
            {
                UndoStack.Add(stage);
                if (UndoStack.Count > MaxStackSize)
                {
                    UndoStack.RemoveAt(0);
                }
                if (clearRedo) RedoStack.Clear();
            }
        }

        /// <summary>
        /// Reduce the size of the Undo stack to the specified maximum
        /// </summary>
        private void LimitUndoStackSize()
        {
            while (UndoStack.Count > _MaxStackSize)
            {
                UndoStack.RemoveAt(0);
            }
        }

        /// <summary>
        /// Add a new stage to the redo stack
        /// </summary>
        /// <param name="stage"></param>
        private void AddRedoStage(UndoStage stage)
        {
            if (stage != null && stage.Count > 0)
            {
                RedoStack.Add(stage);
                if (RedoStack.Count > MaxStackSize)
                {
                    RedoStack.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Reduce the size of the Undo stack to the specified maximum
        /// </summary>
        private void LimitRedoStackSize()
        {
            while (RedoStack.Count > _MaxStackSize)
            {
                RedoStack.RemoveAt(0);
            }
        }

        /// <summary>
        /// Begin a new undo stage.
        /// Until EndStage() is called, all undo states added will be combined together into a single MultiUndoState.
        /// </summary>
        /// <returns>The new MultiUndoState that represents the current stage</returns>
        public UndoStage BeginStage()
        {
            EndStage();
            ActiveStage = new UndoStage();
            return ActiveStage;
        }

        /// <summary>
        /// End the currently active undo stage, committing it to the stack
        /// </summary>
        public void EndStage()
        {
            //AddUndoStage(ActiveStage);
            ActiveStage = null;
        }

        /// <summary>
        /// Undo the last undoable action
        /// </summary>
        public virtual void Undo()
        {
            Locked = true;
            if (UndoStack.Count > 0)
            {
                UndoStage stage = UndoStack[UndoStack.Count - 1];
                UndoStack.RemoveAt(UndoStack.Count - 1);
                UndoStage redo = stage.Undo();
                AddRedoStage(redo);
                BeginStage();
            }
            Locked = false;
        }

        /// <summary>
        /// Redo the last redoable action
        /// </summary>
        public virtual void Redo()
        {
            Locked = true;
            if (RedoStack.Count > 0)
            {
                UndoStage stage = RedoStack[RedoStack.Count - 1];
                RedoStack.RemoveAt(RedoStack.Count - 1);
                UndoStage redo = stage.Undo();
                AddUndoStage(redo, false);
                BeginStage();
            }
            Locked = false;
        }

        /// <summary>
        /// Empty both the undo and redo stack
        /// </summary>
        public virtual void ClearUndoStack()
        {
            BeginStage();
            UndoStack.Clear();
            RedoStack.Clear();
        }

        /// <summary>
        /// Register the specified collection to be watched by this manager for undoable operations.
        /// Raised CollectionChanged events will result in a new undostate being automatically created
        /// </summary>
        /// <param name="collection"></param>
        public void WatchCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += Collection_CollectionChanged;
        }

        /// <summary>
        /// Register the specified collection and all objects in it to be watched by this manager
        /// for undoable operations.
        /// </summary>
        /// <param name="collection"></param>
        public void WatchAll(IList collection)
        {
            if (collection is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)collection).CollectionChanged += All_CollectionChanged;
            }
            foreach (object item in collection)
            {
                WatchAll(item);
            }
        }

        /// <summary>
        /// Register the specified object and, if it is a collection, any items contained within it
        /// to be watched for undoable operations.  Raised CollectionChanged events and 
        /// PropertyChanged events that use the PropertyChangedEventArgsExtended type
        /// for arguments will result in a new UndoState being automatically created.
        /// Any new items added to a watched collection will themselves be automatically watched.
        /// </summary>
        /// <param name="obj"></param>
        public void WatchAll(object obj)
        {
            if (obj is IList) WatchAll((IList)obj);
            else if (obj is INotifyPropertyChanged) WatchProperties((INotifyPropertyChanged)obj);
        }

        /// <summary>
        /// Register the specified object to be watched by this manager for undoable operations.
        /// Raised PropertyChanged events that use the PropertyChangedEventArgsExtended type
        /// for arguments will result in a new UndoState being automatically created
        /// </summary>
        /// <param name="obj"></param>
        public virtual void WatchProperties(INotifyPropertyChanged obj)
        {
            obj.PropertyChanged += Obj_PropertyChanged;
        }

        private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedEventArgsExtended)
            {
                AddUndoState(new PropertyUndoState(sender, (PropertyChangedEventArgsExtended)e));
            }
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is IList)
            {
                AddUndoState(new CollectionUndoState((IList)sender, e));
            }
        }

        private void All_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is IList)
            {
                IList list = (IList)sender;
                AddUndoState(new CollectionUndoState(list, e));
                if (e.NewItems != null)
                {
                    foreach (object item in e.NewItems)
                    {
                        WatchAll(item);
                    }
                }
            }
        }

        #endregion

    }
}

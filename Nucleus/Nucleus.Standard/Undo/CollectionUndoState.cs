using Nucleus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Undo
{
    /// <summary>
    /// An undo state which can be used to revert a collection change
    /// </summary>
    [Serializable]
    public class CollectionUndoState : UndoState
    {
        #region Properties

        public override bool IsValid
        {
            get
            {
                return Target != null;
            }
        }

        /// <summary>
        /// The object whose property state is being stored
        /// </summary>
        public IList Target { get; private set; }

        /// <summary>
        /// The collection changed event arguments
        /// </summary>
        public NotifyCollectionChangedEventArgs Args { get; private set; }

        #endregion

        #region Constructors

        public CollectionUndoState(IList target, NotifyCollectionChangedEventArgs args)
        {
            Target = target;
            Args = args;
        }

        #endregion

        #region Methods

        public override UndoState GenerateRedo()
        {
            return new CollectionUndoState(Target, Args.Reverse());
        }

        public override void Restore()
        {
            if (IsValid)
            {
                if (Args.Action == NotifyCollectionChangedAction.Add) // Remove added items
                {
                    foreach (object item in Args.NewItems)
                    {
                        Target.Remove(item);
                    }
                }
                else if (Args.Action == NotifyCollectionChangedAction.Remove) // Add back removed items
                {
                    if (Args.OldItems.Count == 1 && Args.OldStartingIndex.InRange(0, Target.Count - 1))
                    {
                        Target.Insert(Args.OldStartingIndex, Args.OldItems[0]);
                    }
                    else
                    {
                        foreach (object item in Args.OldItems)
                        {
                            Target.Add(item); //Will change position... OK?
                        }
                    }
                }
                else if (Args.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (Args.OldItems.Count == 1)
                    {
                        Target[Args.OldStartingIndex] = Args.OldItems[0];
                    }
                    else
                    {
                        //TODO?
                    }
                }
                
                
            }
        }

        #endregion
    }
}

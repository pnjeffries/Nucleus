using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FreeBuild.Selection
{
    /// <summary>
    /// Abstract base class for ViewModel wrapper classes representing selections of unique objects
    /// </summary>
    public abstract class SelectionViewModel : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Add an item to this selection, checking first whether it is of the appropriate type
        /// </summary>
        /// <param name="item">The item to be added</param>
        public abstract bool Add(object item);

        /// <summary>
        /// Remove an item from this selection, checking first whether it is of the appropriate type
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <returns></returns>
        public abstract bool Remove(object item);

        /// <summary>
        /// Clear the current selection
        /// </summary>
        public abstract void Clear();
    }

    /// <summary>
    /// Abstract generic base class for ViewModel wrapper classes representing selections of unique objects
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TUnique"></typeparam>
    public abstract class SelectionViewModel<TCollection, TUnique> : SelectionViewModel
        where TCollection : UniquesCollection<TUnique>, new() where TUnique : class, IUnique
    {

        /// <summary>
        /// The collection of selected objects
        /// </summary>
        public TCollection Selection { get; } = new TCollection();

        /// <summary>
        /// Constructor
        /// </summary>
        public SelectionViewModel()
        {
            Selection.CollectionChanged += HandleSelectionChanged;
        }

        /// <summary>
        /// Add a new item to the selection
        /// </summary>
        /// <param name="item"></param>
        public bool Add(TUnique item)
        {
            return Selection.TryAdd(item);
        }

        /// <summary>
        /// Add a new item to the selection, checking first whether it is of the appropriate type
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Add(object item)
        {
            if (item is TUnique)
            {
                return Add((TUnique)item);
            }
            else return false;
        }

        /// <summary>
        /// Remove an item from the selection
        /// </summary>
        /// <param name="item"></param>
        public bool Remove(TUnique item)
        {
               return Selection.Remove(item);
        }

        /// <summary>
        /// Remove an item from the selection, checking first whether it is of the appropriate type
        /// </summary>
        /// <param name="item"></param>
        public override bool Remove(object item)
        {
            if (item is TUnique)
            {
                return Remove((TUnique)item);
            }
            else return false;
        }

        /// <summary>
        /// Set the selection to the specified object
        /// </summary>
        /// <param name="item"></param>
        public void Set(TUnique item)
        {
            Clear();
            if (item != null) Add(item);
        }

        /// <summary>
        /// Does the selection contain the specified item?
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TUnique item)
        {
            return Selection.Contains(item.GUID);
        }

        /// <summary>
        /// Clear the selection
        /// </summary>
        public override void Clear()
        {
            Selection.Clear();
        }

        protected static object CombinedValue<V>(Func<V, object> propertyDelegate, IEnumerable<V> collection, object multiValue)
        {
            IEnumerable<object> values = collection.Select(propertyDelegate);
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
                {
                    return multiValue;
                }
            }
            return combinedValue;
        }

        /// <summary>
        /// Retrieve the combined value of the property specified in the lambda function
        /// </summary>
        /// <param name="propertyDelegate">A lambda function that returns a particular property for each item in the selection</param>
        /// <param name="multiValue">The return value that indicates multiple inconsistent values</param>
        /// <returns></returns>
        public object CombinedValue(Func<TUnique, object> propertyDelegate, object multiValue)
        {
            return Selection.CombinedValue(propertyDelegate, multiValue);
        }

        /// <summary>
        /// Retrieve the combined value of the property specified in the lambda function
        /// </summary>
        /// <param name="propertyDelegate">A lambda function that returns a particular property for each item in the selection</param>
        /// <returns></returns>
        public object CombinedValue(Func<TUnique, object> propertyDelegate)
        {
            return CombinedValue(propertyDelegate, "[Multi]");
        }

        /*public void SetAll(Func<U,object> propertyDelegate, object newValue)
        {
            ParameterExpression valuePE = Expression.Parameter(typeof(object));
            //Expression targetExp = 
            //TODO: See StackOverflow 'How to assign a value via Expression?'
        }*/

        /// <summary>
        /// Timer for deferred selection update notification
        /// </summary>
        private Timer _SelectionUpdateTimer;

        private void HandleSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Unique oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= HandleSelectedItemPropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (Unique newItem in e.NewItems)
                {
                    newItem.PropertyChanged += HandleSelectedItemPropertyChanged;
                }
            }

            if (_SelectionUpdateTimer == null)
            {
                _SelectionUpdateTimer = new Timer(100);
                _SelectionUpdateTimer.Elapsed += HandleSelectionUpdateTimerElapsed;
            }
            _SelectionUpdateTimer.Start();
        }

        private void HandleSelectedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        private void HandleSelectionUpdateTimerElapsed(object sender, EventArgs e)
        {
            _SelectionUpdateTimer.Stop();
            NotifyPropertyChanged(null);
        }
    }
}

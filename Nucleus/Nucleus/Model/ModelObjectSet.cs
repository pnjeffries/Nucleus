﻿using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract base class for parametrically defined 'sets' of objects which
    /// allow collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    [Serializable]
    public abstract class ModelObjectSetBase : ModelObject
    {

        /// <summary>
        /// Private backing field for All property
        /// </summary>
        private bool _All = false;

        /// <summary>
        /// Gets or sets whether this set initially (before filtering) contains all objects
        /// of the relevant type within the model that this set belongs to.
        /// </summary>
        public bool All
        {
            get { return _All; }
            set { _All = value; NotifyPropertyChanged("All"); }
        }
    }

    /// <summary>
    /// Abstract generic base class for parametrically defined 'sets' of objects which
    /// allow collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    [Serializable]
    public abstract class ModelObjectSet<TItem, TCollection> : ModelObjectSetBase
         where TItem : ModelObject
        where TCollection : ModelObjectCollection<TItem>, new()
    {
        #region Properties

        /// <summary>
        /// Private backing field for BaseCollection property
        /// </summary>
        private TCollection _BaseCollection = null;

        /// <summary>
        /// The base collection of items to be considered for inclusion in this set.
        /// Filters will be applied to this collection to determine the final collection of
        /// objects which constitute this set.
        /// If this and the SubSets property are null, but the Set has been added to a model,
        /// all objects of the relevant type within the model will be considered for inclusion.
        /// </summary>
        public TCollection BaseCollection
        {
            get
            {
                if (_BaseCollection == null) _BaseCollection = new TCollection();
                return _BaseCollection;
            }
            set { _BaseCollection = value; NotifyPropertyChanged("BaseCollection"); }
        }

        #endregion
    }

    /// <summary>
    /// Abstract generic base class for parametrically defined 'sets' of objects which
    /// allow collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    [Serializable]
    public abstract class ModelObjectSet<TItem, TCollection, TFilter, TFilterCollection, TSubSet, TSubSetCollection> : ModelObjectSet<TItem, TCollection>
        where TItem : ModelObject
        where TCollection : ModelObjectCollection<TItem>, new()
        where TFilter : class, ISetFilter<TItem>
        where TFilterCollection : SetFilterCollection<TFilter, TItem>, new()
        where TSubSet : ModelObjectSet<TItem, TCollection, TFilter, TFilterCollection, TSubSet, TSubSetCollection>
        where TSubSetCollection : ModelObjectSetCollection<TSubSet>, new()
    {
        #region Properties

        /// <summary>
        /// Private backing field for SubSets property
        /// </summary>
        private TSubSetCollection _SubSets = null;

        /// <summary>
        /// The base collection of other sets to be considered for inclusion in this set.
        /// FIlters will be applied to the expanded contents of these sets to determine the final
        /// collection of objects which constitute this set.
        /// If this and the BaseCollection property are null, but the set has been added to a model,
        /// all objects of the relevant type within the model will be considered for inclusion.
        /// </summary>
        public TSubSetCollection SubSets
        {
            get
            {
                if (_SubSets == null) _SubSets = new TSubSetCollection();
                return _SubSets;
            }
            set { _SubSets = value; NotifyPropertyChanged("SubSets"); }
        }

        /// <summary>
        /// Private backing field for Filters property
        /// </summary>
        private TFilterCollection _Filters = null;

        /// <summary>
        /// A set of conditional filters to be applied to the base collection and
        /// subsets of this set in order to parametrically determine the final set of
        /// items to be included within it.
        /// </summary>
        public TFilterCollection Filters
        {
            get
            {
                if (_Filters == null) _Filters = new TFilterCollection();
                return _Filters;
            }
        }

        /// <summary>
        /// Get the final set of items contained within this set, consisting of all items in the base collection
        /// and any subsets (or all items in the model if 'All' is true) that pass all filters specified via the
        /// Filters property.
        /// </summary>
        public TCollection Items
        {
            get { return GenerateItems(); }
        }

        /// <summary>
        /// Is this set circular - i.e. does it's definition include a reference to itself?
        /// </summary>
        public bool IsCircular
        {
            get { return ContainsReferenceTo((TSubSet)this); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Base default constructor
        /// </summary>
        public ModelObjectSet() { }

        /// <summary>
        /// Initialises an 'all objects' set
        /// </summary>
        /// <param name="all"></param>
        public ModelObjectSet(bool all)
        {
            All = true;
        }

        /// <summary>
        /// Initialises this set to contain a single item
        /// </summary>
        /// <param name="item"></param>
        public ModelObjectSet(TItem item)
        {
            BaseCollection.Add(item);
        }

        /// <summary>
        /// Initialises this set to contain the specified collection of items.
        /// </summary>
        /// <param name="items"></param>
        public ModelObjectSet(TCollection baseCollection)
        {
            BaseCollection = baseCollection;
        }

        /// <summary>
        /// Initialises a set containing all objects in the model,
        /// filtered by the specified condition
        /// </summary>
        /// <param name="filter"></param>
        public ModelObjectSet(TFilter filter) : this(true)
        {
            Filters.Add(filter);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new item to the base collection of this set, to be considered for inclusion.
        /// Note that adding an item to this set does not guarantee its inclusion should said
        /// item fail to pass any of the specified set filters.
        /// </summary>
        /// <param name="item"></param>
        public void Add(TItem item)
        {
            BaseCollection.TryAdd(item);
        }

        /// <summary>
        /// Add a collection of items to the base collection of this set, to be considered for
        /// inclusion.  Note that adding an item to this set does not guarantee its inclusion 
        /// should said item fail to pass any of the specified set filters.
        /// </summary>
        /// <param name="items"></param>
        public void Add(TCollection items)
        {
            BaseCollection.TryAddRange(items);
        }

        /// <summary>
        /// Add a new sub-set to the base collection of this set, to be considered for inclusion.
        /// Note that adding these items to this set does not guarantee their inclusion should said items
        /// fail to pass any of the specified set filters.
        /// </summary>
        /// <param name="set"></param>
        public void Add(TSubSet set)
        {
            if (_SubSets == null) _SubSets = new TSubSetCollection();
            _SubSets.TryAdd(set);
        }

        /// <summary>
        /// Add a new logical filter to this set.
        /// </summary>
        /// <param name="filter"></param>
        public void Add(TFilter filter)
        {
            if (_Filters == null) _Filters = new TFilterCollection();
            _Filters.TryAdd(filter);
        }

        /// <summary>
        /// Does this set contain a reference to the specified set within
        /// its SubSets property, either directly or indirectly?
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool ContainsReferenceTo(TSubSet set)
        {
            if (_SubSets != null)
            {
                foreach (TSubSet subSet in _SubSets)
                {
                    if (set == subSet) return true;
                    else if (subSet.ContainsReferenceTo(set)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Does this set contain the specified item?
        /// (or, would it, if they were part of the same model?)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TItem item)
        {
            if (All || BaseCollection.Contains(item.GUID))
            {
                return Filters.Pass(item);
            }
            return false;
        }

        /// <summary>
        /// Get all items of the relevent type from the model that this set belongs to
        /// </summary>
        /// <returns></returns>
        protected abstract TCollection GetItemsInModel();

        /// <summary>
        /// Generate the expanded collection that contains all items in this set
        /// </summary>
        /// <returns></returns>
        protected TCollection GenerateItems()
        {
            TCollection unfiltered = new TCollection(); // The collection of items to consider

            if (All && Model != null)
            {
                // All items in model:
                TCollection modelItems = GetItemsInModel();
                foreach (TItem item in modelItems)
                    unfiltered.Add(item);
            }

            if (BaseCollection != null)
            {
                // Items in base collection:
                foreach (TItem item in BaseCollection)
                    unfiltered.TryAdd(item);
            }

            if (SubSets != null)
            {
                // Items in subsets:
                foreach (TSubSet set in SubSets)
                {
                    // Expand each subset:
                    TCollection setItems = set.Items;
                    foreach (TItem item in setItems)
                        unfiltered.TryAdd(item);
                }
            }

            // Apply filters:
            if (Filters == null || Filters.Count == 0)
            {
                // (or not):
                return unfiltered;
            }
            else
            {
                TCollection result = new TCollection();
                foreach (TItem item in unfiltered)
                {
                    if (Filters.Pass(item))
                        result.Add(item);
                }
                return result;
            }
        }

        #endregion
    }
}

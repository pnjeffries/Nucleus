﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Extensions;

namespace Nucleus.Model
{
    /// <summary>
    /// A generic collection of ModelObjectSet objects
    /// </summary>
    /// <typeparam name="TSet"></typeparam>
    [Serializable]
    public class ModelObjectSetCollection<TSet> : ModelObjectCollection<TSet>
        where TSet : ModelObjectSetBase, IModelObjectSet
    {

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new model object set collection
        /// </summary>
        public ModelObjectSetCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a model object set collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        protected ModelObjectSetCollection(Model model) : base(model) { }

        #endregion

        #region Methods

        /// <summary>
        /// Find a set in this collection of the specified type and with the specified name
        /// or if a match does not exist create a new one
        /// </summary>
        /// <typeparam name="TSetType">The type of set to search for</typeparam>
        /// <param name="name">The name to search for</param>
        /// <returns></returns>
        public TSetType FindOrCreate<TSetType>(string name)
            where TSetType : TSet, new()
        {
            TSetType set = FindByName<TSetType>(name);
            if (set == null)
            {
                set = new TSetType();
                set.Name = name;
                TryAdd(set);
            }
            return set;
        }

        /// Add a set to this collection, if it is of a compatible type
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool TryAdd(IModelObjectSet set)
        {
            if (set is TSet)
            {
                Add((TSet)set);
                return true;
            }
            else return false;
        }

        #endregion
    }

    /// <summary>
    /// A collection of ModelObjectSets
    /// </summary>
    [Serializable]
    public class ModelObjectSetCollection : ModelObjectSetCollection<ModelObjectSetBase>
    {
        #region Properties

        /// <summary>
        /// Get a collection of all the sets of elements within this collection
        /// </summary>
        public ModelObjectSetCollection<ElementSet> ElementSets
        {
            get
            {
                var result = new ModelObjectSetCollection<ElementSet>();
                this.ExtractAllOfType(result);
                return result;
            }
        }

        /// <summary>
        /// Get a collection of all the sets of nodes within this collection
        /// </summary>
        public ModelObjectSetCollection<NodeSet> NodeSets
        {
            get {
                var result = new ModelObjectSetCollection<NodeSet>();
                this.ExtractAllOfType(result);
                return result;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new model object set collection
        /// </summary>
        public ModelObjectSetCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a model object set collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        protected ModelObjectSetCollection(Model model) : base(model) { }

        #endregion

    }
}

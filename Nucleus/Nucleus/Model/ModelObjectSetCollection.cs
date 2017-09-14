using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Add a set to this collection, if it is of a compatible type
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool Add(IModelObjectSet set)
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

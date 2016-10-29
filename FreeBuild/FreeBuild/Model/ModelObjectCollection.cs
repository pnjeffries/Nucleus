using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of model objects.
    /// Generic version to allow further specificity of object type.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    public class ModelObjectCollection<TItem> : OwnedCollection<TItem, Model> where TItem : ModelObject
    {
        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new model object collection
        /// </summary>
        public ModelObjectCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a model object collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        protected ModelObjectCollection(Model model) : base(model) { }

        /// <summary>
        /// Collection combination constructor
        /// </summary>
        /// <param name="toBeCombined"></param>
        public ModelObjectCollection(IEnumerable<IEnumerable<TItem>> toBeCombined) : base(toBeCombined) { }

        #endregion

        #region Methods

        protected override void SetItemOwner(TItem item)
        {
            if (Owner != null) item.Model = Owner;
        }

        protected override void ClearItemOwner(TItem item)
        {
            if (Owner != null) item.Model = null;
        }

        #endregion
    }

    /// <summary>
    /// A collection of model objects
    /// </summary>
    [Serializable]
    public class ModelObjectCollection : ModelObjectCollection<ModelObject>
    {
        /// <summary>
        /// Default constructor.  Initialises a new model object collection
        /// </summary>
        public ModelObjectCollection() : base() { }

        /// <summary>
        /// Collection combination constructor
        /// </summary>
        /// <param name="toBeCombined"></param>
        public ModelObjectCollection(IEnumerable<IEnumerable<ModelObject>> toBeCombined) : base(toBeCombined) { }
    }
}

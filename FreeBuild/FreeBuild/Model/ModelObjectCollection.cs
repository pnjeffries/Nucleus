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
        #region Properties

        /// <summary>
        /// Get or set the combined name value of the objects in this collection.
        /// If all the objects in this collection have the same name, that name will be returned.
        /// Otherwise the string "[Multi]" will be returned.
        /// Set this property to set the name property of all objects in this collection
        /// </summary>
        public virtual string Name
        {
            get { return (string)CombinedValue(i => i.Name, "[Multi]"); }
            set { foreach (TItem item in this) item.Name = value; }
        }

        #endregion

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

        /// <summary>
        /// Find the first item in this collection which has the specified name (if any)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual TItem FindByName(string name)
        {
            foreach(TItem mO in this)
            {
                if (mO.Name == name) return mO;
            }
            return null;
        }

        /// <summary>
        /// Return the next version of this name with an attached numerical postfix that
        /// will be a unique name in this collection.
        /// </summary>
        /// <param name="baseName">The base name</param>
        /// <param name="enforcePostFix">Optional.  If set true, a postfix numeral will always be applied, even if it is 1.</param>
        /// <returns></returns>
        public string NextAvailableName(string baseName, bool enforcePostFix = false)
        {
            if (!enforcePostFix && FindByName(baseName) == null) return baseName;
            else
            {
                int postFix = 2;
                if (enforcePostFix) postFix = 1;
                while (postFix < 100000)
                {
                    string nextName = baseName + " " + postFix;
                    if (FindByName(nextName) == null) return nextName;
                    postFix++;
                }
            }
            return baseName;
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

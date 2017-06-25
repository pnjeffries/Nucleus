using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract base class for collections of coordinate system references
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    public abstract class CoordinateSystemReferenceCollection<TItem> : ModelObjectCollection<TItem>
        where TItem : CoordinateSystemReference
    {
        /// <summary>
        /// Default constructor.  Initialises a new model object collection
        /// </summary>
        public CoordinateSystemReferenceCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a model object collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        protected CoordinateSystemReferenceCollection(Model model) : base(model) { }
    }

    /// <summary>
    /// A collection of coordinate system references
    /// </summary>
    [Serializable]
    public class CoordinateSystemReferenceCollection : CoordinateSystemReferenceCollection<CoordinateSystemReference>
    {
        /// <summary>
        /// Default constructor.  Initialises a new model object collection
        /// </summary>
        public CoordinateSystemReferenceCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a model object collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        protected CoordinateSystemReferenceCollection(Model model) : base(model) { }
    }
}

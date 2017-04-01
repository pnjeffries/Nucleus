using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of user coordinate systems
    /// </summary>
    [Serializable]
    public class UserCoordinateSystemReferenceCollection 
        : CoordinateSystemReferenceCollection<UserCoordinateSystemReference>
    {
        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new model object collection
        /// </summary>
        public UserCoordinateSystemReferenceCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a model object collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        protected UserCoordinateSystemReferenceCollection(Model model) : base(model) { }

        #endregion
    }
}

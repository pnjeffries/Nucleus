using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of levels.
    /// </summary>
    [Serializable]
    public class LevelCollection : ModelObjectCollection<Level>
    {
        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new empty LevelCollection.
        /// </summary>
        public LevelCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a new LevelCollection owned by the specified model.
        /// </summary>
        /// <param name="model">The model that owns the items in this collection</param>
        public LevelCollection(Model model) : base(model) { }

        #endregion
    }
}

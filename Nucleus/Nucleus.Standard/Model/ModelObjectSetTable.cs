using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A table of ModelObjectSets to be stored in a Model
    /// </summary>
    [Serializable]
    public class ModelObjectSetTable : ModelObjectSetCollection
    {
        #region Constructors

        /// <summary>
        /// Owner constructor.  Initialises a model object set collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        public ModelObjectSetTable(Model model) : base(model)
        {
        }

        #endregion
    }
}

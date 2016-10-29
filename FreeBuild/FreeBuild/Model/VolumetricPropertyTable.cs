using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// An extended VolumetricPropertyCollection that also contains additional
    /// temporary data structures for fast lookup operations.
    /// </summary>
    [Serializable]
    public class VolumetricPropertyTable : VolumetricPropertyCollection
    {
        #region Constructors

        /// <summary>
        /// Initialises a new VolumetricPropertyTable belonging to the specified model
        /// </summary>
        /// <param name="model"></param>
        public VolumetricPropertyTable(Model model) : base(model) { }

        #endregion
    }
}

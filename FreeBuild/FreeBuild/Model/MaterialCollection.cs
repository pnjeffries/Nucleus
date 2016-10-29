using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of Materials
    /// </summary>
    [Serializable]
    public class MaterialCollection : ModelObjectCollection<Material>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MaterialCollection(): base() { }

        /// <summary>
        /// Owner constructor
        /// </summary>
        /// <param name="model"></param>
        protected MaterialCollection(Model model) : base(model) { }

        #endregion
    }
}

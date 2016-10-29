using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// An extended material collection which may contain additional temporary data structures 
    /// to enable fast object lookup
    /// </summary>
    [Serializable]
    public class MaterialTable : MaterialCollection
    {
        #region Constructor

        /// <summary>
        /// Initialises a new MaterialTable belonging to 
        /// </summary>
        /// <param name="model"></param>
        public MaterialTable(Model model) : base(model) { }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A table of load cases, providing the top-level representation of
    /// structural loading in the model
    /// </summary>
    [Serializable]
    public class LoadCaseTable : LoadCaseCollection
    {
        #region Constructors

        /// <summary>
        /// Initialise a new LoadCaseTable belonging to the specified model
        /// </summary>
        /// <param name="model"></param>
        public LoadCaseTable(Model model) : base(model) { }

        #endregion
    }
}

using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Selection
{
    /// <summary>
    /// Class for dummy section properties.
    /// </summary>
    public class SectionPropertyDummy : SectionProperty
    {
        #region Constructors

        /// <summary>
        /// Initialise a new dummy section property with the specified name
        /// </summary>
        /// <param name="name"></param>
        public SectionPropertyDummy(string name) { Name = name; }

        #endregion
    }
}

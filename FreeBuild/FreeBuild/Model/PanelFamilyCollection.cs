using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of panel families
    /// </summary>
    [Serializable]
    public class PanelFamilyCollection : ModelObjectCollection<PanelFamily>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PanelFamilyCollection() : base() { }

        /// <summary>
        /// Initialise a new PanelFamilyCollection containing the specified single item
        /// </summary>
        /// <param name="family"></param>
        public PanelFamilyCollection(PanelFamily family) : base()
        {
            Add(family);
        }

        /// <summary>
        /// Initialise a new PanelFamilyCollection containing the specified set of sections
        /// </summary>
        /// <param name="families"></param>
        public PanelFamilyCollection(IEnumerable<PanelFamily> families)
        {
            if (families != null)
            {
                foreach (PanelFamily family in families)
                {
                    Add(family);
                }
            }
        }

        #endregion
    }
}

using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model.Loading
{
    /// <summary>
    /// A force load applied directly to the faces of panel elements
    /// </summary>
    #if !JS
    [Serializable]
#endif
    public class PanelLoad : ForceLoad<PanelElementSet, PanelElement>
    {

        #region Constructor

        /// <summary>
        /// Default constructor.  Initialises a new blank area load.
        /// </summary>
        public PanelLoad() : base() { }

        /// <summary>
        /// Initialise a new area load over the specified area to be resolved to the
        /// specified elements.
        /// </summary>
        /// <param name="appliedOver"></param>
        /// <param name="appliedTo"></param>
        public PanelLoad(ElementCollection appliedTo) : this()
        {
            AppliedTo.Add(appliedTo);
        }

        #endregion

        #region Methods

        /*
        public override string GetValueUnits()
        {
            return base.GetValueUnits() + "/m²";
        }*/

        #endregion
    }
}

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
    /// A force load applied over a particular planar region
    /// </summary>
    [Serializable]
    public class AreaLoad : ForceLoad<ElementSet, Element>
    {
        #region Properties

        /// <summary>
        /// Private backing field for AppliedOver property
        /// </summary>
        private PlanarRegion _AppliedOver;

        /// <summary>
        /// The region over which the load is applied.
        /// If this is not specified, the load is assumed to apply
        /// directly over the entire surface of any specified panel
        /// elements, but not at all to linear elements.
        /// </summary>
        public PlanarRegion AppliedOver
        {
            get { return _AppliedOver; }
            set { ChangeProperty(ref _AppliedOver, value, "AppliedOver"); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.  Initialises a new blank area load.
        /// </summary>
        public AreaLoad() : base() { }

        /// <summary>
        /// Initialise a new area load over the specified area to be resolved to the
        /// specified elements.
        /// </summary>
        /// <param name="appliedOver"></param>
        /// <param name="appliedTo"></param>
        public AreaLoad(PlanarRegion appliedOver, ElementCollection appliedTo) : this()
        {
            _AppliedOver = appliedOver;
            AppliedTo.Add(appliedTo);
        }

        #endregion

        #region Methods

        public override string GetValueUnits()
        {
            return base.GetValueUnits() + "/m²";
        }

        #endregion
    }
}

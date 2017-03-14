using FreeBuild.Geometry;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model.Loading
{
    /// <summary>
    /// A force load applied over a particular planar region
    /// </summary>
    public class AreaLoad : ForceLoad<ElementSet>
    {
        #region Properties

        /// <summary>
        /// Private backing field for AppliedOver property
        /// </summary>
        private PlanarRegion _AppliedOver;

        /// <summary>
        /// The region over which the load is applied
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
    }
}

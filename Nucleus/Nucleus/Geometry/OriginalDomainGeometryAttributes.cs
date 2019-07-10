using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Specialised type of GeometryAttributes which can be used to store
    /// the subdomain of a curve as part of a larger curve.  Typically
    /// to allow for interpolation between offset curves where segments
    /// may have collapsed.
    /// </summary>
    [Serializable]
    public class OriginalDomainGeometryAttributes : GeometryAttributes
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the SubDomain property
        /// </summary>
        private Interval _SubDomain = Interval.Unset;

        /// <summary>
        /// The subdomain of this curve within a longer parent curve
        /// </summary>
        public Interval SubDomain
        {
            get { return _SubDomain; }
            set { _SubDomain = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new set of attributes with default values
        /// </summary>
        public OriginalDomainGeometryAttributes() { }

        /// <summary>
        /// Initialise a new set of attributes copying from another
        /// </summary>
        /// <param name="other">Another set of geometry attributes.  This may be null,
        /// in which case default values will be retained.</param>
        public OriginalDomainGeometryAttributes(GeometryAttributes other)
            : base(other)
        {

        }

        /// <summary>
        /// Initialise a new set of attributes copying from another
        /// </summary>
        /// <param name="other">Another set of geometry attributes.  This may be null,
        /// in which case default values will be retained.</param>
        /// <param name="subDomain">The subdomain interval to store</param>
        public OriginalDomainGeometryAttributes(GeometryAttributes other, Interval subDomain)
            : this(other)
        {
            _SubDomain = subDomain;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Immutable geometric primitive representing a planar region within a
    /// set radius of an origin point.
    /// </summary>
    public class Disk : CylindricalCoordinateSystem
    {
        #region Fields

        /// <summary>
        /// The radius of the circle that encloses this disk
        /// </summary>
        public readonly double Radius;

        #endregion

        #region Constructors

        /// <summary>
        /// Radius constructor
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        public Disk(double radius) : base()
        {
            Radius = radius;
        }

        #endregion
    }
}

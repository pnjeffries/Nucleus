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
        /// Creates a disk centred on the origin on the global XY plane
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        public Disk(double radius) : base()
        {
            Radius = radius;
        }

        /// <summary>
        /// Radius, coordinatesystem constructor.
        /// Creates a disk lying on the reference plane and centred on the origin
        /// of the specified coordinate system.
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="cSystem">The coordinate system on which the circle will be placed</param>
        public Disk(double radius, CylindricalCoordinateSystem cSystem) : base(cSystem)
        {
            Radius = radius;
        }

        /// <summary>
        /// Create a Disk of the specified radius about the specified centrepoint.
        /// The disk will be oriented to the global XY plane.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="centre"></param>
        public Disk(double radius, Vector centre) : base(centre)
        {
            Radius = radius;
        }

        /// <summary>
        /// Create a Disk of the specified radius about the specified centrepoint
        /// lying on a plane perpendicular to the given normal direction
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="centre"></param>
        /// <param name="normal"></param>
        public Disk(double radius, Vector centre, Vector normal) : base(centre, normal)
        {
            Radius = radius;
        }

        #endregion
    }
}

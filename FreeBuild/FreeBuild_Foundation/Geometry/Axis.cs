using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An infinite straight line defined by an origin point and direction.
    /// Immutable geometric primitive.
    /// </summary>
    [Serializable]
    public class Axis
    {
        #region Fields

        /// <summary>
        /// Position vector describing a point on this axis.
        /// </summary>
        public readonly Vector Origin;

        /// <summary>
        /// Direction vector describing the direction of this axis.
        /// </summary>
        public readonly Vector Direction;

        #endregion

        #region Properties

        /// <summary>
        /// Is this axis definition valid?
        /// An axis is valid provided origin and direction vectors 
        /// are valid and it has a non-zero direction vector.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Origin.IsValid() && Direction.IsValid() && !Direction.IsZero();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, creating an axis defined by an origin point and direction vector
        /// </summary>
        /// <param name="origin">The origin point of the axis</param>
        /// <param name="direction">The direction vector of the axis</param>
        public Axis(Vector origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find the position along this axis, as a multiplication factor of the direction vector
        /// where this axis crosses a plane.
        /// </summary>
        /// <param name="plane">The plane to find the intersection point with.</param>
        /// <returns>The factor that it is necessary to multiply the direction factor by and add to the origin in order to
        /// obtain the intersection point, if an intersection point exists.
        /// Use PointAt to resolve this to a vector if required.
        /// If the Axis is parallel to the plane and there is no intersection then double.NaN will be returned instead.</returns>
        public double IntersectPlane(Plane plane)
        {
            Vector normal = plane.Z;
            double directionProjection = Direction.Dot(normal);

            if (directionProjection == 0) return double.NaN;
            else
            {
                double originProjection = Origin.Dot(normal);
                double planeProjection = plane.Origin.Dot(normal);
                return (originProjection - planeProjection) / directionProjection;
            }
        }

        /// <summary>
        /// Find the position along this axis that is closest to the specified point.
        /// Expressed as a multiplication factor of the direction vector from the origin.
        /// </summary>
        /// <param name="point">The test point.</param>
        /// <returns>The parameter, t, on this axis that describes
        /// the point on this axis closest to the test point.  Expressed as
        /// a multiplication factor of the direction vector from the origin.
        /// Use PointAt to evaluate this as a vector if required.</returns>
        public double ClosestPoint(Vector point)
        {
            Vector OP = point - Origin;
            return OP.Dot(Direction) / Direction.MagnitudeSquared();
        }

        /// <summary>
        /// Find the position along this axis described by a parameter
        /// representing a multiplication of the direction vector from the
        /// origin point.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector PointAt(double t)
        {
            return Origin + Direction * t;
        }

        #endregion
    }
}

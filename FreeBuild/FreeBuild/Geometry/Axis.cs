// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using FreeBuild.Base;
using FreeBuild.Extensions;
using FreeBuild.Units;
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
    public class Axis : IDuplicatable
    {
        #region Fields

        /// <summary>
        /// Position vector describing a point on this axis.
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector Origin;

        /// <summary>
        /// Direction vector describing the direction of this axis.
        /// </summary>
        [Dimension(DimensionType.Distance)]
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
        public double Closest(Vector point)
        {
            Vector OP = point - Origin;
            return OP.Dot(Direction) / Direction.MagnitudeSquared();
        }

        /// <summary>
        /// Find the position along this axis that is closest to the specified
        /// other axis.
        /// Expressed as a multiplication factor of the direction vector from the origin.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="t">OUTPUT.  The parameter on the other axis.</param>
        /// <returns>The parameter on this axis describing the closest point to the other axis.
        /// Use PointAt to resolve this into a vector if required.</returns>
        /// <remarks>Algorithm based on http://geomalgorithms.com/a07-_distance.html </remarks>
        public double ClosestParameter(Axis other, out double t)
        {
            Vector w0 = Origin - other.Origin; //w0 = P0 - Q0
            double a = Direction.Dot(Direction); //a = u*u
            double b = Direction.Dot(other.Direction); //b = u*v
            double c = other.Direction.Dot(other.Direction); //c = v*v
            double d = Direction.Dot(w0); //d = u*w0
            double e = other.Direction.Dot(w0); //e = v*w0
            double s = (b * e - c * d) / (a * c - b * b); //sc = be-cd/(ac - b^2)
            t = (a * e - b * d) / (a * c - b * b);//tc = ae-bd/(a*c - b^2)
            return s;
        }

        /// <summary>
        /// Find the position along this axis that is closest to the specified
        /// other axis.
        /// Expressed as a multiplication factor of the direction vector from the origin.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The parameter on this axis describing the closest point to the other axis.
        /// Use PointAt to resolve this into a vector if required.</returns>
        /// <remarks>Algorithm based on http://geomalgorithms.com/a07-_distance.html </remarks>
        public double ClosestParameter(Axis other)
        {
            double t;
            return ClosestParameter(other, out t);
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

        #region Static Methods

        /// <summary>
        /// Utility function to find the closest point between two axes
        /// expressed by starting positions and vectors
        /// </summary>
        /// <param name="pt0">The origin of the first axis</param>
        /// <param name="v0">The direction of the first axis</param>
        /// <param name="pt1">The origin of the second axis</param>
        /// <param name="v1">The direction of the second axis</param>
        /// <returns>The closest point on the first axis to the second.</returns>
        public static Vector ClosestPoint(Vector pt0, Vector v0, Vector pt1, Vector v1)
        {
            Vector w0 = pt0 - pt1; //w0 = P0 - Q0
            double a = v0.Dot(v0); //a = u*u
            double b = v0.Dot(v1); //b = u*v
            double c = v1.Dot(v1); //c = v*v
            double d = v0.Dot(w0); //d = u*w0
            double e = v1.Dot(w0); //e = v*w0
            double s = (b * e - c * d) / (a * c - b * b); //sc = be-cd/(ac - b^2)
            return pt0 + v0 * s;
        }

        /// <summary>
        /// Find the position along an infinite axis expressed by an origin point and
        /// direction which is closest to a point, expressed as a multiplication factor of that
        /// direction vector from that origin.
        /// </summary>
        /// <param name="origin">The origin point of the axis</param>
        /// <param name="direction">The direction vector of the axis</param>
        /// <param name="point">The point to find the closest distance to</param>
        /// <returns></returns>
        public static double ClosestParameter(Vector origin, Vector direction, Vector point)
        {
            Vector OP = point - origin;
            return OP.Dot(direction) / direction.MagnitudeSquared();
        }

        /// <summary>
        /// Find the position along an infinite axis expressed by an origin point and
        /// direction which is closest to a point, expressed as a point
        /// </summary>
        /// <param name="origin">The origin point of the axis</param>
        /// <param name="direction">The direction vector of the axis</param>
        /// <param name="point">The point to find the closest distance to</param>
        /// <returns></returns>
        public static Vector ClosestPoint(Vector origin, Vector direction, Vector point)
        {
            Vector OP = point - origin;
            double t = OP.Dot(direction) / direction.MagnitudeSquared();
            return origin + direction * t;
        }

        #endregion
    }
}

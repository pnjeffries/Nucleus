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

using Nucleus.Exceptions;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Immutable geometric primitive representing a planar region within a
    /// set radius of an origin point.
    /// </summary>
    [Serializable]
    public class Circle : CylindricalCoordinateSystem
    {
        #region Fields

        /// <summary>
        /// The radius of the circle
        /// </summary>
        public readonly double Radius;

        #endregion

        #region Properties

        /// <summary>
        /// The circumference of the circle
        /// </summary>
        public double Circumference
        {
            get
            {
                return 2 * Math.PI * Radius;
            }
        }

        /// <summary>
        /// The area enclosed by this circle on its own plane
        /// </summary>
        public double Area
        {
            get
            {
                return Math.PI * Radius * Radius;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Radius constructor
        /// Creates a disk centred on the origin on the global XY plane
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        public Circle(double radius) : base()
        {
            Radius = radius;
        }

        /// <summary>
        /// Radius, coordinatesystem constructor.
        /// Creates a circle lying on the reference plane and centred on the origin
        /// of the specified coordinate system.
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="cSystem">The coordinate system on which the circle will be placed</param>
        public Circle(double radius, CylindricalCoordinateSystem cSystem) : base(cSystem)
        {
            Radius = radius;
        }

        /// <summary>
        /// Radius, coordinatesystem, origin constructor.
        /// Creates a circle orientated to the given coordinate system but with a new origin
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="cSystem">The coordinate system on which the circle will be placed</param>
        public Circle(double radius, CylindricalCoordinateSystem cSystem, Vector origin) : base(cSystem, origin)
        {
            Radius = radius;
        }

        /// <summary>
        /// Radius, Coordinate System constructor.
        /// Creates a circle on the XY plane of the given coordinate system with the specified radius.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="cSystem"></param>
        public Circle(double radius, CartesianCoordinateSystem cSystem) : base(cSystem)
        {
            Radius = radius;
        }

        /// <summary>
        /// Duplication constructor.
        /// Create a copy of the other circle.
        /// </summary>
        /// <param name="other"></param>
        public Circle(Circle other) : this(other.Radius,other)
        {

        }

        /// <summary>
        /// Dupication constructor.
        /// Create a copy of the other circle, moving its centroid to the specified point
        /// </summary>
        /// <param name="other"></param>
        /// <param name="origin"></param>
        public Circle(Circle other, Vector origin) : this(other.Radius, other, origin) { }

        /// <summary>
        /// Create a Disk of the specified radius about the specified centrepoint.
        /// The disk will be oriented to the global XY plane.
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="centre">The centre point of the circle</param>
        public Circle(double radius, Vector centre) : base(centre)
        {
            Radius = radius;
        }

        /// <summary>
        /// Create a Disk of the specified radius about the specified centrepoint
        /// lying on a plane perpendicular to the given normal direction
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="centre">The centrepoint of the circle</param>
        /// <param name="normal">The normal vector to the plane the circle lies on</param>
        public Circle(double radius, Vector centre, Vector normal) : base(centre, normal)
        {
            Radius = radius;
        }

        /// <summary>
        /// Initialise a circle from three points.  The positions specified must all be different.
        /// </summary>
        /// <param name="pt0">The first point on the circle.  Will be used as the 'start' point
        /// of the circle as well.</param>
        /// <param name="pt1">The second point on the circle</param>
        /// <param name="pt2">The third point on the circle</param>
        public Circle(Vector pt0, Vector pt1, Vector pt2)
            : this(ref pt0, ref pt1, ref pt2, 0)
        {
        }

        /// <summary>
        /// Initialise a circle from three points
        /// Internal version that allows the radius to be calculated only once.
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        private Circle(ref Vector pt0, ref Vector pt1, ref Vector pt2, double radius)
            : base(Calculate3PtCSystem(ref pt0, ref pt1, ref pt2, ref radius))
        {
            Radius = radius;
        }

        /// <summary>
        /// Calculate the data needed for initialising a circle from 3 points
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        private static Vector[] Calculate3PtCSystem(ref Vector pt0, ref Vector pt1, ref Vector pt2, ref double radius)
        {
            Vector v01 = (pt1 - pt0);
            Vector v12 = (pt2 - pt1);
            //Find mid-points
            Vector mid01 = pt0.Interpolate(pt1, 0.5);
            Vector mid12 = pt1.Interpolate(pt2, 0.5);
            Vector normal = v01.Cross(v12).Unitize();
            //Find perpendicular axes
            Vector axis1 = v01.Cross(normal);
            Vector axis2 = v12.Cross(normal);
            //FInd intersection:
            Vector origin = Axis.ClosestPoint(mid01, axis1, mid12, axis2);
            Vector vO0 = pt0 - origin;
            radius = vO0.Magnitude();
            if (radius == 0) return new Vector[] { pt0, Vector.UnitZ, Vector.UnitX }; //Zero-radius circle!
            return new Vector[] { origin, normal, vO0 / radius };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find the closest point on this circle to the specified test point.
        /// Expressed as a circle parameter which is itself a rotation angle
        /// counter-clockwise around the circle.
        /// </summary>
        /// <param name="toPoint">The point to test.</param>
        /// <returns>The closest position as an angle</returns>
        public double Closest(Vector toPoint)
        {
            return Azimuth(toPoint);
        }

        /// <summary>
        /// Find the closest point on this circle to the specified test point.
        /// Expressed as a position vector.
        /// </summary>
        /// <param name="toPoint">The point to test.</param>
        /// <returns>The closest position as a Vector</returns>
        public Vector ClosestPoint(Vector toPoint)
        {
            return PointAt(Closest(toPoint));
        }

        /// <summary>
        /// Does the (projection of the) specified point lie within this circle?
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInside(Vector point)
        {
            Vector relative = point - Origin;
            double z = relative.Dot(L);
            Vector onPlane = relative - L * z;
            return onPlane.MagnitudeSquared() < Radius.Squared();
        }

        /// <summary>
        /// Find the position on the circle at the specified angular parameter
        /// </summary>
        /// <param name="t">An angle around the circle, in Radians</param>
        /// <param name="t">An angle (anti-clockwise) around the circle, in Radians</param>
        /// <returns>The point on the circle at the specified parameter</returns>
        public Vector PointAt(double t)
        {
            return LocalToGlobal(Radius, t);
        }

        /// <summary>
        /// Divide this circle into a number of equal-length segments and return
        /// the division points between those segments.
        /// </summary>
        /// <param name="divisions">The number of segments to divide the circle into</param>
        /// <returns></returns>
        public Vector[] Divide(int divisions)
        {
            var result = new Vector[divisions];
            for (int i = 0; i < divisions; i++)
            {
                result[i] = PointAt(i * 2 * Math.PI / divisions);
            }
            return result;
        }

        /// <summary>
        /// Divide an arc segment of this circle into a number of equal-length segments
        /// and return the division points between those segments
        /// </summary>
        /// <param name="divisions"></param>
        /// <param name="arcStart"></param>
        /// <param name="arcEnd"></param>
        /// <returns></returns>
        public Vector[] Divide(int divisions, Angle arcStart, Angle arcEnd)
        {
            var result = new Vector[divisions + 1];
            for (int i = 0; i <= divisions; i++)
            {
                result[i] = PointAt(arcStart + i * (arcEnd - arcStart) / divisions);
            }
            return result;
        }

        /// Find the position on the circle at the specified angular parameter
        /// </summary>
        /// <param name="t">An angle (anti-clockwise) around the circle, in Radians</param>
        /// <returns>The point on the circle at the specified parameter</returns>
        public Vector PointAt(Angle t)
        {
            return LocalToGlobal(Radius, t);
        }

        /// <summary>
        /// Find the unit tangent vector to the circle at the specified angular
        /// parameter
        /// </summary>
        /// <param name="t">An angle (anti-clockwise) around the circle, in Radians</param>
        /// <returns></returns>
        public Vector TangentAt(double t)
        {
            Vector localY = L.Cross(A);
            return localY.Rotate(L, t);
        }

        /// <summary>
        /// Find the unit tangent vector to the circle at the specified angular
        /// parameter
        /// </summary>
        /// <param name="t">An angle (anti-clockwise) around the circle, in Radians</param>
        /// <returns></returns>
        public Vector TangentAt(Angle t)
        {
            Vector localY = L.Cross(A);
            return localY.Rotate(L, t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Axis TangentAxisAt(Angle t)
        {
            return new Axis(PointAt(t), TangentAt(t));
        }

        /// <summary>
        /// Get the plane on which this circle lies
        /// </summary>
        /// <returns></returns>
        public Plane Plane()
        {
            return new Plane(Origin, L);
        }

        /// <summary>
        /// Create a copy of this circle, moved along the specified translation vector
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public Circle Move(Vector translation)
        {
            return new Circle(this, Origin + translation);
        }

        /// <summary>
        /// Create a transformed copy of this circle
        /// !!!TEMPORARY IMPLEMENTATION THAT ONLY WORKS FOR SIMPLE UNIFORM SCALING!!!
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Circle Transform(Transform t)
        {
            // TEMP!  Applies x-scaling factor (uniformly) but does nothing else!
            return new Circle(Math.Abs(this.Radius * t[0, 0]), this);
        }

        /// <summary>
        /// Calculate the maximum distance this circle could be moved in the specified
        /// direction while still keeping the specfied point within the circle.
        /// Returned as a distance along the direction vector.
        /// </summary>
        /// <param name="point">A point.  Must lie within the circle.</param>
        /// <param name="direction">A vector in the direction of movement.</param>
        /// <returns></returns>
        public double MaximumShiftWhileCovering(Vector point, Vector direction)
        {
            Vector d = direction.Project(L);
            if (d.IsZero()) return double.PositiveInfinity; //Vector is normal!
            else d = d.Unitize();
            Vector AO = (Origin - point).Project(L);
            if (AO.MagnitudeSquared() > Radius * Radius) return double.NaN; //Point is outside circle!
            Vector p = d.Rotate(L, Angle.Right);
            double m = Math.Sqrt(Radius.Squared() - (AO * p).Squared()) - AO * d;
            return m;
        }

        #endregion
    }
}

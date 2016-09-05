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

using FreeBuild.Exceptions;
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
        /// Creates a disk lying on the reference plane and centred on the origin
        /// of the specified coordinate system.
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="cSystem">The coordinate system on which the circle will be placed</param>
        public Circle(double radius, CylindricalCoordinateSystem cSystem) : base(cSystem)
        {
            Radius = radius;
        }

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
        /// Find the position on the circle at the specified angular parameter
        /// </summary>
        /// <param name="t">An angle around the circle, in Radians</param>
        /// <returns>The point on the circle at the specified parameter</returns>
        public Vector PointAt(double t)
        {
            return LocalToGlobal(Radius, t);
        }

        #endregion
    }
}

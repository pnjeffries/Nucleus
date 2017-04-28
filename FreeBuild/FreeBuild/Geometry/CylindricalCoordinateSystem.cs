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
using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Represents a cylindrical coordinate system defined by an origin,
    /// longitudinal axis and a polar axis lying in the reference plane, and
    /// positions are defined by radius, rotation around the longitudinal axis
    /// from the polar one and a distance from the reference plane, or r, theta
    /// and z respectively.
    /// Positive values of theta represent anticlockwise rotations around the
    /// longitudinal axis.
    /// </summary>
    [Serializable]
    public class CylindricalCoordinateSystem :
        ICoordinateSystem, IDuplicatable
    {
        #region Constants

        /// <summary>
        /// The default cylindrical coordinate system.
        /// The origin is at the global origin.
        /// The longitudinal axis is set to the global z-axis
        /// The polar axis is set to the global x-axis.
        /// </summary>
        public static CylindricalCoordinateSystem Default = new CylindricalCoordinateSystem();

        #endregion

        #region Fields

        /// <summary>
        /// The origin point of the coordinate system
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector Origin;

        /// <summary>
        /// The unit vector defining the longitudinal axis
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector L;

        /// <summary>
        /// The unit vector defining the polar axis on the reference plane
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector A;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Creates a cylindrical coordinate system with the
        /// longitudinal axis aligned along the global z-axis and the polar axis
        /// aligned along the global x-axis, with its origin at the global origin.
        /// </summary>
        public CylindricalCoordinateSystem()
        {
            Origin = Vector.Zero;
            L = Vector.UnitZ;
            A = Vector.UnitX;
        }

        /// <summary>
        /// Origin constructor.  Creates a cylindrical coordinate system with the 
        /// longitudinal axis aligned along the global z-axis and the polar axis
        /// aligned along the global x-axis, with its origin at the specified point.
        /// </summary>
        /// <param name="origin">The origin point</param>
        public CylindricalCoordinateSystem(Vector origin)
        {
            Origin = origin;
            L = Vector.UnitZ;
            A = Vector.UnitX;
        }

        /// <summary>
        /// Create a cylindrical coordinate system defined by an origin point and 
        /// the longitudinal axis.  The polar axis will be aligned towards the global X-axis.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="lAxis"></param>
        public CylindricalCoordinateSystem(Vector origin, Vector lAxis)
        {
            Origin = origin;
            L = lAxis;
            Vector yTemp = L.Cross(Vector.UnitX);
            A = yTemp.Cross(L);
        }

        /// <summary>
        /// Plane constructor.  Creates a cylindrical coordinate system by specifying
        /// the reference plane.  The longitudinal axis will be taken as the local
        /// z-axis of the plane, the polar as the local x and the origin set to the
        /// plane's origin.
        /// </summary>
        /// <param name="plane"></param>
        public CylindricalCoordinateSystem(CartesianCoordinateSystem plane)
        {
            Origin = plane.Origin;
            L = plane.Z;
            A = plane.X;
        }

        /// <summary>
        /// Origin, polar axis, plane vector constructor.  Creates a cylindrical coordinate
        /// system from an origin point and two vectors lying on the reference plane.
        /// </summary>
        /// <param name="origin">The origin point</param>
        /// <param name="aAxis">The polar axis vector</param>
        /// <param name="onPlane">Another vector which lies on the reference plane
        /// but that is not coincident with the polar axis vector</param>
        public CylindricalCoordinateSystem(Vector origin, Vector aAxis, Vector onPlane)
        {
            Origin = origin;
            A = aAxis;
            L = A.Cross(onPlane).Unitize();
        }

        /// <summary>
        /// Duplication constructor.
        /// </summary>
        /// <param name="other"></param>
        public CylindricalCoordinateSystem(CylindricalCoordinateSystem other)
        {
            Origin = other.Origin;
            L = other.L;
            A = other.A;
        }

        /// <summary>
        /// Duplication constructor, moving the origin of the coordinat system to a new position
        /// </summary>
        /// <param name="other"></param>
        /// <param name="origin"></param>
        public CylindricalCoordinateSystem(CylindricalCoordinateSystem other, Vector origin)
        {
            Origin = origin;
            L = other.L;
            A = other.A;
        }

        /// <summary>
        /// Constructor specifying all axes as a single vector array containing
        /// Origin, L, A in that order.
        /// Allows data to be populated via a static function in a subclass constructor.
        /// </summary>
        /// <param name="vectors"></param>
        protected CylindricalCoordinateSystem(Vector[] vectors)
        {
            Origin = vectors[0];
            L = vectors[1];
            A = vectors[2];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert a vector defined in the global coordinate system into 
        /// one defined in local coordinates of this coordinate system,
        /// where X = radius, Y = theta, Z = z
        /// </summary>
        /// <param name="vector">A cartesian vector in the global coordinate system.</param>
        /// <param name="direction">If true, this vector represents a direction rather than a point
        /// and will be transformed without reference to the origin.</param>
        /// <returns>A vector in local coordinates, where X = radius, Y = theta, Z = z</returns>
        public Vector GlobalToLocal(Vector vector, bool direction = false)
        {
            if (direction)
            {
                double z = vector.Dot(L);
                Vector onPlane = vector - L * z;
                double r = onPlane.Magnitude();
                Vector localY = L.Cross(A);
                double theta = Math.Atan2(onPlane.Dot(localY), onPlane.Dot(A));
                return new Vector(r, theta, z);
            }
            else
            { 
                Vector relative = vector - Origin;
                double z = relative.Dot(L);
                Vector onPlane = relative - L * z;
                double r = onPlane.Magnitude();
                Vector localY = L.Cross(A);
                double theta = Math.Atan2(onPlane.Dot(localY), onPlane.Dot(A));
                return new Vector(r, theta, z);
            }
        }

        /// <summary>
        /// Convert a vector defined in the local coordinate system into
        /// one defined in global coordinates
        /// </summary>
        /// <param name="vector">A vector in the local coordinate system,
        /// where X = radius and Y = theta.</param>
        /// <param name="direction">If true, this vector represents a direction rather than a point
        /// and will be transformed without reference to the origin.</param>
        /// <returns>A vector in global cartesian coordinates</returns>
        public Vector LocalToGlobal(Vector vector, bool direction = false)
        {
            if (direction)
            {
                return A.Rotate(L, vector.Y) * vector.X + L * vector.Z;
            } 
            else return Origin + A.Rotate(L, vector.Y) * vector.X + L * vector.Z;
        }

        /// <summary>
        /// Convert a set of coordinates defined in the local coordinate system
        /// into one defined in global coordinates
        /// </summary>
        /// <param name="r">The radial distance to the point on the reference plane</param>
        /// <param name="theta">The rotation from the polar axis, A</param>
        /// <param name="z">The distance from the reference plane</param>
        /// <param name="direction">If true, this vector represents a direction rather than a point
        /// and will be transformed without reference to the origin.</param>
        /// <returns>A vector representing a position in the global cartesian coordinate system.</returns>
        public Vector LocalToGlobal(double r, double theta, double z = 0, bool direction = false)
        {
            if (direction)
                return A.Rotate(L, theta) * r + L * z;
            else return Origin + A.Rotate(L, theta) * r + L * z;
        }

        /// <summary>
        /// Calculate the azimuth angle of the given point.
        /// This is the angle necessary for the polar axis to be rotated
        /// about the longitudinal one in order to be pointing in the direction
        /// of the specified point.
        /// </summary>
        /// <param name="point">The point to test</param>
        /// <returns>The azimuth angle, in degrees</returns>
        public Angle Azimuth(Vector point)
        {
            Vector relative = point - Origin;
            double z = relative.Dot(L);
            Vector onPlane = relative - L * z;
            Vector localY = L.Cross(A);
            double theta = Math.Atan2(onPlane.Dot(localY), onPlane.Dot(A));
            return theta;
        }

        #endregion


    }
}

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

using Nucleus.Base;
using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Represents an infinite plane described in terms of an origin point and a local coordinate system,
    /// where X and Y axes lie on the plane and the Z axis is normal to it.
    /// The coordinate system of the plane is right-handed.
    /// Immutable geometric primitive.
    /// </summary>
    [Serializable]
    public class Plane : 
        CartesianCoordinateSystem,
        IDuplicatable
    {
        #region Constants

        /// <summary>
        /// A preset plane representing the global XY plane, centered on the global origin.
        /// </summary>
        public static readonly Plane GlobalXY = new Plane();

        /// <summary>
        /// A preset plane representing the global YZ plane, centred on the global origin.
        /// </summary>
        public static readonly Plane GlobalYZ = new Plane(Vector.Zero, Vector.UnitY, Vector.UnitZ);

        /// <summary>
        /// A present plane representing the global XZ plane, centred on the global origin.
        /// </summary>
        public static readonly Plane GlobalXZ = new Plane(Vector.Zero, Vector.UnitX, Vector.UnitZ);

        /// <summary>
        /// A preset plane representing the global XY plane, centered on the global origin,
        /// but aligned with its X-axis lying along the global Y-axis.
        /// </summary>
        public static readonly Plane GlobalYX = new Plane(Vector.Zero, Vector.UnitY, Vector.UnitX);

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Plane is initialised as a global XY plane.
        /// </summary>
        public Plane() : base() { }

        /// <summary>
        /// Constructor creating a global XY plane with its origin at the specified point
        /// </summary>
        /// <param name="origin">The origin point of the new plane</param>
        public Plane(Vector origin) : base(origin) { }

        /// <summary>
        /// Constructor creating a plane defined by an origin point and a normal vector.
        /// The X- and Y-axes will be generated perpendicular to the normal vector and with the x-axis orientated as closely as possible to the global x-axis.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="normal">The normal vector to the plane.  Will become the plane's local Z-axis.
        /// Should be a unit vector if consistent scaling is required.</param>
        public Plane(Vector origin, Vector normal) : base(origin, normal) { }

        /// <summary>
        /// Constructor creating a plane defined by an origin and two vectors on the plane.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="xAxis">The first vector that lies on the plane.  Will form the plane local x-axis.</param>
        /// <param name="xyVector">A second vector that lies on the plane but that is not coincident with the first.</param>
        public Plane(Vector origin, Vector xAxis, Vector xyVector) : base(origin, xAxis, xyVector) { }

        /// <summary>
        /// Constructor creating a plane from the XY plane of the specified coordinate system
        /// </summary>
        /// <param name="cSystem"></param>
        public Plane(CartesianCoordinateSystem cSystem) : base(cSystem) { }

        /// <summary>
        /// Initialise a new plane aligned with the XY plane of the specified coordinate system positioned
        /// at a new origin point
        /// </summary>
        /// <param name="cSystem">The coordinate system (or plane) to copy orientation from</param>
        /// <param name="newOrigin">The origin point of the new plane</param>
        public Plane(CartesianCoordinateSystem cSystem, Vector newOrigin) : base(cSystem, newOrigin) { }

        /// <summary>
        /// Constructor explicitly specifying all axes.
        /// The data is not validated.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        protected Plane(Vector origin, Vector x, Vector y, Vector z) : base(origin, x, y, z) { }

        #endregion

        #region Methods

        /// <summary>
        /// Project the specified point onto the plane.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector Project(Vector point)
        {
            return LocalToGlobal(GlobalToLocal(point).WithZ(0));
        }

        /// <summary>
        /// Get the distance above or below the plane the specified point lies.
        /// This is returned as a signed multiple of the local z-axis.
        /// If above the plane the result will be positive, if below negative.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double DistanceTo(Vector point)
        {
            Vector relV = point - Origin;
            return relV.Dot(Z);
        }

        /// <summary>
        /// Get the adjustment ratio of an area plotted on this plane to that of
        /// its projection on the global XY plane.
        /// </summary>
        /// <returns></returns>
        public double AreaRatio()
        {
            Vector v = X + Y;
            return Math.Abs(v.X * X.Y);
        }

        /// <summary>
        /// Get the adjustment ratio of an area plotted on this plane to that of
        /// its projection on the specified plane
        /// </summary>
        /// <param name="onPlane"></param>
        /// <returns></returns>
        public double AreaRatio(Plane onPlane)
        {
            Vector v = X + Y;
            return Math.Abs(v.Dot(onPlane.X) * v.Dot(onPlane.Y));
        }

        /// <summary>
        /// Rotate this plane around its origin about the specified axis
        /// </summary>
        /// <param name="axis">The axis of rotation</param>
        /// <param name="rotation">The angle of rotation</param>
        /// <returns></returns>
        public Plane Rotate(Vector axis, Angle rotation)
        {
            return new Plane(Origin, X.Rotate(axis, rotation), Y.Rotate(axis, rotation));
        }

        /// <summary>
        /// Create a copy of this Plane rotated about its own normal.
        /// The plane itself will remain the same however the local coordinate axes will
        /// have been rotated within that plane.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Plane Rotate(Angle rotation)
        {
            return new Plane(Origin, X.Rotate(Z, rotation), Y.Rotate(Z, rotation));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Construct a plane from three points in space.
        /// The three points must be different, else this will fail and return null.
        /// </summary>
        /// <param name="origin">The point to be used as the origin of the plane</param>
        /// <param name="ptX">A point lying on the X-axis</param>
        /// <param name="ptXY">A point lying somewhere on the XY-plane, but not on the X-axis</param>
        /// <returns></returns>
        public static Plane From3Points(Vector origin, Vector ptX, Vector ptXY)
        {
            Vector xAxis = ptX - origin;
            Vector xyVector = ptXY - origin;
            if (!xAxis.IsZero() && !xyVector.IsZero())
                return new Plane(origin, ptX - origin, ptXY - origin);
            else
                return null;
        }

        /// <summary>
        /// Construct a cartesian coordinate system from X and Z axis vectors
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="xAxis"></param>
        /// <param name="zAxis"></param>
        /// <returns></returns>
        public static Plane FromXAndZ(Vector origin, Vector xAxis, Vector zAxis)
        {
            return new Plane(origin, xAxis, zAxis.Cross(xAxis), zAxis);
        }

        /// <summary>
        /// Construct a plane from an X axis, using the global Z axis
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="xAxis"></param>
        /// <returns></returns>
        public static Plane FromXAxis(Vector origin, Vector xAxis)
        {
            return FromXAndZ(origin, xAxis, Vector.UnitZ);
        }

        /// <summary>
        /// Constructor creating a plane defined by an origin point and a z-axis vector.
        /// The X- and Y-axes will be generated perpendicular to the z-axis and with the y-axis orientated
        /// as closely as possible to the global y-axis (unless the specified z-axis already lies in that axis,
        /// in which case it will be aligned as closely as possible to the global z).  Note that this is different
        /// from the standard constructor where X is dominant.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="normal">The coordinate system z-axis
        /// Should be a unit vector if consistent scaling is required.</param>
        public static Plane FromNormalYDominant(Vector origin, Vector normal)
        {
            var z = normal;
            var x = (z.IsXOnly() ? z.Cross(Vector.UnitZ) : z.Cross(-Vector.UnitY)).Unitize();
            var y = x.Cross(-z);
            return new Plane(origin, x, y, z);
        }
    }

    #endregion

}

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
    /// Represents a cartesian coordinate system
    /// </summary>
    [Serializable]
    public class CartesianCoordinateSystem
        : ICoordinateSystem, IDuplicatable
    {
        #region Constants

        /// <summary>
        /// The default, global coordinate system.
        /// </summary>
        public static CartesianCoordinateSystem Global = new CartesianCoordinateSystem();

        #endregion

        #region Fields

        /// <summary>
        /// The origin point of the coordinate system
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector Origin;

        /// <summary>
        /// The unit vector defining the local X-axis
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector X;

        /// <summary>
        /// The unit vector defining the local Y-axis
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector Y;

        /// <summary>
        /// The unit vector defining the local Z-axis
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public readonly Vector Z;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Coordinate system is initialised matching the global coordinate system.
        /// </summary>
        public CartesianCoordinateSystem()
        {
            Origin = Vector.Zero;
            X = Vector.UnitX;
            Y = Vector.UnitY;
            Z = Vector.UnitZ;
        }

        /// <summary>
        /// Constructor creating a globally-aligned coordinate system with its origin at the specified point
        /// </summary>
        /// <param name="origin">The origin point of the coordinate system</param>
        public CartesianCoordinateSystem(Vector origin)
        {
            Origin = origin;
            X = Vector.UnitX;
            Y = Vector.UnitY;
            Z = Vector.UnitZ;
        }

        /// <summary>
        /// Constructor creating a coordinate system defined by an origin point and a z-axis vector.
        /// The X- and Y-axes will be generated perpendicular to the z-axis and with the x-axis orientated
        /// as closely as possible to the global x-axis (unless the specified z-axis already lies in that axis,
        /// in which case it will be aligned as closely as possible to the global z).
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="zAxis">The coordinate system z-axis
        /// Should be a unit vector if consistent scaling is required.</param>
        public CartesianCoordinateSystem(Vector origin, Vector zAxis)
        {
            Origin = origin;
            Z = zAxis;
            Y = (Z.IsXOnly() ? Z.Cross(Vector.UnitZ) : Z.Cross(Vector.UnitX)).Unitize();
            X = Y.Cross(Z);
        }

        /// <summary>
        /// Constructor creating a coordinate system defined by an origin and two vectors on the XY plane.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="xAxis">The x-axis.</param>
        /// <param name="xyVector">A second vector that lies on the plane but that is not coincident with the first.</param>
        public CartesianCoordinateSystem(Vector origin, Vector xAxis, Vector xyVector)
        {
            Origin = origin;
            X = xAxis.Unitize();
            Z = X.Cross(xyVector).Unitize();
            Y = Z.Cross(X);
        }

        /// <summary>
        /// Constructor explicitly specifying all axes.
        /// The data is not validated.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        protected CartesianCoordinateSystem(Vector origin, Vector x, Vector y, Vector z)
        {
            Origin = origin;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Constructor specifying all axes as a single vector array containing
        /// Origin, X, Y, Z in that order.
        /// Allows data to be populated via a static function in a subclass constructor.
        /// </summary>
        /// <param name="vectors"></param>
        protected CartesianCoordinateSystem(Vector[] vectors)
        {
            Origin = vectors[0];
            X = vectors[1];
            Y = vectors[2];
            Z = vectors[3];
        }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other">Another coordinate system to copy values from</param>
        public CartesianCoordinateSystem(CartesianCoordinateSystem other)
        {
            Origin = other.Origin;
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        /// <summary>
        /// Constructor to duplicate a coordinate system while changing its origin to
        /// a new position
        /// </summary>
        /// <param name="other">Another coordinate system to copy axis directions from</param>
        /// <param name="newOrigin">The origin point of the new coordinate system</param>
        public CartesianCoordinateSystem(CartesianCoordinateSystem other, Vector newOrigin)
        {
            Origin = newOrigin;
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert a vector defined in the global coordinate system into 
        /// one defined in local coordinates of this coordinate system.
        /// </summary>
        /// <param name="vector">A vector in the global coordinate system.</param>
        /// <param name="direction"></param>
        /// <returns>A vector in local coordinates</returns>
        public Vector GlobalToLocal(Vector vector, bool direction = false)
        {
            if (direction)
            {
                return new Vector(vector.Dot(X), vector.Dot(Y), vector.Dot(Z));
            }
            else
            {
                Vector relV = vector - Origin;
                return new Vector(relV.Dot(X), relV.Dot(Y), relV.Dot(Z));
            }
        }

        /// <summary>
        /// Convert a vector defined in the local coordinate system into
        /// one defined in global coordinates
        /// </summary>
        /// <param name="vector">A vector in the local coordinate system.</param>
        /// <param name="direction">If true, this vector represents a direction rather than a point
        /// and will not be offset by the origin.</param>
        /// <returns>A vector in global coordinates</returns>
        public Vector LocalToGlobal(Vector vector, bool direction = false)
        {
            if (direction)
            {
                return X * vector.X + Y * vector.Y + Z * vector.Z;
            }
            else return Origin + X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        /// <summary>
        /// Get the axis vector relevant to the specified local axis
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector GetAxisVector(Direction direction)
        {
            if (direction == Direction.X || direction == Direction.XX) return X;
            if (direction == Direction.Y || direction == Direction.YY) return Y;
            else return Z;
        }

        /// <summary>
        /// Convert a set of coordinates defined in the local coordinate system
        /// into one defined in global coordinates
        /// </summary>
        /// <param name="x">The first coordinate.</param>
        /// <param name="y">The second coordinate.</param>
        /// <param name="z">The third coordinate.</param>
        /// <returns>A vector representing a position in the global cartesian coordinate system.</returns>
        public Vector LocalToGlobal(double x, double y, double z = 0, bool direction = false)
        {
            if (direction) return X * x + Y * y + Z * z;
            else return Origin + X * x + Y * y + Z * z;
        }

        /// <summary>
        /// Convert a set of points defined in the local coordinate system
        /// to a set of points defined in the global one
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public Vector[] LocalToGlobal(IList<Vector> vectors)
        {
            Vector[] result = new Vector[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
            {
                result[i] = LocalToGlobal(vectors[i]);
            }
            return result;
        }

        /// <summary>
        /// Extract the XY plane from this coordinate system
        /// </summary>
        /// <returns></returns>
        public Plane XYPlane()
        {
            return new Plane(Origin, X, Y);
        }

        /// <summary>
        /// Extract the YZ plane from this coordinate system
        /// </summary>
        /// <returns></returns>
        public Plane YZPlane()
        {
            return new Plane(Origin, Y, Z);
        }

        /// <summary>
        /// Extract the XZ plane form this coordinate system
        /// </summary>
        /// <returns></returns>
        public Plane XZPlane()
        {
            return new Plane(Origin, X, Z);
        }

        /// <summary>
        /// Invert the directions of the X and Y axes, keeping Z the same
        /// </summary>
        /// <returns></returns>
        public CartesianCoordinateSystem ReverseXY()
        {
            return new CartesianCoordinateSystem(Origin, -X, -Y, Z);
        }

        public override string ToString()
        {
            return Origin.ToString() + "|" + X.ToString() + "|" + Y.ToString() + "|" + Z.ToString();
        }

        #endregion
    }
}

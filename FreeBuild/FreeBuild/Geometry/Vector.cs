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

using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// 3D Vector structure.
    /// Represents a position or direction in 3D space through X, Y and Z components.
    /// Immutable.
    /// </summary>
    /// <remarks>
    /// This type is currently implemented as a structure instead of a class for
    /// reasons of efficiency.  This may be reviewed once benchmarking has been performed, however.
    /// As the size of this struct exceeds the recommended 16 bytes, it may work out that
    /// passing vectors around in function arguments etc. is inefficient.  
    /// </remarks>
    [Serializable]
    [DebuggerDisplay("( {X} , {Y} , {Z}) ")]
    public struct Vector :
        IEquatable<Vector>
    {
        #region Constants

        /// <summary>
        /// A constant value representing an unset, invalid vector.
        /// All components are set to NaN.
        /// </summary>
        public static readonly Vector Unset = new Vector(false);

        /// <summary>
        /// Constant value representing a zero-length 3d vector
        /// </summary>
        public static readonly Vector Zero = new Vector(0, 0, 0);

        /// <summary>
        /// Constant value representing a unit vector along the global X-axis
        /// </summary>
        public static readonly Vector UnitX = new Vector(1, 0, 0);

        /// <summary>
        /// Constant value representing a unit vector along the global Y-axis
        /// </summary>
        public static readonly Vector UnitY = new Vector(0, 1, 0);

        /// <summary>
        /// Constant value representing a unit vector along the global Z-axis
        /// </summary>
        public static readonly Vector UnitZ = new Vector(0, 0, 1);

        #endregion

        #region Fields

        /// <summary>
        /// The vector's X component
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The vector's Y component
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// The vector's Z component
        /// </summary>
        public readonly double Z;

        #endregion

        #region Properties

        /// <summary>
        /// Get an indexed component of this vector.
        /// 0 = X, 1 = Y, 2 = Z
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get
            {
                if (index == 0) return X;
                else if (index == 1) return Y;
                else if (index == 2) return Z;
                else throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Get the specified dimension of this vector.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public double this[Dimension dimension]
        {
            get
            {
                if (dimension == Dimension.X) return X;
                else if (dimension == Dimension.Y) return Y;
                else if (dimension == Dimension.Z) return Z;
                else return 0;
            }
        }

        /// <summary>
        /// Get the angle of this vector on the XY plane.
        /// </summary>
        public Angle Angle
        {
            get { return Math.Atan2(Y, X); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// X, Y coordinate constructor - vector will be initialised to (x,y,0)
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        /// <summary>
        /// X, Y, Z coordinate constructor - vector will be initialised to (x,y,z)
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Angle constructor.
        /// Create a new vector on the XY plane pointing in the specified
        /// angle anticlockwise from the global X-axis.
        /// </summary>
        /// <param name="angle">The angle, clockwise from the global X axis.  In radians.</param>
        public Vector(Angle angle)
        {
            X = Math.Cos(angle);
            Y = Math.Sin(angle);
            Z = 0;
        }

        /// <summary>
        /// Validity constructor.
        /// If the input boolean is false then the vector will be created as an invalid vector,
        /// identical to the Vector.Unset constant.  If true, will be initialised as 0,0,0
        /// </summary>
        /// <param name="valid">Create a valid vector?</param>
        public Vector(bool valid)
        {
            if (!valid)
            {
                X = double.NaN;
                Y = double.NaN;
                Z = double.NaN;
            }
            else
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
        }

        /// <summary>
        /// String constructor.
        /// Creates a new vector by attempting to parse a string in the format
        /// {X} [separator] {Y} [separator] {Z}.
        /// </summary>
        /// <param name="description">The string to be parsed</param>
        /// <param name="separator">The character used to denote the separations between coordinates.
        /// By default this is taken as being a comma.</param>
        /// <param name="scalingFactor">An optional scaling factor which can be used to
        /// convert from non-SI units.  Each component of the vector will be multiplied by
        /// this value.</param>
        public Vector(string description, char separator=',', double scalingFactor = 1.0)
        {
            var tokens = description.Split(separator);
            if (tokens.Count() > 0) X = double.Parse(tokens[0]) * scalingFactor;
            else X = 0;
            if (tokens.Count() > 1) Y = double.Parse(tokens[1]) * scalingFactor;
            else Y = 0;
            if (tokens.Count() > 2) Z = double.Parse(tokens[2]) * scalingFactor;
            else Z = 0;
        }

        /// <summary>
        /// Create a unit vector pointing in the specified direction
        /// (or in the direction of the axis of rotation)
        /// </summary>
        /// <param name="direction"></param>
        public Vector(Direction direction)
        {
            X = 0;
            Y = 0;
            Z = 0;
            if (direction == Direction.X || direction == Direction.XX) X = 1;
            if (direction == Direction.YY || direction == Direction.YY) Y = 1;
            else Z = 1; 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is this vector valid?
        /// A vector is valid if all of its components are not NaN.
        /// </summary>
        /// <returns>True if all components are valid, else false.</returns>
        public bool IsValid()
        {
            return !double.IsNaN(X) && !double.IsNaN(Y) && !double.IsNaN(Z);
        }

        /// <summary>
        /// IEquatable implementation.
        /// Checks whether this vector is equal to another.
        /// </summary>
        /// <param name="other">Another vector to check</param>
        /// <returns>True if the components of the two vectors are equal, else false.</returns>
        public bool Equals(Vector other)
        {
            return (X == other.X && Y == other.Y && Z == other.Z);
        }

        /// <summary>
        /// Checks whether this vector is approximately equal to another, within a tolerance value
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns>True if the components of the two vectors are all within tolerance of
        /// one another.  Else false.</returns>
        public bool Equals(Vector other, double tolerance)
        {
            return (X > other.X - tolerance && X < other.X + tolerance
                && Y > other.Y - tolerance && Y < other.Y + tolerance
                && Z > other.Z - tolerance && Z < other.Z + tolerance);
        }

        /// <summary>
        /// Equals override for generic objects.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector) return Equals((Vector)obj);
            else return false;
        }

        /// <summary>
        /// GetHashCode override.
        /// Generates a hash code by XORing those of its components.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Get the squared magnitude of this vector.
        /// Faster than using Magnitude() as it's calculation does not include a (slow) square-root operation.
        /// </summary>
        /// <returns></returns>
        public double MagnitudeSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Get the current magnitude of this vector.
        /// Use of MagnitudeSquared instead is advised in performance-criticial situations 
        /// where the exact value is not necessary as it does not require a square-root operation.
        /// </summary>
        /// <returns></returns>
        public double Magnitude()
        {
            return Math.Sqrt(MagnitudeSquared());
        }

        /// <summary>
        /// Calculate the squared distance from this position vector to another.
        /// </summary>
        /// <param name="position">Another position vector</param>
        /// <returns>The squared distance from this point to another, as a double.</returns>
        public double DistanceToSquared(Vector position)
        {
            double dX = position.X - X;
            double dY = position.Y - Y;
            double dZ = position.Z - Z;
            return dX * dX + dY * dY + dZ * dZ;
        }

        /// <summary>
        /// Calculate the distance from this position vector to another.
        /// Use of DistanceToSquared instead is advised in performance-critical situations
        /// where the exact value is not necessary as it does not require a square-root operation.
        /// </summary>
        /// <param name="position">Another position vector</param>
        /// <returns>The distance from this point to another, as a double.</returns>
        public double DistanceTo(Vector position)
        {
            double dX = position.X - X;
            double dY = position.Y - Y;
            double dZ = position.Z - Z;
            return Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        /// <summary>
        /// Calculate the squared distance from this position vector to another
        /// on the XY plane.
        /// </summary>
        /// <param name="position">Another position vector.</param>
        /// <returns>The squared distance on the XY plane from this point to another,
        /// as a double.</returns>
        public double XYDistanceToSquared(Vector position)
        {
            double dX = position.X - X;
            double dY = position.Y - Y;
            return dX * dX + dY * dY;
        }

        /// <summary>
        /// Calculate the distance from this position vector to another
        /// on the XY plane.
        /// </summary>
        /// <param name="position">Another position vector.</param>
        /// <returns>The distance on the XY plane from this point to another,
        /// as a double.</returns>
        public double XYDistanceTo(Vector position)
        {
            double dX = position.X - X;
            double dY = position.Y - Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        /// <summary>
        /// Find the angle on the XY plane from this position vector to another
        /// </summary>
        /// <param name="other">The position vector to measure the angle to</param>
        /// <returns></returns>
        public Angle AngleTo(Vector other)
        {
            return Math.Atan2(other.Y - Y, other.X - X);
        }

        /// <summary>
        /// Are all components of this vector equal to zero?
        /// </summary>
        /// <returns>True if all components are zero, else false</returns>
        public bool IsZero()
        {
            if (X == 0 && Y == 0 && Z == 0) return true;
            else return false;
        }

        /// <summary>
        /// Interpolate between this vector and another
        /// </summary>
        /// <param name="towards">The vector to interpolate towards</param>
        /// <param name="factor">The interpolation factor.  0 = this vector, 1 = the 'towards' vector</param>
        /// <returns>An interpolated 3D vector</returns>
        public Vector Interpolate(Vector towards, double factor)
        {
            return new Vector(
                X + (towards.X - X) * factor,
                Y + (towards.Y - Y) * factor,
                Z + (towards.Z - Z) * factor);
        }

        /// <summary>
        /// Interpolate between this vector and another corresponding to
        /// a set of key values
        /// </summary>
        /// <param name="v1">The vector to interpolate towards</param>
        /// <param name="x0">The key value mapped to this vector</param>
        /// <param name="x1">The key value mapped to the other vector</param>
        /// <param name="x">The value of the vector at the position to be interpolated</param>
        /// <returns></returns>
        public Vector Interpolate(Vector v1, double x0, double x1, double x)
        {
            return Interpolate(v1, (x - x0) / (x1 - x0));
        }

        /// <summary>
        /// Calculate the cross, or vector, product of this and another vector.
        /// Creates a new vector perpendicular to both input vectors.
        /// </summary>
        /// <param name="other">The vector to cross with this one.</param>
        /// <returns>The cross-product vector</returns>
        public Vector Cross(Vector other)
        {
            return new Vector(
                Y * other.Z - Z * other.Y,
                Z * other.X - X * other.Z,
                X * other.Y - Y * other.X);
        }

        /// <summary>
        /// Calculate the cross, or vector, product of this and another vector.
        /// Creates a new vector perpendicular to both input vectors.
        /// </summary>
        /// <param name="other">The vector to cross with this one.</param>
        /// <returns>The cross-product vector</returns>
        public Vector Cross(ref Vector other)
        {
            return new Vector(
                Y * other.Z - Z * other.Y,
                Z * other.X - X * other.Z,
                X * other.Y - Y * other.X);
        }

        /// <summary>
        /// Calculate the dot, or scalar, product of this and another vector.
        /// Provides the component of this vector in the direction of (or, the projection onto) the other vector.
        /// </summary>
        /// <param name="other">The vector to project this vector onto.  
        /// If the length of the projection is required, this should be a unit vector.</param>
        /// <returns>The dot product as a double.</returns>
        public double Dot(Vector other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        /// <summary>
        /// Calculate the dot, or scalar, product of this and another vector.
        /// Provides the component of this vector in the direction of (or, the projection onto) the other vector.
        /// This version recieves the other by reference to avoid having to copy the full data of the struct.
        /// </summary>
        /// <param name="other">The vector to project this vector onto.  
        /// If the length of the projection is required, this should be a unit vector.</param>
        /// <returns>The dot product as a double.</returns>
        public double Dot(ref Vector other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        /// <summary>
        /// Create a unitized version of this vector
        /// (i.e. a vector in the same direction but with a magnitude of 1)
        /// </summary>
        /// <returns></returns>
        public Vector Unitize()
        {
            double magnitude = MagnitudeSquared();
            if (magnitude == 1) return this;
            magnitude = Math.Sqrt(magnitude);
            return new Vector(X / magnitude, Y / magnitude, Z / magnitude);
        }

        /// <summary>
        /// Get the inverse of this vector
        /// </summary>
        /// <returns></returns>
        public Vector Reverse()
        {
            return new Vector(-X, -Y, -Z);
        }

        /// <summary>
        /// Scale this vector by a factor
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns>A new vector created by scaling all components of this one.</returns>
        public Vector Scale(double scalar)
        {
            return new Vector(X * scalar, Y * scalar, Z * scalar);
        }

        /// <summary>
        /// Add another vector to this one and return the result
        /// </summary>
        /// <param name="other">Another vector to be added to this one.</param>
        /// <returns>The combined vector</returns>
        public Vector Add(Vector other)
        {
            return new Vector(X + other.X, Y + other.Y, Z + other.Z);
        }

        /// <summary>
        /// Add another vector to this one and return the result.
        /// Pass-by-reference version.
        /// </summary>
        /// <param name="other">Another vector to be added to this one.</param>
        /// <returns>The combined vector</returns>
        public Vector Add(ref Vector other)
        {
            return new Vector(X + other.X, Y + other.Y, Z + other.Z);
        }

        /// <summary>
        /// Add another vector defined as X and Y coordinates to this one
        /// and return the resultant vector.
        /// </summary>
        /// <param name="x">The value to add to the x-component</param>
        /// <param name="y">The value to add to the y-component</param>
        /// <returns>The combined vector</returns>
        public Vector Add(double x, double y)
        {
            return new Vector(X + x, Y + y, Z);
        }

        /// <summary>
        /// Add another vector defined as X, Y and Z coordinates to this one
        /// and return the resultant vector.
        /// </summary>
        /// <param name="x">The value to add to the x-component</param>
        /// <param name="y">The value to add to the y-component</param>
        /// <param name="z">The value to add to the z-component</param>
        /// <returns>The combined vector</returns>
        public Vector Add(double x, double y, double z)
        {
            return new Vector(X + x, Y + y, Z + z);
        }

        /// <summary>
        /// Calculate the (smallest, non-directional) angle between this vector and another.
        /// In the range -PI/2 to PI/2.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The smallest angle between this vector and another,
        /// in Radians.</returns>
        public Angle AngleBetween(Vector other)
        {
            Vector v1 = this.Unitize();
            Vector v2 = other.Unitize();
            double dot = v1.Dot(ref v2);
            return Math.Acos(dot);
        }


        /// <summary>
        /// Rotate this vector by an angle around an axis,
        /// following the right-hand rule.
        /// </summary>
        /// <param name="axis">A unit vector representing an axis of rotation.</param>
        /// <param name="angle">The angle of rotation, in radians.
        /// Counter-clockwise around the axis.</param>
        /// <returns>A new vector created by rotating this vector anticlockwise about the given axis by the given angle.</returns>
        /// <remarks>Uses the Rodrigues Rotation Formula - see: 
        /// https://en.wikipedia.org/wiki/Rodrigues%27_rotation_formula </remarks>
        public Vector Rotate(Vector axis, Angle angle)
        {
            //Rotation formula:
            //v_rot = v*cos(theta) + (k x v)sin(theta) + k(k*v)(1-cos(theta)
            return Scale(Math.Cos(angle)).Add(axis.Cross(ref this).Scale(Math.Sin(angle)).Add(
                axis.Scale(axis.Dot(ref this)).Scale(1 - Math.Cos(angle))));
        }

        /// <summary>
        /// Apply the specified transformation to this vector and return the result.
        /// The vector is treated as a column vector for the purposes of this calculation.
        /// </summary>
        /// <param name="transform">THe transformation matrix.</param>
        /// <returns>A new vector representing the result of the transformation on this one.</returns>
        public Vector Transform(Transform transform)
        {
            return new Vector(
                transform[0, 0] * X + transform[0, 1] * Y + transform[0, 2] * Z + transform[0, 3],
                transform[1, 0] * X + transform[1, 1] * Y + transform[1, 2] * Z + transform[1, 3],
                transform[2, 0] * X + transform[2, 1] * Y + transform[2, 2] * Z + transform[2, 3]);
        }

        /// <summary>
        /// Project this point in space onto a plane
        /// </summary>
        /// <param name="ontoPlane">The plane to project the point onto.</param>
        /// <returns></returns>
        public Vector Project(Plane ontoPlane)
        {
            return ontoPlane.LocalToGlobal(ontoPlane.GlobalToLocal(this).WithZ(0));
        }

        /// <summary>
        /// Create a new copy of this vector, but with the specified X coordinate
        /// </summary>
        /// <param name="x">The new value of the X coordinate in the copied vector</param>
        /// <returns>A new vector, copying the components from this one but replacing X</returns>
        public Vector WithX(double x)
        {
            return new Vector(x, Y, Z);
        }

        /// <summary>
        /// Create a new copy of this vector, but with the specified Y coordinate
        /// </summary>
        /// <param name="y">The new value of the Y coordinate in the copied vector</param>
        /// <returns>A new vector, copying the components from this one but replacing Y</returns>
        public Vector WithY(double y)
        {
            return new Vector(X, y, Z);
        }

        /// <summary>
        /// Create a new copy of this vector, but with the specified Z coordinate
        /// </summary>
        /// <param name="z">The new value of the Z coordinate in the copied vector</param>
        /// <returns>A new vector, copying the components from this one but replacing Z</returns>
        public Vector WithZ(double z)
        {
            return new Vector(X, Y, z);
        }

        /// <summary>
        /// Add a value to the X component of this vector and return the result
        /// </summary>
        /// <param name="value">The value to be added to the X component</param>
        /// <returns></returns>
        public Vector AddX(double value)
        {
            return new Vector(X + value, Y, Z);
        }

        /// <summary>
        /// Add a value to the Y component of this vector and return the result
        /// </summary>
        /// <param name="value">The value to be added to the Y component</param>
        /// <returns></returns>
        public Vector AddY(double value)
        {
            return new Vector(X, Y + value, Z);
        }

        /// <summary>
        /// Add a value to the Z component of this vector and return the result
        /// </summary>
        /// <param name="value">The value to be added to the Z component</param>
        /// <returns></returns>
        public Vector AddZ(double value)
        {
            return new Vector(X, Y, Z + value);
        }

        /// <summary>
        /// Create a copy of this vector flattened to the XY plane
        /// </summary>
        /// <returns>A new vector with the same X and Y components as this one,
        /// but with Z set to 0</returns>
        public Vector XY()
        {
            return new Vector(X, Y, 0);
        }

        /// <summary>
        /// Does this vector have a non-zero component in the X-axis only?
        /// </summary>
        /// <returns>True if only the X-component is non-zero</returns>
        public bool IsXOnly()
        {
            return (X != 0 && Y == 0 && Z == 0);
        }

        /// <summary>
        /// Does this vector have a non-zero component in the Y-axis only?
        /// </summary>
        /// <returns>True if only the Y-component is non-zero</returns>
        public bool IsYOnly()
        {
            return (X == 0 && Y != 0 && Z == 0);
        }

        /// <summary>
        /// Does this vector have a non-zero component in the Z-axis only?
        /// </summary>
        /// <returns>True if only the Y-component is non-zero</returns>
        public bool IsZOnly()
        {
            return (X == 0 && Y == 0 && Z != 0);
        }

        /// <summary>
        /// Is this vector parallel to another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsParallelTo(Vector other)
        {
            return (Y * other.Z - Z * other.Y == 0 &&
                Z * other.X - X * other.Z == 0 &&
                X * other.Y - Y * other.X == 0);
            //double checkVal = X / other.X;
            //return (checkVal == Y / other.Y && checkVal == Z / other.Z);
        }

        /// <summary>
        /// Test whether this position lies in a particular direction from a point.
        /// This test will return true for angle differences of below 90 degrees (non-inclusive)
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="fromPoint"></param>
        /// <returns></returns>
        public bool IsInDirection(Vector direction, Vector fromPoint)
        {
            return (this - fromPoint).Dot(direction) > 0;
        }

        /// <summary>
        /// Flip (or not) this vector to face in the same overall direction as another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector AlignTo(Vector other)
        {
            if (Dot(other) >= 0) return this;
            else return Reverse(); 
        }

        /// <summary>
        /// Create a copy of this vector with its components rounded to the nearest whole
        /// increment
        /// </summary>
        /// <param name="increment"></param>
        /// <returns></returns>
        public Vector RoundOff(double increment)
        {
            return new Vector(X.Round(increment), Y.Round(increment), Z.Round(increment));
        }

        /// <summary>
        /// Produce a new Vector using remapped components of this one.
        /// </summary>
        /// <param name="newX">The source dimension to use for the x component of the new vector</param>
        /// <param name="newY">The source dimension to use for the y component of the new vector</param>
        /// <param name="newZ">The source dimension to use for the z component of the new vector</param>
        /// <returns></returns>
        public Vector Remap(Dimension newX, Dimension newY, Dimension newZ)
        {
            return new Vector(this[newX], this[newY], this[newZ]);
        }

        /// <summary>
        /// Produce a new Vector using the Z,X,Y components of this one mapped
        /// respectively onto X,Y,Z of the new one.
        /// Useful to convert points drawn on the XY plane to ones drawn on the YZ plane.
        /// </summary>
        /// <returns></returns>
        public Vector RemapZXY()
        {
            return new Vector(Z,X,Y);
        }

        /// <summary>
        /// Produce a new Vector using the Z,-X,Y components of this one mapped
        /// respectively onto X,Y,Z of the new one.
        /// Useful to convert points drawn on the XY plane to ones drawn on the YZ plane,
        /// where the Y axis is flipped.
        /// </summary>
        /// <returns></returns>
        public Vector RemapZnegXY()
        {
            return new Vector(Z, -X, Y);
        }

        /// <summary>
        /// Get a vector perpendicular to this vector on the XY plane
        /// </summary>
        /// <returns></returns>
        public Vector PerpendicularXY()
        {
            return new Vector(Y, -X);
        }

        /// <summary>
        /// Get the absolute magnitude of the largest component coordinate of this vector.
        /// </summary>
        /// <returns></returns>
        public double LargestComponent()
        {
            return Math.Max(X.Abs(), Math.Max(Y.Abs(), Z.Abs()));
        }

        /// <summary>
        /// Get a Vector containing only the X component of this vector.
        /// </summary>
        /// <returns></returns>
        public Vector XComponent()
        {
            return new Vector(X, 0, 0);
        }

        /// <summary>
        /// Get a Vector containing only the Y component of this vector.
        /// </summary>
        /// <returns></returns>
        public Vector YComponent()
        {
            return new Vector(0, Y, 0);
        }

        /// <summary>
        /// Get a Vector containing only the Z component of this vector.
        /// </summary>
        /// <returns></returns>
        public Vector ZComponent()
        {
            return new Vector(0, 0, Z);
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0:0.####},{1:0.####},{2:0.####})", X, Y, Z);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Find the unit vector perpendicular to the plane defined by the specified three
        /// points.
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static Vector PerpendicularTo(Vector pt0, Vector pt1, Vector pt2)
        {
            //TODO: Add error checking?
            Vector axis1 = pt1 - pt0;
            Vector axis2 = pt2 - pt0;
            return axis1.Cross(axis2).Unitize();
        }

        /// <summary>
        /// Find the area of the 3D triangle denoted by the specified three vectors
        /// </summary>
        /// <param name="pt0">The first vertex point</param>
        /// <param name="pt1">The second vertex point</param>
        /// <param name="pt2">The third vertex point</param>
        /// <returns>Triangle area as double</returns>
        public static double TriangleArea(Vector pt0, Vector pt1, Vector pt2)
        {
            double d01 = pt0.DistanceTo(pt1);
            double d12 = pt1.DistanceTo(pt2);
            double d20 = pt2.DistanceTo(pt0);
            double s = (d01 + d12 + d20) / 2;
            return Math.Sqrt(s * (s - d01) * (s - d12) * (s - d20));
        }

        /// <summary>
        /// Find the sqhared area of the 3D triangle denoted by the specified three vectors
        /// </summary>
        /// <param name="pt0">The first vertex point</param>
        /// <param name="pt1">The second vertex point</param>
        /// <param name="pt2">The third vertex point</param>
        /// <returns>Squared triangle area as double</returns>
        public static double TriangleAreaSquared(Vector pt0, Vector pt1, Vector pt2)
        {
            double d01 = pt0.DistanceTo(pt1);
            double d12 = pt1.DistanceTo(pt2);
            double d20 = pt2.DistanceTo(pt0);
            double s = (d01 + d12 + d20) / 2;
            return (s * (s - d01) * (s - d12) * (s - d20));
        }



        #endregion

        #region Operators

        /// <summary>
        /// == operator override.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Vector v1, Vector v2)
            => v1.Equals(v2);

        /// <summary>
        /// != operator override
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Vector v1, Vector v2)
            => !v1.Equals(v2);

        /// <summary>
        /// > operator overrides.
        /// Compares the magnitudes of the two vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator > (Vector v1, Vector v2)
            => v1.MagnitudeSquared() > v2.MagnitudeSquared();

        /// <summary>
        /// Operator overrides.
        /// Compares the magnitudes of the two vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <(Vector v1, Vector v2)
            => v1.MagnitudeSquared() < v2.MagnitudeSquared();

        /// <summary>
        /// >= operator overrides.
        /// Compares the magnitudes of the two vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >=(Vector v1, Vector v2)
            => v1.MagnitudeSquared() >= v2.MagnitudeSquared();

        /// <summary>
        /// Operator overrides.
        /// Compares the magnitudes of the two vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <=(Vector v1, Vector v2)
            => v1.MagnitudeSquared() <= v2.MagnitudeSquared();

        /// <summary>
        /// + operator override.  Performs vector addition on two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector operator +(Vector v1, Vector v2)
            => new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        /// <summary>
        /// - operator override.  Performs vector addition on two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector operator -(Vector v1, Vector v2)
            => new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        /// <summary>
        /// The inverse vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector operator -(Vector v)
            => new Vector(-v.X, -v.Y, -v.Z);

        /// <summary>
        /// * operator override.  Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector operator *(Vector v, double s)
            => new Vector(v.X * s, v.Y * s, v.Z * s);

        /// <summary>
        /// * operator override.  Calculates the dot product of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector v1, Vector v2)
            => v1.Dot(ref v2);

        /// <summary>
        /// / operator override.  Divides a vector by a divisor.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector operator /(Vector v, double d)
            => new Vector(v.X / d, v.Y / d, v.Z / d);

        /// <summary>
        /// % operator override.  Perform
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector operator %(Vector v, double d)
            => new Vector(v.X % d, v.Y % d, v.Z % d);

        
        #endregion
    }

    /// <summary>
    /// Extension methods related to Vectors
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Remap all vectors in this array from the XY to the YZ plane
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static Vector[] RemapZXY(this Vector[] vectors)
        {
            Vector[] result = new Vector[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                result[i] = vectors[i].RemapZXY();
            }
            return result;
        }

        /// <summary>
        /// Remap all vectors in this array from the XY to the YZ plane,
        /// with the Y axis flipped
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static Vector[] RemapZnegXY(this Vector[] vectors)
        {
            Vector[] result = new Vector[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                result[i] = vectors[i].RemapZnegXY();
            }
            return result;
        }
    }
}


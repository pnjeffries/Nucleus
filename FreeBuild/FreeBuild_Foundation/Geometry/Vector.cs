using System;
using System.Collections.Generic;
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
    /// </remarks>
    [Serializable]
    public struct Vector :
        IEquatable<Vector>
    {
        #region Constants

        /// <summary>
        /// A constant value representing an unset, invalid vector.
        /// All components are set to NaN.
        /// </summary>
        public static readonly Vector Unset = new Vector(double.NaN, double.NaN, double.NaN);

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
        /// Create a new vector on the XY plane pointing in the specified
        /// angle anticlockwise from the global X-axis.
        /// </summary>
        /// <param name="angle">The angle, clockwise from the global X axis.  In radians.</param>
        public Vector(double angle)
        {
            X = Math.Cos(angle);
            Y = Math.Sin(angle);
            Z = 0;
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
            return X != double.NaN && Y != double.NaN && Z != double.NaN;
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
            return Math.Sqrt(DistanceToSquared(position));
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
        /// Calculate the (smallest, non-directional) angle between this vector and another.
        /// In the range 0 - PI/2.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The smallest angle between this vector and another,
        /// in Radians.</returns>
        public double AngleBetween(Vector other)
        {
            Vector v1 = this.Unitize();
            Vector v2 = other.Unitize();
            double dot = v1.Dot(ref v1);
            return Math.Atan(dot);
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
        public Vector Rotate(Vector axis, double angle)
        {
            //Rotation formula:
            //v_rot = v*cos(theta) + (k x v)sin(theta) + k(k*v)(1-cos(theta)
            return Scale(Math.Cos(angle)).Add(axis.Cross(ref this).Scale(Math.Sin(angle)).Add(
                axis.Scale(axis.Dot(ref this)).Scale(1 - Math.Cos(angle))));
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
}

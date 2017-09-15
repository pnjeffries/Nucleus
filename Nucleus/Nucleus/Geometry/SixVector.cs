using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A vector in six dimensions.
    /// Represents a quantity in 6D space through X,Y,Z,XX,YY,ZZ components,
    /// where X,Y,Z are typically translational and XX, YY, ZZ are rotational
    /// about the respective axes.
    /// Primarily used to store support stiffnesses
    /// </summary>
    [Serializable]
    public class SixVector
    {
        #region Properties

        private double _X;

        /// <summary>
        /// The vector's X component
        /// </summary>
        public double X { get { return _X; } }

        private double _Y;

        /// <summary>
        /// The vector's Y component
        /// </summary>
        public double Y { get { return _Y; } }

        private double _Z;

        /// <summary>
        /// The vector's Z component
        /// </summary>
        public double Z { get { return _Z; } }

        private double _XX;

        /// <summary>
        /// The vector's XX component
        /// </summary>
        public double XX { get { return _XX; } }

        private double _YY;

        /// <summary>
        /// The vector's YY component
        /// </summary>
        public double YY { get { return _YY; } }

        private double _ZZ;

        /// <summary>
        /// The vector's ZZ component
        /// </summary>
        public double ZZ { get { return _ZZ; } }

        /// <summary>
        /// Get an indexed component of this six-vector.
        /// 0 = X, 1 = Y, 2 = Z, 3 = XX, 4 = YY, 5 = ZZ
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
                else if (index == 3) return XX;
                else if (index == 4) return YY;
                else if (index == 5) return ZZ;
                else throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Get the component of this six-vector in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public double this[Direction direction]
        {
            get
            {
                if (direction == Direction.X) return X;
                else if (direction == Direction.Y) return Y;
                else if (direction == Direction.Z) return Z;
                else if (direction == Direction.XX) return XX;
                else if (direction == Direction.YY) return YY;
                else if (direction == Direction.ZZ) return ZZ;
                else return 0;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new SixVector with all components set to 0
        /// </summary>
        public SixVector() { }

        /// <summary>
        /// Initialise a new SixVector with the specified X, and Y
        /// components (and all others set to 0)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public SixVector(double x, double y)
        {
            _X = x;
            _Y = y;
        }

        /// <summary>
        /// Initialise a new SixVector with the specified X, Y and Z
        /// components (and all others set to 0)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public SixVector(double x, double y, double z)
        {
            _X = x;
            _Y = y;
            _Z = z;
        }

        /// <summary>
        /// Initialise a new SixVector from a standard 3-dimensional vector
        /// </summary>
        /// <param name="vector"></param>
        public SixVector(Vector vector) : this(vector.X, vector.Y, vector.Z) { }

        /// <summary>
        /// Initialise a new SixVector with the specified coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        public SixVector(double x, double y, double z, double xx, double yy, double zz) : this(x,y,z)
        {
            _XX = xx;
            _YY = yy;
            _ZZ = zz;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is this six-vector valid?
        /// A vector is valid if all of its components are not NaN.
        /// </summary>
        /// <returns>True if all components are valid, else false.</returns>
        public bool IsValid()
        {
            return !double.IsNaN(X) && !double.IsNaN(Y) && !double.IsNaN(Z)
                && !double.IsNaN(XX) && !double.IsNaN(YY) && !double.IsNaN(ZZ);
        }

        /// <summary>
        /// IEquatable implementation.
        /// Checks whether this six-vector is equal to another.
        /// </summary>
        /// <param name="other">Another vector to check</param>
        /// <returns>True if the components of the two vectors are equal, else false.</returns>
        public bool Equals(SixVector other)
        {
            return (X == other.X && Y == other.Y && Z == other.Z
                && XX == other.XX && YY == other.YY && ZZ == other.ZZ);
        }

        /// <summary>
        /// Merge this six-vector with another.
        /// The highest value of each component will be taken
        /// for the merged vector.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public SixVector MergeMax(SixVector other)
        {
            return new SixVector(
                Math.Max(_X, other._X),
                Math.Max(_Y, other._Y),
                Math.Max(_Z, other._Z),
                Math.Max(_XX, other._XX),
                Math.Max(_YY, other._YY),
                Math.Max(_ZZ, other._ZZ));
        }

        /// <summary>
        /// Are all components of this six-vector equal to zero?
        /// </summary>
        /// <returns>True if all components are zero, else false</returns>
        public bool IsZero()
        {
            if (X == 0 && Y == 0 && Z == 0 &&
                XX == 0 && YY == 0 && ZZ == 0) return true;
            else return false;
        }


        /// <summary>
        /// Create a new SixVector copying all values from this one
        /// bar the new specified value of X.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public SixVector WithX(double x)
        {
            return new SixVector(x, Y, Z, XX, YY, ZZ);
        }

        /// <summary>
        /// Create a new SixVector copying all values from this one
        /// bar the new specified value of Y.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public SixVector WithY(double y)
        {
            return new SixVector(X, y, Z, XX, YY, ZZ);
        }

        /// <summary>
        /// Create a new SixVector copying all values from this one
        /// bar the new specified value of Z.
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public SixVector WithZ(double z)
        {
            return new SixVector(X, Y, z, XX, YY, ZZ);
        }

        /// <summary>
        /// Create a new SixVector copying all values from this one
        /// bar the new specified value of XX.
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        public SixVector WithXX(double xx)
        {
            return new SixVector(X, Y, Z, xx, YY, ZZ);
        }

        /// <summary>
        /// Create a new SixVector copying all values from this one
        /// bar the new specified value of YY.
        /// </summary>
        /// <param name="yy"></param>
        /// <returns></returns>
        public SixVector WithYY(double yy)
        {
            return new SixVector(X, Y, Z, XX, yy, ZZ);
        }

        /// <summary>
        /// Create a new SixVector copying all values from this one
        /// bar the new specified value of ZZ.
        /// </summary>
        /// <param name="zz"></param>
        /// <returns></returns>
        public SixVector WithZZ(double zz)
        {
            return new SixVector(X, Y, Z, XX, YY, zz);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create a vector from a sequential set of values in a list,
        /// starting at the specified index.  Out-of-bounds errors are automatically checked for.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static SixVector FromTokensList(IList<string> tokens, int startIndex = 0)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            double xx = 0;
            double yy = 0;
            double zz = 0;
            if (tokens.Count() > startIndex) x = double.Parse(tokens[startIndex]);
            if (tokens.Count() > startIndex + 1) y = double.Parse(tokens[startIndex + 1]);
            if (tokens.Count() > startIndex + 2) z = double.Parse(tokens[startIndex + 2]);
            if (tokens.Count() > startIndex + 3) xx = double.Parse(tokens[startIndex + 3]);
            if (tokens.Count() > startIndex + 4) yy = double.Parse(tokens[startIndex + 4]);
            if (tokens.Count() > startIndex + 5) zz = double.Parse(tokens[startIndex + 5]);
            return new SixVector(x, y, z, xx, yy, zz);
        }

        #endregion

    }
}

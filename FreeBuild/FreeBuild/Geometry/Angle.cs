using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An angle expressed in radians
    /// </summary>
    public struct Angle : IComparable<Angle>
    {
        #region Fields

        /// <summary>
        /// The angle expressed in radians
        /// </summary>
        public readonly double Radians;

        #endregion

        #region Properties

        /// <summary>
        /// Get the angle expressed in degrees
        /// </summary>
        public double Degrees { get { return 180 * Radians / Math.PI; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises the angle to the specified radians value.
        /// </summary>
        /// <param name="radians">The value of the angle in radians</param>
        public Angle(double radians)
        {
            Radians = radians;
        }

        /// <summary>
        /// Initialises the angle as a copy of another
        /// </summary>
        /// <param name="other"></param>
        public Angle(Angle other)
        {
            Radians = other.Radians;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Normalize this angle to between -PI and +PI
        /// </summary>
        /// <returns>A normalised copy of this angle</returns>
        public Angle Normalize()
        {
            double result = Radians % (2 * Math.PI);
            result = (result + 2 * Math.PI) % (2 * Math.PI); //Ensure is positive
            if (result > Math.PI) result -= 2 * Math.PI;
            return result;
        }

        /// <summary>
        /// Normalise this angle to between 0 and 2*PI
        /// </summary>
        /// <returns>A normalised copy of this angle</returns>
        public Angle NormalizeTo2PI()
        {
            double result = Radians % (2 * Math.PI);
            result = (result + 2 * Math.PI) % (2 * Math.PI); //Ensure is positive
            return result;
        }

        /// <summary>
        /// Get a unit vector describing the direction of this angle on the
        /// XY plane.
        /// </summary>
        /// <returns></returns>
        public Vector Direction()
        {
            return new Vector(this);
        }

        public override bool Equals(object obj)
        {
            return Radians.Equals((double)obj);
        }

        public override int GetHashCode()
        {
            return Radians.GetHashCode();
        }

        public int CompareTo(Angle other)
        {
            return Radians.CompareTo(other.Radians);
        }

        public override string ToString()
        {
            return (Radians / Math.PI).ToString() + "π";
        }

        #endregion

        #region Static Methods

        public static Angle FromDegrees(double degrees)
        {
            return new Angle(Math.PI * degrees / 180);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implicit conversion to a double
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator double(Angle value) { return value.Radians; }

        /// <summary>
        /// Implicit conversion from a double
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Angle(double value) { return new Angle(value); }

        public static Angle operator + (Angle a1, Angle a2) => new Angle(a1.Radians + a2.Radians);

        public static Angle operator - (Angle a1, Angle a2) => new Angle(a1.Radians - a2.Radians);

        public static Angle operator * (Angle a, double d) => new Angle(a.Radians * d);

        public static Angle operator * (double d, Angle a) => new Angle(a.Radians * d);

        public static Angle operator / (Angle a, double d) => new Angle(a.Radians / d);

        #endregion
    }
}

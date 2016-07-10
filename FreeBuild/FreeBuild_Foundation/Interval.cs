using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maths
{
    /// <summary>
    /// A numeric interval.
    /// Represents a set of real numbers with the property that any number
    /// that lies between two numbers in the set is also included in the set.
    /// Immutable.
    /// </summary>
    [Serializable]
    public struct Interval :
        IEquatable<Interval>,
        IComparable<Interval>
    {
        #region Constants

        /// <summary>
        /// Constant value representing an unset interval
        /// </summary>
        public static readonly Interval Unset = new Interval(double.NaN, double.NaN);

        #endregion

        #region Fields

        /// <summary>
        /// The minimum, or start, value of this interval.
        /// </summary>
        public readonly double Min;

        /// <summary>
        /// The maximum, or end, value of this interval.
        /// </summary>
        public readonly double Max;

        #endregion

        #region Properties

        /// <summary>
        /// The mid-point of this interval
        /// </summary>
        public double Mid { get { return (Min + Max) / 2; } }

        /// <summary>
        /// The size, or length, of this interval
        /// </summary>
        public double Size { get { return (Max - Min); } }

        /// <summary>
        /// Gets the indexed bound of this interval.
        /// 0 = Min, 1 = Max
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this [int index]
        {
            get
            {
                if (index == 0) return Min;
                else if (index == 1) return Max;
                else throw new IndexOutOfRangeException();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor creating a singularity interval with
        /// maximum and minimum 
        /// </summary>
        /// <param name="value"></param>
        public Interval(double value)
        {
            Min = value;
            Max = value;
        }

        /// <summary>
        /// Constructor creating an interval from maximum and minimum values.
        /// The minimum value should be lower than or equal to the maximum, or else
        /// this interval will not be valid.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Interval(double min, double max)
        {
            Min = min;
            Max = max;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is this interval valid?
        /// An interval is valid providing both of its limits are not set to NaN
        /// and the minimum value is below or equal to the maximum one.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !double.IsNaN(Min) && !double.IsNaN(Max) && Min <= Max;
        }

        /// <summary>
        /// Is this a singularity?  i.e. are the maximum and minimum values the same?
        /// </summary>
        /// <returns></returns>
        public bool IsSingularity()
        {
            return Min == Max;
        }

        /// <summary>
        /// Is this interval equal to another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Interval other)
        {
            return Min == other.Min && Max == other.Max; 
        }

        /// <summary>
        /// IComparable implementation.  Compares this interval to another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Interval other)
        {
            if (Min < other.Min)
                return -1;
            if (Min > other.Min)
                return 1;
            if (Max < other.Max)
                return -1;
            if (Max > other.Max)
                return 1;
            return 0;
        }

        /// <summary>
        /// GetHashCode override.
        /// Generates a hash code by XORing those of its components.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        /// <summary>
        /// Evaluate the value at a normalised parameter of this interval.
        /// </summary>
        /// <param name="parameter">A normalised parameter.
        /// 0 = Min, 1 = Max.</param>
        /// <returns></returns>
        public double ValueAt(double parameter)
        {
            return Min + parameter * (Max - Min);
        }

        /// <summary>
        /// Does this interval include the specified value?
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the specified value falls on or between the interval limits, else false.</returns>
        public bool Contains(double value)
        {
            return value >= Min && value <= Max;
        }

        /// <summary>
        /// Does this interval entirely include another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Contains(Interval other)
        {
            return Min <= other.Min && Max >= other.Max;
        }

        /// <summary>
        /// Does this interval entirely or partiall include another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(Interval other)
        {
            return Min <= other.Max && Max >= other.Min;
        }

        /// <summary>
        /// Find the overlap between this interval and another,
        /// if there is one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The interval overlap between the two intervals.
        /// Or, Interval.Unset if there is no overlap.</returns>
        public Interval Overlap(Interval other)
        {
            if (Min <= other.Max && Max >= other.Min)
            {
                return new Interval(Math.Max(Min, other.Min), Math.Min(Max, other.Max));
            }
            else return Unset;
        }

        /// <summary>
        /// Expand this interval to include the specified value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns
        public Interval Include(double value)
        {
            if (value > Max) return new Interval(Min, value);
            else if (value < Min) return new Interval(value, Max);
            else return this;
        }

        /// <summary>
        /// Find the union of this and another interval.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>An interval from the combined minimum and maximum 
        /// values of the two intervals.</returns>
        public Interval Union(Interval other)
        {
            return new Interval(Math.Min(Min, other.Min), Math.Max(Max, other.Max));
        }

        #endregion

        #region Operators

        /// <summary>
        /// Addition operator override.
        /// Adds a double value to both max and min of an interval.
        /// </summary>
        public static Interval operator +(Interval i, double d)
            => new Interval(i.Min + d, i.Max + d);

        /// <summary>
        /// Addition operator override.
        /// Adds a double value to both max and min of an interval.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator +(double d, Interval i)
            => new Interval(i.Min + d, i.Max + d);

        /// <summary>
        /// Addition operator override.  
        /// Calculates the range of possible values if any value in the
        /// first interval is added to any value in the second
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Interval operator +(Interval i1, Interval i2)
            => new Interval(i1.Min + i2.Min);

        /// <summary>
        /// Subtraction operator override.  Subtracts a double from both
        /// max and min of an interval.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator -(Interval i, double d)
            => new Interval(i.Min - d, i.Max - d);

        /// <summary>
        /// Subtraction operator override.
        /// Returns the range of possible values from subtracting an 
        /// interval from a double.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator -(double d, Interval i)
            => new Interval(d - i.Max, d - i.Min);

        /// <summary>
        /// Subtraction operator override.
        /// Returns the range of possible values from subtracting one
        /// interval from another.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Interval operator -(Interval i1, Interval i2)
            => new Interval(i1.Min - i2.Max, i1.Max - i2.Min);

        /// <summary>
        /// Multiplication operator override.
        /// Multiplies an interval by a scalar.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator *(Interval i, double d)
            => new Interval(i.Min * d, i.Max * d);

        /// <summary>
        /// Multiplication operator override.
        /// Multiplies an interval by a scalar.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator *(double d, Interval i)
            => new Interval(i.Min * d, i.Max * d);

        /// <summary>
        /// Multiplication operator override.
        /// Returns the range of possible values from
        /// multiplying one interval by another.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Interval operator *(Interval i1, Interval i2)
            => new Interval(i1.Min*i2.Min, i1.Max*i2.Max);

        /// <summary>
        /// Division operator override.
        /// Divides an interval by a divisor.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator /(Interval i, double d)
            => new Interval(i.Min / d, i.Max / d);

        /// <summary>
        /// Division operator override.
        /// Divides a value by an interval.
        /// Returns the range of possible values emerging from the division.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Interval operator /(double d, Interval i)
            => new Interval(d / i.Max, d / i.Min);

        #endregion
    }
}

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

using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
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
        public readonly double Start;

        /// <summary>
        /// The maximum, or end, value of this interval.
        /// </summary>
        public readonly double End;

        #endregion

        #region Properties

        /// <summary>
        /// Is this interval valid?
        /// An interval is valid providing both of its limits are not set to NaN.
        /// </summary>
        /// <returns></returns>
        public bool IsValid
        {
            get { return !double.IsNaN(Start) && !double.IsNaN(End); }
        }

        /// <summary>
        /// Is this a singularity?  i.e. are the maximum and minimum values the same?
        /// </summary>
        /// <returns></returns>
        public bool IsSingularity
        {
            get { return Start == End; }
        }

        /// <summary>
        /// The mid-point of this interval
        /// </summary>
        public double Mid { get { return (Start + End) / 2; } }

        /// <summary>
        /// The size, or length, of this interval
        /// </summary>
        public double Size { get { return (End - Start); } }

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
                if (index == 0) return Start;
                else if (index == 1) return End;
                else throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Does this interval represent a range of values which increases from start to finish
        /// </summary>
        public bool IsIncreasing
        {
            get { return Start < End; }
        }

        /// <summary>
        /// Does this interval represent a range of
        /// </summary>
        public bool IsDecreasing
        {
            get { return Start > End; }
        }

        /// <summary>
        /// Get the signed value of the greatest absolute value in this interval.
        /// This will return whichever of Max and Min has the largest (unsigned) magnitude.
        /// </summary>
        public double AbsMax { get { return Math.Abs(End) > Math.Abs(Start) ? End : Start; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor creating a singularity interval with
        /// maximum and minimum set to the specified value
        /// </summary>
        /// <param name="value"></param>
        public Interval(double value)
        {
            Start = value;
            End = value;
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
            Start = min;
            End = max;
        }

        /// <summary>
        /// Constructor creating an interval surrounding the specified set of 
        /// values.  The maximum and minimum will be automatically determined.
        /// </summary>
        /// <param name="list"></param>
        public Interval(double val1, double val2, double val3)
        {
            if (val1 < val2)
            {
                Start = Math.Min(val1, val3);
                End = Math.Max(val2, val3);
            }
            else
            {
                Start = Math.Min(val2, val3);
                End = Math.Max(val1, val3);
            }
        }

        /// <summary>
        /// Constructor creating an interval surrounding the specified set of 
        /// values.  The maximum and minimum will be automatically determined
        /// </summary>
        /// <param name="list"></param>
        public Interval(params double[] list)
        {
            double min = list[0];
            double max = list[1];
            for (int i = 0; i < list.Length; i++)
            {
                double val = list[i];
                if (val < min) min = val;
                if (val > max) max = val;
            }
            Start = min;
            End = max;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is this interval equal to another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Interval other)
        {
            return Start == other.Start && End == other.End; 
        }

        /// <summary>
        /// Is this interval equal to a value?
        /// The interval counts as equal to a single value only if the interval
        /// is a singularity with limits equal to the value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Equals(double value)
        {
            return Start == value && End == value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            else if (obj is Interval) return Equals((Interval)obj);
            else if (typeof(double).IsAssignableFrom(obj.GetType())) return Equals((double)obj);
            else return false;
        }

        /// <summary>
        /// Is this interval less than another?
        /// The interval counts as less than another if its maximum value
        /// is lower than the minimum value of the other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool LesserThan(Interval other)
        {
            return End < other.Start;
        }

        /// <summary>
        /// Is this interval greater than another?
        /// The interval counts as greater than another if its minimum value
        /// is greater than the maximum value of the other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool GreaterThan(Interval other)
        {
            return Start > other.End;
        }

        /// <summary>
        /// IComparable implementation.  Compares this interval to another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Interval other)
        {
            if (Start < other.Start)
                return -1;
            if (Start > other.Start)
                return 1;
            if (End < other.End)
                return -1;
            if (End > other.End)
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
            return Start.GetHashCode() ^ End.GetHashCode();
        }

        /// <summary>
        /// Evaluate the value at a normalised parameter of this interval.
        /// </summary>
        /// <param name="parameter">A normalised parameter.
        /// 0 = Min, 1 = Max.</param>
        /// <returns></returns>
        public double ValueAt(double parameter)
        {
            return Start + parameter * (End - Start);
        }

        /// <summary>
        /// Evaluate the normalised parameter (where 0 is Min and 1 is Max) of
        /// the specified value's position related to this interval
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double ParameterOf(double value)
        {
            double size = Size;
            return size != 0 ? (value - Start) / size : 0;
        }

        /// <summary>
        /// Does this interval include the specified value?
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the specified value falls on or between the interval limits, else false.</returns>
        public bool Contains(double value)
        {
            return value >= Start && value <= End;
        }

        /// <summary>
        /// Does this interval entirely include another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Contains(Interval other)
        {
            return Start <= other.Start && End >= other.End;
        }

        /// <summary>
        /// Does this interval entirely or partiall include another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(Interval other)
        {
            return Start <= other.End && End >= other.Start;
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
            if (Start <= other.End && End >= other.Start)
            {
                return new Interval(Math.Max(Start, other.Start), Math.Min(End, other.End));
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
            if (value > End) return new Interval(Start, value);
            else if (value < Start) return new Interval(value, End);
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
            return new Interval(Math.Min(Start, other.Start), Math.Max(End, other.End));
        }

        /// <summary>
        /// Interpolate this Interval towards another
        /// </summary>
        /// <param name="towards">The interval to interpolate towards</param>
        /// <param name="factor">The interpolation factor.  0 = this interval, 1 = the 'towards' interval</param>
        /// <returns></returns>
        public Interval Interoplate(Interval towards, double factor)
        {
            return new Interval(
                Start.Interpolate(towards.Start, factor),
                End.Interpolate(towards.End, factor));
        }

        /// <summary>
        /// Interpolate this Interval towards another
        /// </summary>
        /// <param name="i1">The other interval to interpolate towards</param>
        /// <param name="x0">The key value mapped to this interval</param>
        /// <param name="x1">The key value mapped to the other interval</param>
        /// <param name="x">The key value at the interpolation point</param>
        /// <returns></returns>
        public Interval Interpolate(Interval i1, double x0, double x1, double x)
        {
            return new Interval(
                Start.Interpolate(i1.Start, x0, x1, x),
                End.Interpolate(i1.End, x0, x1, x));
        }

        /// <summary>
        /// Generate a set of evently-spaced 'reasonable' rounded numbers within
        /// this interval, to be used as such things as generating grid lines on graphs
        /// </summary>
        /// <param name="divisions">The target number of divisions.  The ultimate number of values may be slightly more or less than this depending on rounding</param>
        /// <returns></returns>
        public IList<double> ReasonableDivisions(int divisions)
        {
            var result = new List<double>();
            int digits = divisions.Digits();
            double increment = (Size/divisions).RoundToSignificantFigures(digits);
            if (increment > 0)
            {
                // Start at 0 if 0 is included in this interval:
                if (Contains(0))
                {
                    int negDivs = (int)Math.Floor(-Start / increment);
                    for (int i = negDivs; i > 0; i--) result.Add(i * -increment);
                    int posDivs = (int)Math.Floor(End / increment);
                    for (int i = 0; i <= posDivs; i++) result.Add(i * increment);
                }
                else
                {
                    for (double value = Start.RoundToSignificantFigures(digits); value < End; value += increment) result.Add(value);
                }
            }
            return result;
        }

        /// <summary>
        /// Wrap the specified value to the bounds of this interval
        /// </summary>
        /// <param name="value">The value to wrap</param>
        /// <returns></returns>
        public double Wrap(double value)
        {
            return Start + (value - Start) % Size;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Addition operator override.
        /// Adds a double value to both max and min of an interval.
        /// </summary>
        public static Interval operator +(Interval i, double d)
            => new Interval(i.Start + d, i.End + d);

        /// <summary>
        /// Addition operator override.
        /// Adds a double value to both max and min of an interval.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator +(double d, Interval i)
            => new Interval(i.Start + d, i.End + d);

        /// <summary>
        /// Addition operator override.  
        /// Calculates the range of possible values if any value in the
        /// first interval is added to any value in the second
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Interval operator +(Interval i1, Interval i2)
            => new Interval(i1.Start + i2.Start);

        /// <summary>
        /// Subtraction operator override.  Subtracts a double from both
        /// max and min of an interval.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator -(Interval i, double d)
            => new Interval(i.Start - d, i.End - d);

        /// <summary>
        /// Subtraction operator override.
        /// Returns the range of possible values from subtracting an 
        /// interval from a double.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator -(double d, Interval i)
            => new Interval(d - i.End, d - i.Start);

        /// <summary>
        /// Subtraction operator override.
        /// Returns the range of possible values from subtracting one
        /// interval from another.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Interval operator -(Interval i1, Interval i2)
            => new Interval(i1.Start - i2.End, i1.End - i2.Start);

        /// <summary>
        /// Multiplication operator override.
        /// Multiplies an interval by a scalar.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator *(Interval i, double d)
            => new Interval(i.Start * d, i.End * d);

        /// <summary>
        /// Multiplication operator override.
        /// Multiplies an interval by a scalar.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator *(double d, Interval i)
            => new Interval(i.Start * d, i.End * d);

        /// <summary>
        /// Multiplication operator override.
        /// Returns the range of possible values from
        /// multiplying one interval by another.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Interval operator *(Interval i1, Interval i2)
            => new Interval(i1.Start*i2.Start, i1.End*i2.End);

        /// <summary>
        /// Division operator override.
        /// Divides an interval by a divisor.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator /(Interval i, double d)
            => new Interval(i.Start / d, i.End / d);

        /// <summary>
        /// Division operator override.
        /// Divides a value by an interval.
        /// Returns the range of possible values emerging from the division.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Interval operator /(double d, Interval i)
            => new Interval(d / i.End, d / i.Start);

        /// <summary>
        /// Equality operator override
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator == (Interval a, Interval b)
            => a.Equals(b);

        /// <summary>
        /// Inequality operator override
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator != (Interval a, Interval b)
            => !a.Equals(b);


        /// <summary>
        /// Lesser-than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator < (Interval a, Interval b)
            => a.LesserThan(b);

        /// <summary>
        /// Greater-than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator > (Interval a, Interval b)
            => a.GreaterThan(b);

        /// <summary>
        /// Interval-double equality operator
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool operator == (Interval i, double d)
            => i.Equals(d);

        /// <summary>
        /// Interval double equality operator
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool operator != (Interval i, double d)
            => !i.Equals(d);

        #endregion
    }
}

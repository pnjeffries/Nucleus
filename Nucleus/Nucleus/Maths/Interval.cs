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

        /// <summary>
        /// The unit interval from 0 to 1.
        /// </summary>
        public static readonly Interval Unit = new Interval(0.0, 1.0);

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
        /// Get the minimum value encompassed by this interval
        /// (This will be the lesser of the Start and End fields)
        /// </summary>
        public double Min { get { return Math.Min(Start, End); } }

        /// <summary>
        /// Get the maximum value encompassed by this interval
        /// (This will be the greater of the Start and End fields)
        /// </summary>
        public double Max { get { return Math.Max(Start, End); } }

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
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Interval(double start, double end)
        {
            Start = start;
            End = end;
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
        /// Wrap the start and end values of this interval to the
        /// specified interval
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public Interval WrapTo(Interval bounds)
        {
            return new Interval(Start.WrapTo(bounds), End.WrapTo(bounds));
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
        /// Evaluate the normalised parameter (where 0 is Start and 1 is End) of
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
        /// Normalise another interval to the range of this one.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Interval ParameterOf(Interval value)
        {
            return new Interval(ParameterOf(value.Start), ParameterOf(value.End));
        }

        /// <summary>
        /// Does this interval include the specified value?
        /// Both start and end are inclusive.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the specified value falls on or between the interval limits, else false.</returns>
        public bool Contains(double value)
        {
            return value >= Start && value <= End;
        }

        /// <summary>
        /// Is the parameter t enclosed by this interval?
        /// The start value is inclusive, the end is exclusive
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ContainsOpenEndWrapped(double t)
        {
            if (IsDecreasing) return (t < End || t >= Start);
            else return (t >= Start && t < End);
        }

        public bool ContainsWrapped(Interval contains)
        {
            if (this.IsDecreasing)
                return (contains.Start < this.End || contains.Start > this.Start) && (contains.End < this.End || contains.End >this.Start);
            else return (contains.Start > this.Start && contains.End < this.End);
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
        /// Does this interval entirely or partially include another?
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
        public Interval Interpolate(Interval towards, double factor)
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

        /// <summary>
        /// Create a new interval with the same end value as this one
        /// but with the new specified start value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Interval WithStart(double value)
        {
            return new Interval(value, End);
        }

        /// <summary>
        /// Create a new interval with the same start value as this one
        /// but with the new specified end value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Interval WithEnd(double value)
        {
            return new Interval(Start, value);
        }

        /// <summary>
        /// Generate a random double within this interval
        /// </summary>
        /// <param name="rng">The random number generator to use</param>
        /// <returns></returns>
        public double Random(Random rng)
        {
            return Start + rng.NextDouble() * Size;
        }

        public override string ToString()
        {
            if (IsSingularity) return End.ToString();
            else return "[" + Start.ToString() + ";" + End.ToString() + "]";
        }

        
        /// <summary>
        /// Find the boolean difference of this interval and a set of other intervals which
        /// subtract from it.
        /// </summary>
        /// <param name="detractors"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public IList<Interval> BooleanDifference(IList<Interval> subtractors)
        {
            IList<Interval> result = new List<Interval>();
            BooleanDifference(subtractors, result);
            return result;
        }

        /// <summary>
        /// Find the boolean difference of this interval and a set of other intervals which
        /// subtract from it.
        /// </summary>
        /// <param name="detractors"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public void BooleanDifference(IList<Interval> subtractors, IList<Interval> outList)
        {
            Interval interval = this;
            if (subtractors.Count > 0)
            {
                double t = interval.Start;
                Interval cutter = subtractors.FindEnclosing(t); // Test whether the start point is inside a cutter
                bool wrapped = false; // Have we wrapped around the domain yet?

                int cutterCount = 0;
                while (true)
                {
                    if (cutterCount > subtractors.Count) return;
                    if (cutter.IsValid)
                    {
                        if (cutter.IsDecreasing && t >= cutter.Start) wrapped = true; //

                        // Move to the end of the current cutter:
                        t = cutter.End;

                        // Check for a next enclosing cutter:
                        cutter = subtractors.FindEnclosing(t);
                        cutterCount++;
                    }
                    else
                    {
                        double tNext = interval.End;
                        // Move to the start of the next cutter:
                        cutter = subtractors.FindNext(t);

                        if (cutter.IsValid &&
                            ((interval.IsIncreasing && tNext > cutter.Start) ||
                            (interval.IsDecreasing)))
                        {
                            // Move to the start of the next cutter:
                            tNext = cutter.Start;
                        }
                        else if (cutter.IsValid && !wrapped && interval.IsDecreasing)
                        {
                            // Wrap around to the start if the interval does:
                            cutter = subtractors.FindNext(double.MinValue);
                            wrapped = true;
                            // Move to the start of the next cutter:
                            if (cutter.IsValid && cutter.Start < tNext) tNext = cutter.Start;
                        }
                        else if (tNext < t) wrapped = true;

                        if (tNext != t)
                            outList.Add(new Interval(t, tNext));
                        else
                            tNext = t.NextValidValue(); //Nudge it forward!  Bit hacky...
                        t = tNext;
                    }

                    // Check end conditions:
                    if (interval.IsIncreasing && (t >= interval.End || wrapped)) return;
                    if (interval.IsDecreasing && (t >= interval.End && wrapped)) return;
                    if (outList.Count > subtractors.Count) return;
                }
            }
            else
            {
                outList.Add(interval);
            }
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

        /// <summary>
        /// Interval modulo operator
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Interval operator % (Interval i, double d)
        {
            return new Interval(i.Start % d, i.End % d);
        }

        #endregion
    }

    public static class IntervalExtensions
    {
        /// <summary>
        /// Find the first interval in this collection that encloses the specified t value
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Interval FindEnclosing(this IList<Interval> intervals, double t)
        {
            foreach (Interval interval in intervals)
            {
                if (interval.ContainsOpenEndWrapped(t)) return interval;
            }
            return Interval.Unset;
        }

        /// <summary>
        /// Find the next interval in this collection starting after the specified parameter
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Interval FindNext(this IList<Interval> intervals, double t)
        {
            Interval next = Interval.Unset;
            if (intervals != null && intervals.Count > 0)
            {
                foreach (Interval interval in intervals)
                {
                    if (interval.Start > t && (!next.IsValid || next.Start > interval.Start))
                    {
                        next = interval;
                    }
                }
            }
            return next;
        }
    }
}

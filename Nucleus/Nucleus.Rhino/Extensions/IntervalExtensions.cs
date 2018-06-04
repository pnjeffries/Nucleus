using Nucleus.Extensions;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot
{
    /// <summary>
    /// Extension methods for the RhinoCommon Interval struct
    /// </summary>
    public static class IntervalExtensions
    {
        /// <summary>
        /// Is the parameter t included in any of the intervals within this collection?
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Includes(this IList<Interval> intervals, double t)
        {
            foreach (Interval interval in intervals)
            {
                if (interval.Encloses(t)) return true;
            }
            return false;
        }

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
                if (interval.Encloses(t)) return interval;
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
                    if (interval.T0 > t && (!next.IsValid || next.T0 > interval.T0))
                    {
                        next = interval;
                    }
                }
            }
            return next;
        }

        /// <summary>
        /// Find the boolean difference of an interval and a set of other intervals which
        /// subtract from it.
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="detractors"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static IList<Interval> BooleanDifference(this Interval interval, IList<Interval> subtractors)
        {
            IList<Interval> result = new List<Interval>();
            BooleanDifference(interval, subtractors, result);
            //result.Add(interval);
            return result;
        }

        /// <summary>
        /// Find the boolean difference of an interval and a set of other intervals which
        /// subtract from it.
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="detractors"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static void BooleanDifference(this Rhino.Geometry.Interval interval, IList<Rhino.Geometry.Interval> subtractors, IList<Interval> outList)
        {
            if (subtractors.Count > 0)
            {
                double t = interval.T0;
                Interval cutter = subtractors.FindEnclosing(t); // Test whether the start point is inside a cutter
                bool wrapped = false; // Have we wrapped around the domain yet?

                int cutterCount = 0;
                while (true)
                {
                    if (cutterCount > subtractors.Count) return;
                    if (cutter != Interval.Unset)
                    {
                        if (cutter.IsDecreasing && t >= cutter.T0) wrapped = true; //

                        // Move to the end of the current cutter:
                        t = cutter.T1;   

                        // Check for a next enclosing cutter:
                        cutter = subtractors.FindEnclosing(t);
                        cutterCount++;
                    }
                    else
                    {
                        double tNext = interval.T1;
                        // Move to the start of the next cutter:
                        cutter = subtractors.FindNext(t);
                        
                        if (cutter != Interval.Unset && 
                            ((interval.IsIncreasing && tNext > cutter.T0) ||
                            (interval.IsDecreasing)))
                        {
                            // Move to the start of the next cutter:
                            tNext = cutter.T0;
                        }
                        else if (cutter == Interval.Unset && !wrapped && interval.IsDecreasing)
                        {
                            // Wrap around to the start if the interval does:
                            cutter = subtractors.FindNext(double.MinValue);
                            wrapped = true;
                            // Move to the start of the next cutter:
                            if (cutter != Interval.Unset && cutter.T0 < tNext) tNext = cutter.T0;
                        }
                        
                        if (tNext != t)
                            outList.Add(new Interval(t, tNext));
                        else
                           tNext = t.NextValidValue(); //Nudge it forward!  Bit hacky...
                        t = tNext;
                    }

                    // Check end conditions:
                    if (interval.IsIncreasing && (t >= interval.T1 || wrapped)) return;
                    if (interval.IsDecreasing && (t >= interval.T1 && wrapped)) return;
                    if (outList.Count > subtractors.Count) return;
                }
            }
            else
            {
                outList.Add(interval);
            }

                //==================================

                /*foreach (Interval subtractor in subtractors)
                {
                    if (interval.IsIncreasing == subtractor.IsIncreasing) // Both wrapping or both not
                    {
                        if (interval.T1 <= subtractor.T1 && subtractor.T0 < interval.T0) return;
                        else if (interval.T0 < subtractor.T0 && subtractor.T1 < interval.T1)
                        {
                            // Split in two:
                            Interval newInterval = new Interval(subtractor.T1, interval.T1);
                            if (Math.Abs(newInterval.Length) > 0.0001) BooleanDifference(newInterval, subtractors, outList);
                            interval = new Interval(interval.T0, subtractor.T0);
                            if (Math.Abs(interval.Length) < 0.0001) return;
                            continue;
                        }
                    }
                    else //One wrapping, one not
                    {
                        if ((interval.T0 >= subtractor.T0 || interval.T0 <= subtractor.T1) &&
                            ()
                        if (interval.T0 < subtractor.T1 && subtractor.T0 < interval.T1)
                        {
                            interval = new Interval(subtractor.T1, subtractor.T0);
                            continue;
                        }
                    }

                    // Trim ends:
                    if (interval.T0 < subtractor.T1 && subtractor.T0 < interval.T0)
                        interval = new Interval(subtractor.T1, interval.T1);
                    else if (interval.T1 < subtractor.T1 && subtractor.T0 < interval.T1)
                        interval = new Interval(interval.T0, subtractor.T0);

                    /*


                    // Interval is annihilated:
                    if (subtractor.IntervalContains(interval)) return;
                    if (interval == subtractor) return;

                    if (IntervalContains(interval, subtractor)) // Interval wholly contains subtractor
                    {
                        // Split in two:
                        Interval newInterval = new Interval(subtractor.T1, interval.T1);
                        if (Math.Abs(newInterval.Length) > 0.0001) BooleanDifference(newInterval, subtractors, outList);
                        interval = new Interval(interval.T0, subtractor.T0);
                        if (Math.Abs(interval.Length) < 0.0001) return;
                    }
                    else // Does interval partially contain subtractor?
                    { 
                        if (interval.IntervalContains(subtractor.T0))
                            interval = new Interval(interval.T0, subtractor.T0);

                        if (interval.IntervalContains(subtractor.T1))
                            interval = new Interval(subtractor.T1, interval.T1);
                    }


                    //else if (IntervalContains(subtractor, interval.T0))
                    //{
                    //    //if (IntervalContains(subtractor, interval.T1))
                    //    //{
                    //    //    return; // Interval is wholly inside - shortcut out
                    //    //}
                    //    //else
                    //    interval = new Interval(subtractor.T1, interval.T1);
                    //}
                    //else if (IntervalContains(subtractor, interval.T1))
                    //{
                    //    interval = new Interval(interval.Min, subtractor.T0);
                    //}
                    //else if (IntervalContains(interval, subtractor))
                    //{
                    //    // Split in two:
                    //    Interval newInterval = new Interval(subtractor.T1, interval.T1);
                    //    if (Math.Abs(newInterval.Length) > 0.0001) BooleanDifference(newInterval, subtractors, outList);
                    //    interval = new Interval(interval.T0, subtractor.T0);
                    //    if (Math.Abs(interval.Length) < 0.0001) return;
                    //}

                }

                outList.Add(interval);
                */
            
        }

        /// <summary>
        /// Is the parameter t enclosed by this interval?
        /// The start value is inclusive, the end is exclusive
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Encloses(this Interval interval, double t)
        {
            if (interval.IsDecreasing) return (t < interval.T1 || t >= interval.T0);
            else return (t >= interval.T0 && t < interval.T1);
        }

        public static bool IntervalContains(this Interval interval, Interval contains)
        {
            if (interval.IsDecreasing)
                return (contains.T0 < interval.T1 || contains.T0 > interval.T0) && (contains.T1 < interval.T1 || contains.T1 > interval.T0);
            else return (contains.T0 > interval.T0 && contains.T1 < interval.T1);
        }
    }
}

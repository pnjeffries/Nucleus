using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucleus.Rhino
{
    /// <summary>
    /// Extension methods for the RhinoCommon Curve class
    /// </summary>
    public static class CurveExtensions
    {
        /// <summary>
        /// Pull out a segment of a curve domain based on the centre of the segment and the overall length of the segment
        /// </summary>
        /// <param name="curve">The curve</param>
        /// <param name="tMid">The parameter on the old curve at the mid-point of the new one</param>
        /// <param name="length">The length of the resultant sub-curve</param>
        /// <returns></returns>
        public static Interval SubDomainFromCentre(this Curve curve, double tMid, double length)
        {
            double crvLength = curve.GetLength();
            // Check whether the sub-curve encompasses the whole
            if (length >= crvLength) return curve.Domain;

            // Calculate lengths along curve of start and end
            double midLength = curve.GetLength(new Interval(curve.Domain.Min, tMid));
            double startLength = midLength - length / 2.0;
            double endLength = midLength + length / 2.0;

            // Fit lengths to the actual curve:

            if (startLength < 0)
            {
                if (curve.IsClosed)
                {
                    startLength = crvLength + startLength;
                }
                else startLength = 0;
            }
            if (endLength > crvLength)
            {
                if (curve.IsClosed)
                {
                    endLength = endLength - crvLength;
                }
                else endLength = crvLength;
            }

            double tStart;
            curve.LengthParameter(startLength, out tStart);
            double tEnd;
            curve.LengthParameter(endLength, out tEnd);

            return new Interval(tStart, tEnd);
        }

        /// <summary>
        /// Split this curve, rejoining the start and end pieces into one curve if they were originally joined
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Curve[] SplitPeriodic(this Curve curve, IEnumerable<double> t)
        {
            // Split the curves via the traditional method:
            Curve[] curves = curve.Split(t);
            if (curve.IsClosed && curves.Length > t.Count())
            {
                // Rejoin the start and end:
                Curve[] endStart = Curve.JoinCurves(new Curve[] { curves.Last(), curves.First() }, 0.00001, true);

                // Combine together back to a single array:
                Curve[] result = new Curve[endStart.Length + curves.Length - 2];
                int i = 0;
                for (int j = 0; j < endStart.Length; j++)
                {
                    result[i] = endStart[j];
                    i++;
                }
                for (int j = 1; j < curves.Length - 1; j++)
                {
                    result[i] = curves[j];
                    i++;
                }
                return result;
            }
            else return curves;
        }

        /// <summary>
        /// Get the longest curve in this collection
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static Curve GetLongest(this IList<Curve> curves)
        {
            double maxLength = 0;
            Curve longest = null;
            foreach (Curve curve in curves)
            {
                double length = curve.GetLength();
                if (length > maxLength)
                {
                    maxLength = length;
                    longest = curve;
                }
            }
            return longest;
        }

        /// <summary>
        /// Break this curve down into its constituent segments.
        /// </summary>
        /// <param name="crv"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static IList<Curve> Explode(this Curve crv, bool recursive)
        {
            var result = new List<Curve>();
            crv.Explode(result, recursive);
            return result;        
        }

        /// <summary>
        /// Break a curve down into its constituent segments.
        /// Based on David Rutten's version here:
        /// http://www.grasshopper3d.com/forum/topics/explode-closed-planar-curve-using-rhinocommon
        /// </summary>
        /// <param name="crv"></param>
        /// <param name="outCurves"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static bool Explode(this Curve crv, List<Curve> outCurves, bool recursive)
        {
            if (crv == null) { return false; }
            PolyCurve polycurve = crv as PolyCurve;
            if (polycurve != null)
            {
                if (recursive) { polycurve.RemoveNesting(); }
                Curve[] segments = polycurve.Explode();
                if (segments == null) { return false; }
                if (segments.Length == 0) { return false; }
                if (recursive)
                {
                    foreach (Curve S in segments)
                    {
                        S.Explode(outCurves, recursive);
                    }
                }
                else
                {
                    foreach (Curve S in segments)
                    {
                        outCurves.Add(S.DuplicateShallow() as Curve);
                    }
                }
                return true;
            }
            PolylineCurve polyline = crv as PolylineCurve;
            if (polyline != null)
            {
                if (recursive)
                {
                    for (int i = 0; i < (polyline.PointCount - 1); i++)
                    {
                        outCurves.Add(new LineCurve(polyline.Point(i), polyline.Point(i + 1)));
                    }
                }
                else
                {
                    outCurves.Add(polyline.DuplicateCurve());
                }
                return true;
            }
            Polyline p;
            if (crv.TryGetPolyline(out p))
            {
                if (recursive)
                {
                    for (int i = 0; i < (p.Count - 1); i++)
                    {
                        outCurves.Add(new LineCurve(p[i], p[i + 1]));
                    }
                }
                else
                {
                    outCurves.Add(new PolylineCurve(p));
                }
                return true;
            }
            //Maybe it's a LineCurve?
            LineCurve line = crv as LineCurve;
            if (line != null)
            {
                outCurves.Add(line.DuplicateCurve());
                return true;
            }
            //It might still be an ArcCurve...
            ArcCurve arc = crv as ArcCurve;
            if (arc != null)
            {
                outCurves.Add(arc.DuplicateCurve());
                return true;
            }
            //Nothing else worked, lets assume it's a nurbs curve and go from there...
            NurbsCurve nurbs = crv.ToNurbsCurve();
            if (nurbs == null) { return false; }
            double t0 = nurbs.Domain.Min;
            double t1 = nurbs.Domain.Max;
            double t;
            int LN = outCurves.Count;
            do
            {
                if (!nurbs.GetNextDiscontinuity(Continuity.C1_locus_continuous, t0, t1, out t)) { break; }
                Interval trim = new Interval(t0, t);
                if (trim.Length < 1e-10)
                {
                    t0 = t;
                    continue;
                }
                Curve M = nurbs.DuplicateCurve();
                M = M.Trim(trim);
                if (M.IsValid) { outCurves.Add(M); }
                t0 = t;
            } while (true);
            if (outCurves.Count == LN) { outCurves.Add(nurbs); }
            return true;
        }

    }
}

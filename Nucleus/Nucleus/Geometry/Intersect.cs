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
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A static class of helper functions to find the intersections 
    /// between geometries of various types
    /// </summary>
    public static class Intersect
    {
        /// <summary>
        /// Find the intersection point, if one exists, for two infinite lines on the XY plane.
        /// For 3D, use the Axis class and ClosestPoint function instead.
        /// </summary>
        /// <param name="pt0">The origin point of the first line</param>
        /// <param name="v0">The direction of the first line</param>
        /// <param name="pt1">The origin point of the second line</param>
        /// <param name="v1">The direction of the second line</param>
        /// <returns>The XY intersection point, if one exists.  Else (the lines are null or parallel) Vector.Unset</returns>
        public static Vector LineLineXY(Vector pt0, Vector v0, Vector pt1, Vector v1)
        {
            if (v0.X.Abs() < 0.00000001)
            {
                if (v1.X.Abs() < 0.00000001) return Vector.Unset;
                else
                {
                    double m2 = v1.Y / v1.X;
                    double c2 = pt1.Y - m2 * pt1.X;
                    double x = pt0.X;
                    return new Vector(x, m2 * x + c2, pt0.Z);
                }
            }
            else if (v1.X.Abs() < 0.00000001)
            {
                double m1 = v0.Y / v0.X;
                double c1 = pt0.Y - m1 * pt0.X;
                double x = pt1.X;
                return new Vector(x, m1 * x + c1, pt0.Z);
            }
            else
            {
                double m1 = v0.Y / v0.X;
                double c1 = pt0.Y - m1 * pt0.X;
                double m2 = v1.Y / v1.X;
                double c2 = pt1.Y - m2 * pt1.X;

                if (m1 - m2 == 0) return Vector.Unset;
                else
                {
                    double x = (c2 - c1) / (m1 - m2);
                    double y = m1 * x + c1;
                    return new Vector(x, y, pt0.Z);
                }
            }
        }

        /// <summary>
        /// Find the intersection point, if one exists, for two infinite lines on the XY plane.
        /// For 3D, use the Axis class and ClosestPoint function instead.
        /// This version also provides the parameters on the two lines - i.e. the multiplication factor necessary
        /// to apply to the line direction vector to get to the intersection point from the line origin.
        /// </summary>
        /// <param name="pt0">The origin point of the first line</param>
        /// <param name="v0">The direction of the first line</param>
        /// <param name="pt1">The origin point of the second line</param>
        /// <param name="v1">The direction of the second line</param>
        /// <param name="t0">The parameter on the first line</param>
        /// <param name="t1">The parameter on the second line</param>
        /// <returns>The XY intersection point, if one exists.  Else (the lines are null or parallel) Vector.Unset</returns>
        public static Vector LineLineXY(Vector pt0, Vector v0, Vector pt1, Vector v1, ref double t0, ref double t1)
        {
            const double tolerance = 0.0000001;
            if (v0.X.Abs() <= tolerance)
            {
                if (v1.X.Abs() <= tolerance)
                {
                    // Lines are parallel, but may still touch/overlap...
                    return Vector.Unset;
                }
                else
                {
                    double m2 = v1.Y / v1.X;
                    double c2 = pt1.Y - m2 * pt1.X;
                    double x = pt0.X;
                    double y = m2 * x + c2;
                    t0 = (y - pt0.Y) / v0.Y;
                    t1 = (x - pt1.X) / v1.X;
                    return new Vector(x, y, pt0.Z);
                }
            }
            else if (v1.X.Abs() <= 0.0000001)
            {
                double m1 = v0.Y / v0.X;
                double c1 = pt0.Y - m1 * pt0.X;
                double x = pt1.X;
                double y = m1 * x + c1;
                t0 = (x - pt0.X) / v0.X;
                t1 = (y - pt1.Y) / v1.Y;
                return new Vector(x, y, pt0.Z);
            }
            else
            {
                double m1 = v0.Y / v0.X;
                double c1 = pt0.Y - m1 * pt0.X;
                double m2 = v1.Y / v1.X;
                double c2 = pt1.Y - m2 * pt1.X;

                if ((m1 - m2).IsTiny()) return Vector.Unset;
                else
                {
                    double x = (c2 - c1) / (m1 - m2);
                    double y = m1 * x + c1;
                    t0 = (x - pt0.X) / v0.X;
                    t1 = (x - pt1.X) / v1.X;
                    return new Vector(x, y, pt0.Z);
                }
            }
        }

        /// <summary>
        /// Find the intersection point, if one exists, for two lines on the XY plane.
        /// By default, the lines will be treated as extending to infinity, but may optionally
        /// be bounded to not return intersections outside the extents of the lines themselves.
        /// </summary>
        /// <param name="lineA">The first line</param>
        /// <param name="lineB">The second line</param>
        /// <param name="bounded">If true, intersections outside the bounds of the line segments specified will be ignored.</param>
        /// <returns></returns>
        public static Vector LineLineXY(Line lineA, Line lineB, bool bounded)
        {
            Interval bounds = Interval.Unset;
            if (bounded) bounds = Interval.Unit;
            return LineLineXY(lineA, lineB, bounds);
        }

        /// <summary>
        /// Find the intersection point, if one exists, for two lines on the XY plane.
        /// By default, the lines will be treated as extending to infinity, but may optionally
        /// be bounded to not return intersections outside the extents of the lines themselves.
        /// </summary>
        /// <param name="lineA">The first line</param>
        /// <param name="lineB">The second line</param>
        /// <param name="bounds">The parameter-space range on each line where intersections are valid</param>
        /// <returns></returns>
        public static Vector LineLineXY(Line lineA, Line lineB, Interval bounds)
        {
            Vector pt0 = lineA.StartPoint;
            Vector v0 = lineA.EndPoint - pt0;
            Vector pt1 = lineB.StartPoint;
            Vector v1 = lineB.EndPoint - pt1;
            double t0 = 0;
            double t1 = 0;
            Vector result = LineLineXY(pt0, v0, pt1, v1, ref t0, ref t1);
            if (!bounds.IsValid || (result.IsValid() && t0 >= bounds.Min && t0 <= bounds.Max && t1 >= bounds.Min && t1 <= bounds.Max)) return result;
            else return Vector.Unset;
        }

        /// <summary>
        /// Find the intersection points, if any exist, for an infinite line and a circle on the XY plane,
        /// given as an array of doubles representing multiplication factors which should be applied to the
        /// line direction vector.  There may be one, two or zero intersection points.
        /// </summary>
        /// <param name="lineOrigin">The origin point of the line</param>
        /// <param name="lineDir">The direction vector of the line</param>
        /// <param name="circleCentre">The centre point of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <returns>An array of doubles representing the line parameters of the intersection points</returns>
        public static double[] LineCircleXY(Vector lineOrigin, Vector lineDir, Vector circleCentre, double radius)
        {
            if (lineDir.X.IsTiny())
            {
                //TODO: Deal with case where Y is also tiny?

                // Treat line as aligned with Y axis
                double dY = circleCentre.Y - lineOrigin.Y;
                double dX = (circleCentre.X - lineOrigin.X).Abs();
                double r = radius.Abs();
                if (dX > r) // Line misses circle
                    return new double[] { };
                else if (dX == r) // Line just hits circle at one point
                    return new double[] { dY / lineDir.Y };
                else
                {
                    double offsetY = Math.Sqrt(r * r - dX * dX);
                    return new double[]
                    {
                        (dY - offsetY)/lineDir.Y,
                        (dY + offsetY)/lineDir.Y
                    };
                }  
            }
            else
            {
                // Solve using quadratic formula:
                double m = lineDir.Y / lineDir.X;
                double y0 = lineOrigin.Y - m * lineOrigin.X;
                double a = m.Squared() + 1;
                double b = 2 * (m * y0 - m * circleCentre.Y - circleCentre.X);
                double c = circleCentre.Y.Squared() - radius.Squared() + circleCentre.X.Squared() 
                    - 2 * y0 * circleCentre.Y + y0.Squared();
                double discriminant = b * b - 4 * a * c;

                if (discriminant < 0) return new double[] { }; // No intersection!
                else
                {
                    double t0 = (((-b - discriminant.Root()) / (2 * a)) - lineOrigin.X)/lineDir.X;
                    if (discriminant == 0) return new double[] { t0 }; // One solution
                    else 
                    {
                        double t1 = (((-b + discriminant.Root()) / (2 * a)) - lineOrigin.X) / lineDir.X;
                        return new double[] { t0, t1 }; // Two solutions
                    }
                }
            }
        }

        /// <summary>
        /// Find the intersection points, if any exist, for a line and a circle on the
        /// XY plane.  Note that for the purposes of calculation both are assumed to lie
        /// on the XY plane even if they do not.
        /// By default, the line is assumed to be infinite, however the calculation may optionally
        /// be bounded to exclude intersections outside of the bounds of the specified line segment
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="circle">The circle.  This should lie on the XY plane for the calculation to be accurate.</param>
        /// <returns></returns>
        public static Vector[] LineCircleXY(Line line, Circle circle, bool bounded = false)
        {
            Vector pt0 = line.StartPoint;
            Vector v0 = line.EndPoint - pt0;
            double[] ts = LineCircleXY(pt0, v0, circle.Origin, circle.Radius);

            // Sort out which intersections exist and (optionally) are within bounds
            bool t0Valid = ts.Length > 0 && (!bounded || (ts[0] >= 0 && ts[0] <= 1));
            bool t1Valid = ts.Length > 1 && (!bounded || (ts[1] >= 0 && ts[1] <= 1));
            if (t0Valid)
            {
                if (t1Valid) return new Vector[] { line.PointAt(ts[0]), line.PointAt(ts[1]) };
                else return new Vector[] { line.PointAt(ts[0]) };
            }
            else if (t1Valid)
                return new Vector[] { line.PointAt(ts[1]) };
            else return new Vector[] { };
        }

        /// <summary>
        /// Find the intersection points, if any exist, for a line and an arc on the XY plane.
        /// By default, the line is assumed to be infinite, however the calculation may optionally 
        /// be bounded to exclude intersections beyond the ends of the specified line segment
        /// </summary>
        /// <param name="lineOrigin">The start point of the line</param>
        /// <param name="lineVect">The vector from the start of the line to its end</param>
        /// <param name="arc">The arc</param>
        /// <param name="lineBounded">Optional.  If true, intersections which lie outside of
        /// the extents of the line will be ignored.</param>
        /// <returns></returns>
        public static Vector[] LineArcXY(Vector lineOrigin, Vector lineVect, Arc arc, bool lineBounded = false)
        {
            Interval bounds = Interval.Unset;
            if (lineBounded) bounds = Interval.Unit;
            return LineArcXY(lineOrigin, lineVect, arc, bounds);
        }

        /// <summary>
        /// Find the intersection points, if any exist, for a line and an arc on the XY plane.
        /// By default, the line is assumed to be infinite, however the calculation may optionally 
        /// be bounded to exclude intersections beyond the ends of the specified line segment
        /// </summary>
        /// <param name="lineOrigin">The start point of the line</param>
        /// <param name="lineVect">The vector from the start of the line to its end</param>
        /// <param name="arc">The arc</param>
        /// <param name="lineBounds">The range on the line, as a proportion of the line length, where intersections
        /// will be valid.  0 will represent the start of the line and 1 its end.</param>
        /// <returns></returns>
        public static Vector[] LineArcXY(Vector lineOrigin, Vector lineVect, Arc arc, Interval lineBounds)
        {
            bool lineBounded = lineBounds.IsValid;
            double[] ts = LineCircleXY(lineOrigin, lineVect, arc.Circle.Origin, arc.Circle.Radius);
            
            // Sort out which intersections exist and (optionally) are within bounds:
            bool t0Valid = ts.Length > 0 && (!lineBounded || (ts[0] >= lineBounds.Min && ts[0] <= lineBounds.Max));
            bool t1Valid = ts.Length > 1 && (!lineBounded || (ts[1] >= lineBounds.Min && ts[1] <= lineBounds.Max));
            Vector pt0 = Vector.Unset;
            Vector pt1 = Vector.Unset;
            if (t0Valid)
            {
                pt0 = lineOrigin + lineVect * ts[0];
                if (!arc.IsInAngleRange(pt0)) t0Valid = false;
            }
            if (t1Valid)
            {
                pt1 = lineOrigin + lineVect * ts[1];
                if (!arc.IsInAngleRange(pt1)) t1Valid = false;
            }

            if (t0Valid)
            {
                if (t1Valid) return new Vector[] { pt0, pt1 };
                else return new Vector[] { pt0 };
            }
            else if (t1Valid)
                return new Vector[] { pt1 };
            else return new Vector[] { };
        }

        /// <summary>
        /// Find the intersection points, if any exist, for a line and an arc on the XY plane
        /// </summary>
        /// <param name="line">The line</param>
        /// <param name="arc">The arc</param>
        /// <returns></returns>
        public static Vector[] LineArcXY(Line line, Arc arc)
        {
            return LineArcXY(line.StartPoint, line.EndPoint - line.StartPoint, arc, true);
        }

        /// <summary>
        /// Find the intersection points, if any exist, for a portion of a line and an arc on the XY plane
        /// </summary>
        /// <param name="line">The line</param>
        /// <param name="arc">The arc</param>
        /// <param name="lineBounds">The range on the line, as a proportion of the line length, where intersections
        /// will be valid.  0 will represent the start of the line and 1 its end.</param>
        /// <returns></returns>
        public static Vector[] LineArcXY(Line line, Arc arc, Interval lineBounds)
        {
            return LineArcXY(line.StartPoint, line.EndPoint - line.StartPoint, arc, lineBounds);
        }

        /// <summary>
        /// Find the intersection(s) between a curve and an infinite line on the XY plane.
        /// Returns the list of intersection parameters on the curve.
        /// </summary>
        /// <param name="curve">The curve</param>
        /// <param name="lnPt">The origin of the line</param>
        /// <param name="lnDir">The direction of the line</param>
        /// <param name="result">The collection to which results should be added.  
        /// If null, a new collection will be instantiated and returned.</param>
        /// <param name="domainAdjustMin">The start parameter of the curve will be adjusted
        /// to this value in the returned parameters.</param>
        /// <param name="domainAdjustMax">The end parameter of the curve will be adjusted 
        /// to this value in the returned parameters.</param>
        /// <param name="lineBounded">If true, intersections outside of the bounds of the line
        /// running from the origin point to the end of the direction vector will be ignored.</param>
        /// <param name="tLine">Optional.  If supplied, this collection will be populated with the
        /// intersection parameters on the line.</param>
        /// <returns>The list of intersection parameters on the curve</returns>
        public static IList<double> CurveLineXY(Curve curve, Vector lnPt, Vector lnDir, IList<double> result = null, 
            double domainAdjustMin = 0, double domainAdjustMax = 1, bool lineBounded = false, IList<double> tLine = null)
        {
            if (result == null) result = new List<double>();
            if (curve is Line)
            {
                double t0 = -1, t1 = -1;
                LineLineXY(curve.StartPoint, curve.EndPoint - curve.StartPoint, lnPt, lnDir, ref t0, ref t1);
                if (t0 >= 0 && t0 <= 1 && (!lineBounded || t1 >= 0 && t1 <= 1))
                {
                    double t = (domainAdjustMin + t0 * (domainAdjustMax - domainAdjustMin));
                    result.Add(t);
                    if (tLine != null) tLine.Add(t1);
                }
            }
            else if (curve is Arc)
            {
                var arc = (Arc)curve;
                Vector[] intPts = LineArcXY(lnPt, lnDir, arc, lineBounded);
                foreach (var intPt in intPts)
                {
                    double t = arc.ClosestParameter(intPt);
                    t = (domainAdjustMin + t * (domainAdjustMax - domainAdjustMin));
                    result.Add(t);
                    if (tLine != null)
                    {
                        double t1 = Line.ClosestParameter(lnPt, lnPt + lnDir, intPt);
                        tLine.Add(t1);
                    }
                }
            }
            else if (curve is PolyLine)
            {
                var pLine = (PolyLine)curve;
                for (int i = 0; i < pLine.SegmentCount; i++)
                {
                    var v0 = pLine.Vertices[i];
                    var v1 = pLine.Vertices.GetWrapped(i + 1);
                    double tMin = domainAdjustMin + pLine.ParameterAtVertexIndex(i) * (domainAdjustMax - domainAdjustMin);
                    double tMax = domainAdjustMin + pLine.ParameterAtVertexIndex(i+1) * (domainAdjustMax - domainAdjustMin);
                    double t0 = -1, t1 = -1;
                    Vector sPt = v0.Position;
                    Vector ePt = v1.Position;
                    LineLineXY(sPt, ePt - sPt, lnPt, lnDir, ref t0, ref t1);
                    if (t0 >= 0 && t0 <= 1 && (!lineBounded || t1 >= 0 && t1 <= 1))
                    {
                        double t = (tMin + t0 * (tMax - tMin));
                        result.Add(t);
                        if (tLine != null) tLine.Add(t1);
                    }
                }
            }
            else if (curve is PolyCurve)
            {
                var pCrv = (PolyCurve)curve;
                foreach (var subCrv in pCrv.SubCurves)
                {
                    if (subCrv?.Start != null && subCrv.End != null)
                    {
                        double tMin = domainAdjustMin + pCrv.ParameterAt(subCrv.Start) * (domainAdjustMax - domainAdjustMin);
                        double tMax = domainAdjustMin + pCrv.ParameterAt(subCrv.End) * (domainAdjustMax - domainAdjustMin);
                        CurveLineXY(subCrv, lnPt, lnDir, result, tMin, tMax, lineBounded, tLine);
                    }
                }
            }
            else throw new NotSupportedException(
                "The curve type '" + curve.GetType().Name + "' is not supported for XY line intersection operations.");
            //TODO: Implement any other curve types
            return result;
        }

        /// <summary>
        /// Find the intersection(s) between two circles on the XY plane.
        /// </summary>
        /// <param name="pt0">The centre point of the first circle</param>
        /// <param name="r0">The radius of the first circle</param>
        /// <param name="pt1">The centre point of the second circle</param>
        /// <param name="r1">The radius of the second circle</param>
        /// <returns>An array of intersection points, which may contain 0, 1 or 2 points.</returns>
        public static Vector[] CircleCircleXY(Vector pt0, double r0, Vector pt1, double r1)
        {
            double dX = pt1.X - pt0.X;
            double dY = pt1.Y - pt0.Y;
            double distSqd = dX * dX + dY * dY;
            double rsSqd = (r0 + r1).Squared();
            if (distSqd > rsSqd || distSqd <= (r0 - r1).Squared()) // No intersections
                return new Vector[] { };
            else if (distSqd == rsSqd) // One intersection
                return new Vector[] { pt0.Interpolate(pt1, 0.5) };
            else // Two intersections
            {
                Vector midPt = pt0.Interpolate(pt1, 0.5);
                double dist = distSqd.Root();
                double a = (r0.Squared() - r1.Squared() + distSqd) / (2 * dist);
                double b = (r0 * r0 - a * a).Root();
                return new Vector[]
                {
                    new Vector(midPt.X + b * dY/dist, midPt.Y - b * dX/dist),
                    new Vector(midPt.X - b * dY/dist, midPt.Y + b * dX/dist)
                };
            }
        }

        /// <summary>
        /// Find the intersections between two circles on the XY plane.
        /// The provided circles will be assumed to lie on the XY plane even if
        /// they do not.
        /// </summary>
        /// <param name="c0">The first circle on the XY plane</param>
        /// <param name="c1">The second circle on the XY plane</param>
        /// <returns>An array of intersection points, which may contain 0, 1 or 2 points.</returns>
        public static Vector[] CircleCircleXY(Circle c0, Circle c1)
        {
            return CircleCircleXY(c0.Origin, c0.Radius, c1.Origin, c1.Radius);
        }

        /// <summary>
        /// Find the intersections between two arcs on the XY plane.
        /// The provided arcs will be assumed to lie on the XY plane even if
        /// they do not.
        /// </summary>
        /// <param name="arc0">The first arc on the XY plane</param>
        /// <param name="arc1">The second arc on the XY plane</param>
        /// <returns>An array of intersection points, which may contain 0, 1 or 2 points.</returns>
        public static Vector[] ArcArcXY(Arc arc0, Arc arc1)
        {
            return ArcArcXY(arc0, arc1, Interval.Unit);
        }

        /// <summary>
        /// Find the intersections between two arcs on the XY plane.
        /// The provided arcs will be assumed to lie on the XY plane even if
        /// they do not.
        /// </summary>
        /// <param name="arc0">The first arc on the XY plane</param>
        /// <param name="arc1">The second arc on the XY plane</param>
        /// <returns>An array of intersection points, which may contain 0, 1 or 2 points.</returns>
        public static Vector[] ArcArcXY(Arc arc0, Arc arc1, Interval firstArcBounds)
        {
            Vector[] v = CircleCircleXY(arc0.Circle, arc1.Circle);

            // Check that the points found lie on both of the arcs:

            bool v0Valid = (v.Length > 0 && arc0.IsInAngleRange(v[0], firstArcBounds) && arc1.IsInAngleRange(v[0]));
            bool v1Valid = (v.Length > 1 && arc0.IsInAngleRange(v[1], firstArcBounds) && arc1.IsInAngleRange(v[1]));

            if (v0Valid)
            {
                if (v1Valid) return new Vector[] { v[0], v[1] };
                else return new Vector[] { v[0] };
            }
            else if (v1Valid)
                return new Vector[] { v[1] };
            else return new Vector[] { };
        }

        /// <summary>
        /// Find the intersections between two simple curves on the XY plane.
        /// </summary>
        /// <param name="crv0">The first curve</param>
        /// <param name="crv1">The second curve</param>
        /// <param name="endStartTolerance">The tolerance distance from the end of the first curve 
        /// within which intersections with the second will be ignored.  Enables self-intersection checks without
        /// false positives from matching curve ends.</param>
        /// <returns>An array of intersection points, which may contain 0, 1 or 2 points.</returns>
        public static Vector[] CurveCurveXY(ISimpleCurve crv0, ISimpleCurve crv1, double endStartTolerance = 0)
        {
            if (crv0 is Line)
            {
                if (crv1 is Line)
                {
                    Vector v = LineLineXY((Line)crv0, (Line)crv1,  new Interval(0, 1 - endStartTolerance));
                    if (v.IsValid()) return new Vector[] { v };
                    else return new Vector[] { };
                }
                else if (crv1 is Arc)
                {
                    return LineArcXY((Line)crv0, (Arc)crv1, new Interval(0, 1 - endStartTolerance));
                }
            }
            else if (crv0 is Arc)
            {
                if (crv1 is Line) return LineArcXY((Line)crv1, (Arc)crv0, new Interval(endStartTolerance, 1));
                else if (crv1 is Arc) return ArcArcXY((Arc)crv0, (Arc)crv1, new Interval(0, 1 - endStartTolerance));
            }
            throw new NotImplementedException("Intersection between curve types '" + crv0.GetType().Name +
                "' and '" + crv1.GetType().Name + "' cannot be resolved.");
        }

        /// <summary>
        /// Find the intersection between an infinite line and an infinite plane.
        /// Expressed as a parameter t, the multiple of the lineDirection away from the lineOrigin
        /// at which the intersection takes place.  If no intersection (i.e. the line is parallel
        /// to the plane) will return double.NaN.
        /// </summary>
        /// <param name="lineOrigin">A point on the line</param>
        /// <param name="lineDirection">A direction vector for the line</param>
        /// <param name="planeOrigin">A point on the plane</param>
        /// <param name="planeNormal">A direction vector perpendicular to the plane</param>
        /// <returns></returns>
        public static double LinePlane(Vector lineOrigin, Vector lineDirection, Vector planeOrigin, Vector planeNormal)
        {
            double directionProj = lineDirection.Dot(planeNormal);

            if (directionProj != 0)
            {
                double startProj = lineOrigin.Dot(planeNormal);
                double pointProj = planeOrigin.Dot(planeNormal);
                double t = (pointProj - startProj) / directionProj;
                return t;
            }
            return double.NaN;
        }

        /// <summary>
        /// Find the intersection between an infinite line and an infinite plane normal to one
        /// of the primary global axes.
        /// Expressed as a parameter t, the multiple of the lineDirection away from the lineOrigin
        /// at which the intersection takes place.  If no intersection (i.e. the line is parallel
        /// to the plane) will return double.NaN.
        /// </summary>
        /// <param name="lineOrigin">A point on the line</param>
        /// <param name="lineDirection">A direction vector for the line</param>
        /// <param name="dimension">The dimensional axis normal to the plane 
        /// (for e.g. entering Dimension.Z would utilise a XY plane)</param>
        /// <param name="planePosition">The position of the plane in the specified dimension</param>
        /// <returns></returns>
        public static double LinePlane(Vector lineOrigin, Vector lineDirection, CoordinateAxis dimension, double planePosition)
        {
            if (dimension == CoordinateAxis.X) return LineYZPlane(lineOrigin, lineDirection, planePosition);
            else if (dimension == CoordinateAxis.Y) return LineXZPlane(lineOrigin, lineDirection, planePosition);
            else if (dimension == CoordinateAxis.Z) return LineXYPlane(lineOrigin, lineDirection, planePosition);
            else return double.NaN;
        }

        /// <summary>
        /// Find the intersection between an infinite line and an infinite plane aligned
        /// parallel to the global XY axes.
        /// Expressed as a parameter t, the multiple of the lineDirection away from the lineOrigin
        /// at which the intersection takes place.  If no intersection (i.e. the line is parallel
        /// to the plane) will return double.NaN.
        /// </summary>
        /// <param name="lineOrigin">A point on the line</param>
        /// <param name="lineDirection">A direction vector for the line</param>
        /// <param name="x">The z-coordinate of the plane.</param>
        /// <returns></returns>
        public static double LineXYPlane(Vector lineOrigin, Vector lineDirection, double z)
        {
            return lineDirection.Z == 0 ? double.NaN : (z - lineOrigin.Z) / lineDirection.Z;
        }

        /// <summary>
        /// Find the intersection between an infinite line and an infinite plane aligned
        /// parallel to the global XZ axes.
        /// Expressed as a parameter t, the multiple of the lineDirection away from the lineOrigin
        /// at which the intersection takes place.  If no intersection (i.e. the line is parallel
        /// to the plane) will return double.NaN.
        /// </summary>
        /// <param name="lineOrigin">A point on the line</param>
        /// <param name="lineDirection">A direction vector for the line</param>
        /// <param name="y">The y-coordinate of the plane.</param>
        /// <returns></returns>
        public static double LineXZPlane(Vector lineOrigin, Vector lineDirection, double y)
        {
            return lineDirection.Y == 0 ? double.NaN : (y - lineOrigin.Y) / lineDirection.Y;
        }

        /// <summary>
        /// Find the intersection between an infinite line and an infinite plane aligned
        /// parallel to the global YZ axes.
        /// Expressed as a parameter t, the multiple of the lineDirection away from the lineOrigin
        /// at which the intersection takes place.  If no intersection (i.e. the line is parallel
        /// to the plane) will return double.NaN.
        /// </summary>
        /// <param name="lineOrigin">A point on the line</param>
        /// <param name="lineDirection">A direction vector for the line</param>
        /// <param name="x">The x-coordinate of the plane.</param>
        /// <returns></returns>
        public static double LineYZPlane(Vector lineOrigin, Vector lineDirection, double x)
        {
            return lineDirection.X == 0 ? double.NaN : (x - lineOrigin.X) / lineDirection.X;
        }


        /// <summary>
        /// Calculate the intersection between a ray and a triangle using the
        /// Möller–Trumbore algorithm.  See:
        /// https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm.
        /// Returns the parameter t, being the multiplication of the rayDirection from the rayOrigin.
        /// If there is no intersection between the ray and the triangle, double.NaN will be returned.
        /// </summary>
        /// <param name="rayOrigin">The origin point of the ray</param>
        /// <param name="rayDirection">The direction of the ray.</param>
        /// <param name="tri0">The first corner of the triangle</param>
        /// <param name="tri1">The second corner of the triangle</param>
        /// <param name="tri2">The third corner of the triangle</param>
        /// <returns></returns>
        public static double RayTriangle(ref Vector rayOrigin, ref Vector rayDirection, ref Vector tri0, ref Vector tri1, ref Vector tri2)
        {
            double tolerance = 0.0000001;
            Vector edge1 = tri1 - tri0;
            Vector edge2 = tri2 - tri0;
            Vector h = rayDirection.Cross(edge2);
            double a = edge1.Dot(h);
            if (a > -tolerance && a < tolerance) return double.NaN;
            double f = 1.0 / a;
            Vector s = rayOrigin - tri0;
            double u = f * s.Dot(h);
            if (u < 0.0 || u > 1.0) return double.NaN;
            Vector q = s.Cross(edge1);
            double v = f * rayDirection.Dot(q);
            if (v < 0.0 || u + v > 1.0) return double.NaN;
            return f * edge2.Dot(q);
        }

        /// <summary>
        /// Calculate the intersection between a ray and a triangle using the
        /// Möller–Trumbore algorithm.  See:
        /// https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm.
        /// Returns the parameter t, being the multiplication of the rayDirection from the rayOrigin.
        /// If there is no intersection between the ray and the triangle, double.NaN will be returned.
        /// </summary>
        /// <param name="rayOrigin">The origin point of the ray</param>
        /// <param name="rayDirection">The direction of the ray.</param>
        /// <param name="tri0">The first corner of the triangle</param>
        /// <param name="tri1">The second corner of the triangle</param>
        /// <param name="tri2">The third corner of the triangle</param>
        /// <returns></returns>
        public static double RayTriangle(Vector rayOrigin, Vector rayDirection, Vector tri0, Vector tri1, Vector tri2)
        {
            double tolerance = 0.0000001;
            Vector edge1 = tri1 - tri0;
            Vector edge2 = tri2 - tri0;
            Vector h = rayDirection.Cross(ref edge2);
            double a = edge1.Dot(ref h);
            if (a > -tolerance && a < tolerance) return double.NaN;
            double f = 1.0 / a;
            Vector s = rayOrigin - tri0;
            double u = f * s.Dot(ref h);
            if (u < 0.0 || u > 1.0) return double.NaN;
            Vector q = s.Cross(ref edge1);
            double v = f * rayDirection.Dot(ref q);
            if (v < 0.0 || u + v > 1.0) return double.NaN;
            return f * edge2.Dot(ref q);
        }

        /// <summary>
        /// Calculate the intersection between a ray and a mesh face.
        /// Returns the parameter t, being the multiplication of the rayDirection from the rayOrigin.
        /// If there is no intersection between the ray and the face, double.NaN will be returned.
        /// </summary>
        /// <param name="rayOrigin">The origin point of the ray.</param>
        /// <param name="rayDirection">The direction of the ray.</param>
        /// <param name="face">The mesh face.</param>
        /// <returns></returns>
        public static double RayFace(Vector rayOrigin, Vector rayDirection, IList<Vertex> face)
        {
            if (face.Count < 3) return double.NaN;
            Vector vA = face[0].Position;
            for (int i = 0; i < face.Count - 2; i++)
            {
                Vector vB = face[i + 1].Position;
                Vector vC = face[i + 2].Position;
                double t = RayTriangle(ref rayOrigin, ref rayDirection, ref vA, ref vB, ref vC);
                if (!t.IsNaN()) return t;
            }
            return double.NaN;
        }

        /// <summary>
        /// Calculate the intersection between a ray and a mesh face.
        /// Returns the parameter t, being the multiplication of the ray Direction from the ray Origin.
        /// If there is no intersection between the ray and the face, double.NaN will be returned.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="face">The mesh face.</param>
        /// <returns></returns>
        public static double RayFace(Axis ray, IList<Vertex> face)
        {
            return RayFace(ray.Origin, ray.Direction, face);
        }

        /// <summary>
        /// Calculate the intersection between a ray and a mesh.
        /// Returns the parameter t, being the multiplication of the rayDirection from the rayOrigin.
        /// If there is no intersection between the ray and the triangle, double.NaN will be returned.
        /// </summary>
        /// <param name="rayOrigin">The origin point of the ray.</param>
        /// <param name="rayDirection">The direction of the ray.</param>
        /// <param name="mesh">The mesh.</param>
        /// <returns></returns>
        public static double RayMesh(Vector rayOrigin, Vector rayDirection, Mesh mesh)
        {
            double t = double.NaN;
            foreach (MeshFace face in mesh.Faces)
            {
                double t2 = RayFace(rayOrigin, rayDirection, face);
                if (!t2.IsNaN() && (t.IsNaN() || (t < 0 && t2 > t) || (t2 > 0 && t2 < t)))
                    t = t2;
            }
            return t;
        }

        /// <summary>
        /// Calculate the intersection between a ray and a mesh.
        /// Returns the parameter t, being the multiplication of the ray Direction from the ray Origin.
        /// If there is no intersection between the ray and the triangle, double.NaN will be returned.
        /// </summary>
        /// <param name="ray">The ray</param>
        /// <param name="mesh">The mesh</param>
        /// <returns></returns>
        public static double RayMesh(Axis ray, Mesh mesh)
        {
            return RayMesh(ray.Origin, ray.Direction, mesh);
        }

        /// <summary>
        /// Find the intersection point between a ray half-line and a line segment on the XY plane, if one exists
        /// </summary>
        /// <param name="rayStart">The ray start point</param>
        /// <param name="rayDir">The ray direction</param>
        /// <param name="segStart">The start of the line segment</param>
        /// <param name="segEnd">The end of the line segment</param>
        /// <returns></returns>
        public static Vector RayLineSegmentXY(ref Vector rayStart, ref Vector rayDir, ref Vector segStart, ref Vector segEnd)
        {
            Vector segDir = segEnd - segStart;
            Vector result = LineLineXY(rayStart, rayDir, segStart, segDir);
            if (result.IsValid())
            {
                //Check intersection lies within segment:
                if ((segDir.Y > 0 && result.Y >= segStart.Y && result.Y < segEnd.Y)
                    || (segDir.Y < 0 && result.Y >= segEnd.Y && result.Y < segStart.Y)
                    || (segDir.Y == 0 && result.X >= Math.Min(segStart.X,segEnd.X) && result.X < Math.Max(segStart.X, segEnd.X)))
                {
                    //Check intersection is not 'behind' ray origin:
                    int rayXSign = rayDir.X.Sign();
                    if (rayXSign == (result.X - rayStart.X).Sign() && (rayXSign != 0 || rayDir.Y.Sign() == (result.Y - rayStart.Y).Sign()))
                    {
                        return result;
                    }
                }
            }

            return Vector.Unset;
        }

        /// <summary>
        /// Find the intersection point between two line segments on the XY plane, if one exists
        /// </summary>
        /// <param name="startA">The start of the first line segment</param>
        /// <param name="endA">The end of the first line segment</param>
        /// <param name="startB">The start of the second line segment</param>
        /// <param name="endB">The end of the second line segment</param>
        /// <returns></returns>
        public static Vector LineSegmentsXY(ref Vector startA, ref Vector endA, ref Vector startB, ref Vector endB)
        {
            Vector dirA = endA - startA;
            Vector dirB = endB - startB;
            Vector result = LineLineXY(startA, dirA, startB, dirB);
            if (result.IsValid())
            {
                //Check intersection lies within segments A & B:
                if ((dirA.Y > 0 && result.Y >= startA.Y && result.Y < endA.Y)
                    || (dirA.Y < 0 && result.Y >= endA.Y && result.Y < startA.Y)
                    || (dirA.Y == 0 && result.X >= Math.Min(startA.X, endA.X) && result.X < Math.Max(startA.X, endA.X))
                    && (dirB.Y > 0 && result.Y >= startB.Y && result.Y < endB.Y)
                    || (dirB.Y < 0 && result.Y >= endB.Y && result.Y < startB.Y)
                    || (dirB.Y == 0 && result.X >= Math.Min(startB.X, endB.X) && result.X < Math.Max(startB.X, endB.X)))
                {
                    return result;
                }
            }

            return Vector.Unset;
        }


        /// <summary>
        /// A quick check on whether a half-line starting at the specified point and travelling parallel to the X-axis
        /// will intersect the specified segment on the XY plane.  Used in planar containment testing.
        /// </summary>
        /// <param name="rayStart"></param>
        /// <param name="segStart"></param>
        /// <param name="segEnd"></param>
        /// <returns></returns>
        public static bool XRayLineSegmentXYCheck(ref Vector rayStart, ref Vector segStart, ref Vector segEnd, out bool onLine)
        {
            onLine = false;
            if (segStart.X < rayStart.X)
            {
                if (segEnd.X < rayStart.X) return false; // Segment to the left!
                // Ray start falls within segment bounds - need to do additional check on whether
                // ray start is to the right or left
                else if (((segStart.Y >= rayStart.Y && segEnd.Y < rayStart.Y)
                || (segStart.Y < rayStart.Y && segEnd.Y >= rayStart.Y)))
                {
                    double dist = segStart.X + (segEnd.X - segStart.X) * ((rayStart.Y - segStart.Y) / (segEnd.Y - segStart.Y)) - rayStart.X;
                    if (dist >= 0)
                    {
                        //if (dist == 0) onLine = true;
                        return true;
                    }
                }
                 return false;
            }
            else if (segEnd.X < rayStart.X) //Ray start falls within segment bounds
            {
                if (((segStart.Y >= rayStart.Y && segEnd.Y < rayStart.Y)
                || (segStart.Y < rayStart.Y && segEnd.Y >= rayStart.Y)))
                {
                    double dist = segStart.X + (segEnd.X - segStart.X) * ((rayStart.Y - segStart.Y) / (segEnd.Y - segStart.Y)) - rayStart.X;
                    if (dist >= 0)
                    {
                        //if (dist == 0) onLine = true;
                        return true;
                    }
                }
                return false;
            }
            //Segment is to the right of ray start
            else if ((segStart.Y >= rayStart.Y && segEnd.Y < rayStart.Y)
                || (segStart.Y <= rayStart.Y && segEnd.Y > rayStart.Y)
                || (rayStart.Y == segStart.Y && rayStart.Y == segEnd.Y))
            {
                //if (segStart.X == rayStart.X && segEnd.X == rayStart.X) onLine = true;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Find the self-intersection parameters of a polycurve
        /// on the XY plane.
        /// Returns a sorted list where the keys are the intersection
        /// parameters along this curve and the values are their matching
        /// parameters.
        /// </summary>
        /// <param name="polyCurve"></param>
        /// <returns></returns>
        public static SortedList<double, double> SelfXY(PolyCurve polyCurve)
        {
            return polyCurve.SelfIntersectionsXY();
        }

        /// <summary>
        /// Calculate the distance by which an offset of a line might be extended until it intersects
        /// with another offset line at an angle to it.  
        /// </summary>
        /// <param name="angle">The angle between line B and line A</param>
        /// <param name="offsetA">The offset of line A</param>
        /// <param name="offsetB">The offset of line B</param>
        /// <returns></returns>
        public static double OffsetExtensionDistance(Angle angle, double offsetA, double offsetB)
        {
            return (offsetA - (offsetB/Math.Cos(angle))) / Math.Tan(angle);
        }

        /// <summary>
        /// Find the section(s) of a curve which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="crv">The curve</param>
        /// <param name="polygon">The vertices which define the containment polygon</param>
        /// <param name="result">Optional.  A collection to which the results should be added.  If null,
        /// a new collection will be instantiated and returned instead.</param>
        /// <param name="remapStart">Optional. The start of the domain that results should be remapped into.</param>
        /// <param name="remapEnd">Optional.  The end of the domain that results should be remapped into.</param>
        /// <returns></returns>
        public static IList<Interval> CurveDomainInPolygonXY(Curve crv, IList<Vertex> polygon, IList<Interval> result = null,
            double remapStart = 0, double remapEnd = 1)
        {
            if (result == null) result = new List<Interval>();
            if (crv == null) return result;
            else if (crv is Line)
                return LineDomainInPolygonXY((Line)crv, polygon, result, remapStart, remapEnd);
            else if (crv is Arc)
                return ArcDomainInPolygonXY((Arc)crv, polygon, result, remapStart, remapEnd);
            else if (crv is PolyLine)
                return PolyLineDomainInPolygonXY((PolyLine)crv, polygon, result, remapStart, remapEnd);
            else if (crv is PolyCurve)
                return PolyCurveDomainInPolygonXY((PolyCurve)crv, polygon, result, remapStart, remapEnd);
            else
                throw new NotImplementedException(); //New curve types need to go here!
        }

        /// <summary>
        /// Find the section(s) of a curve which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="crv">The curve</param>
        /// <param name="polygon">The vertices which define the containment polygon</param>
        /// <param name="result">Optional.  A collection to which the results should be added.  If null,
        /// a new collection will be instantiated and returned instead.</param>
        /// <returns></returns>
        public static CurveCollection CurveInPolygonXY(Curve crv, IList<Vertex> polygon, CurveCollection result = null)
        {
            if (result == null) result = new CurveCollection();
            if (crv == null) return result;
            else if (crv is Line)
                return LineInPolygonXY((Line)crv, polygon, result);
            else if (crv is Arc)
                return ArcInPolygonXY((Arc)crv, polygon, result);
            else if (crv is PolyLine)
                return PolyLineInPolygonXY((PolyLine)crv, polygon, result);
            else if (crv is PolyCurve)
                return PolyCurveInPolygonXY((PolyCurve)crv, polygon, result);
            else
                throw new NotImplementedException(); //New curve types need to go here!
        }

        /// <summary>
        /// Find the section(s) of a polycurve which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="pCurve">The polycurve</param>
        /// <param name="polygon">The vertices which define the polygon</param>
        /// <param name="result">Optional.  A collection to which the results should be added.  If null,
        /// a new collection will be created and returned.</param>
        /// <param name="remapStart">Optional. The start of the domain that results should be remapped into.</param>
        /// <param name="remapEnd">Optional.  The end of the domain that results should be remapped into.</param>
        /// <returns></returns>
        public static IList<Interval> PolyCurveDomainInPolygonXY(PolyCurve pCurve, IList<Vertex> polygon, IList<Interval> result = null,
            double remapStart = 0, double remapEnd = 1)
        {
            if (result == null) result = new List<Interval>();
            int span = 0;
            foreach (var crv in pCurve.SubCurves)
            {
                int endSpan = span + crv.SegmentCount;
                double t0 = pCurve.ParameterAt(span, 0).Remap(remapStart, remapEnd);
                double t1 = pCurve.ParameterAt(endSpan - 1, 1).Remap(remapStart, remapEnd);
                CurveDomainInPolygonXY(crv, polygon, result, t0, t1);
                span = endSpan;
            }
            return result;
        }

        /// <summary>
        /// Find the section(s) of a polycurve which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="pCurve">The polycurve</param>
        /// <param name="polygon">The vertices which define the polygon</param>
        /// <param name="result">Optional.  A collection to which the results should be added.  If null,
        /// a new collection will be created and returned.</param>
        /// <returns></returns>
        public static CurveCollection PolyCurveInPolygonXY(PolyCurve pCurve, IList<Vertex> polygon, CurveCollection result = null)
        {
            if (result == null) result = new CurveCollection();
            foreach (var crv in pCurve.SubCurves)
            {
                CurveInPolygonXY(crv, polygon, result);
            }
            return result.JoinOrderedCurves();
        }

        /// <summary>
        /// Find the section(s) of a polyline which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="pLine"></param>
        /// <param name="polygon"></param>
        /// <param name="result"></param>
        /// <param name="remapStart">Optional. The start of the domain that results should be remapped into.</param>
        /// <param name="remapEnd">Optional.  The end of the domain that results should be remapped into.</param>
        /// <returns></returns>
        public static IList<Interval> PolyLineDomainInPolygonXY(PolyLine pLine, IList<Vertex> polygon, IList<Interval> result = null,
            double remapStart = 0, double remapEnd = 1)
        {
            if (result == null) result = new List<Interval>();
            var lines = pLine.ToLines();
            for (int i = 0; i < pLine.SegmentCount; i++)
            {
                Line line = lines[i];
                double t0 = pLine.ParameterAt(i, 0).Remap(remapStart, remapEnd);
                double t1 = pLine.ParameterAt(i, 1).Remap(remapStart, remapEnd);
                LineDomainInPolygonXY(line, polygon, result, t0,t1);
            }
            return result;
        }

        /// <summary>
        /// Find the section(s) of a polyline which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="pLine"></param>
        /// <param name="polygon"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CurveCollection PolyLineInPolygonXY(PolyLine pLine, IList<Vertex> polygon, CurveCollection result = null)
        {
            if (result == null) result = new CurveCollection();
            foreach (var line in pLine.ToLines())
            {
                LineInPolygonXY(line, polygon, result);
            }
            return result.JoinOrderedCurves();
        }

        /// <summary>
        /// Find the section(s) of an arc which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="polygon"></param>
        /// <param name="remapStart">Optional. The start of the domain that results should be remapped into.</param>
        /// <param name="remapEnd">Optional.  The end of the domain that results should be remapped into.</param>
        /// <returns></returns>
        public static IList<Interval> ArcDomainInPolygonXY(Arc arc, IList<Vertex> polygon, IList<Interval> result = null,
            double remapStart = 0, double remapEnd = 1)
        {
            if (result == null) result = new List<Interval>();
            double tolerance = Tolerance.Distance;
            var intersections = new SortedList<double, Vector>();

            Vector ptL = arc.StartPoint;
            Vector vL = arc.EndPoint - ptL;

            for (int i = 0; i < polygon.Count; i++) // Loop through polygon's edges
            {
                Vertex vP0 = polygon[i];
                Vertex vP1 = polygon.GetWrapped(i + 1);
                Vector ptP = vP0.Position;
                Vector vP = vP1.Position - ptP;
                if (!vP.IsZero())
                {
                    //double t0 = 0;
                    //double t1 = 0;
                    Vector[] iPts = LineArcXY(ptP, vP, arc, true);
                    //LineLineXY(ptL, vL, ptP, vP, ref t0, ref t1); // Find infinite line intersection
                    if (iPts.Length > 0)
                    // && t0 >= -tolerance && t0 <= 1 + tolerance && t1 >= -tolerance && t1 <= 1 + tolerance)
                    {
                        foreach (var iPt in iPts)
                        {
                            double t0 = arc.ClosestParameter(iPt);
                            if (intersections.ContainsKey(t0)) intersections.Remove(t0);
                            //if we have two intersections at the same point ignore both of them!
                            else intersections.Add(t0, iPt);
                        }
                    }
                }
            }

            int j = 0;
            if (polygon.PolygonContainmentXY(arc.StartPoint))
            {
                if (intersections.Count == 0) result.Add(new Interval(remapStart,remapEnd)); // Input line is wholly inside polygon
                else
                {
                    result.Add(new Interval(remapStart, intersections.Keys[0].Remap(remapStart,remapEnd)));
                    //new Line(line.StartPoint, intersections.Values[0]));
                    j += 1;
                }
            }
            for (int i = j; i < intersections.Count; i += 2)
            {
                double tEnd;//Vector endPt;
                if (i + 1 < intersections.Count) tEnd = intersections.Keys[i + 1].Remap(remapStart,remapEnd);
                else tEnd = remapEnd;

                result.Add(new Interval(intersections.Keys[i].Remap(remapStart, remapEnd),tEnd));
                //new Line(intersections.Values[i], endPt));
            }

            return result;
        }

        /// <summary>
        /// Find the section(s) of an arc which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="arc">The arc</param>
        /// <param name="polygon">The polgon vertices</param>
        /// <param name="result">Optional.  A collection to which results should be added.  If null,
        /// a new collection will be created and returned.</param>
        /// <returns></returns>
        public static CurveCollection ArcInPolygonXY(Arc arc, IList<Vertex> polygon, CurveCollection result = null)
        {
            if (result == null) result = new CurveCollection();
            double tolerance = Tolerance.Distance;
            var intersections = new SortedList<double, Vector>();

            Vector ptL = arc.StartPoint;
            Vector vL = arc.EndPoint - ptL;

            for (int i = 0; i < polygon.Count; i++) // Loop through polygon's edges
            {
                Vertex vP0 = polygon[i];
                Vertex vP1 = polygon.GetWrapped(i + 1);
                Vector ptP = vP0.Position;
                Vector vP = vP1.Position - ptP;
                if (!vP.IsZero())
                {
                    //double t0 = 0;
                    //double t1 = 0;
                    Vector[] iPts = LineArcXY(ptP, vP, arc, true);
                    //LineLineXY(ptL, vL, ptP, vP, ref t0, ref t1); // Find infinite line intersection
                    if (iPts.Length > 0)
                        // && t0 >= -tolerance && t0 <= 1 + tolerance && t1 >= -tolerance && t1 <= 1 + tolerance)
                    {
                        foreach (var iPt in iPts)
                        {
                            double t0 = arc.ClosestParameter(iPt);
                            if (intersections.ContainsKey(t0)) intersections.Remove(t0); 
                            //if we have two intersections at the same point ignore both of them!
                            else intersections.Add(t0, iPt);
                        }
                    }
                }
            }

            int j = 0;
            if (polygon.PolygonContainmentXY(arc.StartPoint))
            {
                if (intersections.Count == 0) result.Add(arc); // Input line is wholly inside polygon
                else
                {
                    result.Add(arc.Extract(new Interval(0, intersections.Keys[0])));
                    //new Line(line.StartPoint, intersections.Values[0]));
                    j += 1;
                }
            }
            for (int i = j; i < intersections.Count; i += 2)
            {
                double tEnd;//Vector endPt;
                if (i + 1 < intersections.Count) tEnd = intersections.Keys[i + 1];
                else tEnd = 1;

                result.Add(arc.Extract(new Interval(intersections.Keys[i], tEnd)));
                    //new Line(intersections.Values[i], endPt));
            }

            return result;
        }

        /// <summary>
        /// Find the section(s) of a line which lie(s) inside the specified polygon,
        /// expressed as parameter domains along the line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="polygon"></param>
        /// <param name="result"></param>
        /// <param name="remapStart">Optional. The start of the domain that results should be remapped into.</param>
        /// <param name="remapEnd">Optional.  The end of the domain that results should be remapped into.</param>
        /// <returns></returns>
        public static IList<Interval> LineDomainInPolygonXY(Line line, IList<Vertex> polygon, IList<Interval> result = null, double remapStart = 0, double remapEnd = 1)
        {
            if (result == null) result = new List<Interval>();
            double tolerance = Tolerance.Distance;
            var intersections = new SortedList<double, Vector>();

            Vector ptL = line.StartPoint;
            Vector vL = line.EndPoint - ptL;

            for (int i = 0; i < polygon.Count; i++) // Loop through polygon's edges
            {
                Vertex vP0 = polygon[i];
                Vertex vP1 = polygon.GetWrapped(i + 1);
                Vector ptP = vP0.Position;
                Vector vP = vP1.Position - ptP;
                if (!vP.IsZero())
                {
                    double t0 = 0;
                    double t1 = 0;
                    Vector iPt = LineLineXY(ptL, vL, ptP, vP, ref t0, ref t1); // Find infinite line intersection
                    if (iPt.IsValid() && t0 >= -tolerance && t0 <= 1 + tolerance && t1 >= -tolerance && t1 <= 1 + tolerance)
                    {
                        if (intersections.ContainsKey(t0)) intersections.Remove(t0); //if we have two intersections at the same point ignore both of them!
                        else intersections.Add(t0, iPt);
                    }
                }
            }

            int j = 0;
            if (polygon.PolygonContainmentXY(line.StartPoint))
            {
                if (intersections.Count == 0) result.Add(new Interval(remapStart,remapEnd)); // Input line is wholly inside polygon
                else if (!line.StartPoint.Equals(intersections.Values[0])) //Check the start point isn't *on* the intersection
                {
                    result.Add(new Interval(remapStart, intersections.Keys[0].Remap(remapStart,remapEnd)));
                    //new Line(line.StartPoint, intersections.Values[0]));
                    j += 1;
                }
                else if (intersections.Count > 1)
                {
                    var testPt = line.StartPoint.Interpolate(intersections.Values[1], 0.5); // Test midway to the next intersection
                    if (!polygon.PolygonContainmentXY(testPt)) // Check if the line starts on the edge but heads outside
                    {
                        j += 1; //Skip to next segment
                    }
                }
                else
                {
                    // Does the line start on the edge but then the whole of the rest of the line is outside?
                    if (!polygon.PolygonContainmentXY(line.EndPoint))
                    {
                        j += 1; //Skip to end
                    }
                }

            }
            for (int i = j; i < intersections.Count; i += 2)
            {
                double tEnd;
                if (i + 1 < intersections.Count) tEnd = intersections.Keys[i + 1].Remap(remapStart, remapEnd);
                else tEnd = remapEnd;

                result.Add(new Interval(intersections.Keys[i].Remap(remapStart, remapEnd), tEnd));
                //new Line(intersections.Values[i], endPt));
            }

            return result;
        }



        /// <summary>
        /// Find the section(s) of a line which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="line"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static CurveCollection LineInPolygonXY(Line line, IList<Vertex> polygon, CurveCollection result = null)
        {
            if (result == null) result = new CurveCollection();
            double tolerance = Tolerance.Distance;
            var intersections = new SortedList<double, Vector>();
            
            Vector ptL = line.StartPoint;
            Vector vL = line.EndPoint - ptL;

            for (int i = 0; i < polygon.Count; i++) // Loop through polygon's edges
            {
                Vertex vP0 = polygon[i];
                Vertex vP1 = polygon.GetWrapped(i + 1);
                Vector ptP = vP0.Position;
                Vector vP = vP1.Position - ptP;
                if (!vP.IsZero())
                {
                    double t0 = 0;
                    double t1 = 0;
                    Vector iPt = LineLineXY(ptL, vL, ptP, vP, ref t0, ref t1); // Find infinite line intersection
                    if (iPt.IsValid() && t0 >= -tolerance && t0 <= 1 + tolerance && t1 >= -tolerance && t1 <= 1 + tolerance)
                    {
                        if (intersections.ContainsKey(t0)) intersections.Remove(t0); //if we have two intersections at the same point ignore both of them!
                        else intersections.Add(t0, iPt);
                    }
                }
            }

            int j = 0;
            if (polygon.PolygonContainmentXY(line.StartPoint))
            {
                if (intersections.Count == 0) result.Add(line); // Input line is wholly inside polygon
                else if (!line.StartPoint.Equals(intersections.Values[0])) //Check the start point isn't *on* the intersection
                {
                    result.Add(new Line(line.StartPoint, intersections.Values[0]));
                    j += 1;
                }
                else if (intersections.Count > 1)
                {
                    var testPt = line.StartPoint.Interpolate(intersections.Values[1], 0.5); // Test midway to the next intersection
                    if (!polygon.PolygonContainmentXY(testPt)) // Check if the line starts on the edge but heads outside
                    {
                        j += 1; //Skip to next segment
                    }
                }
                else
                {
                    // Does the line start on the edge but then the whole of the rest of the line is outside?
                    if (!polygon.PolygonContainmentXY(line.EndPoint))
                    {
                        j += 1; //Skip to end
                    }
                }
                
            }
            for (int i = j; i < intersections.Count; i += 2)
            {
                Vector endPt;
                if (i + 1 < intersections.Count) endPt = intersections.Values[i + 1];
                else endPt = line.EndPoint;

                result.Add(new Line(intersections.Values[i], endPt));
            }

            return result;
        }

        /// <summary>
        /// Find the overlapping region(s) between two polygons, represented as sets of vertices.
        /// Uses an algorithm similar to that presented in Efficient Clipping of Arbitrary Polygons
        /// by Gunter Greiner and Kai Hormann: http://davis.wpi.edu/~matt/courses/clipping/.
        /// The returned polygons will be composed of the vertices of the previous polygons plus additional
        /// ones at the intersection points.  Note that you may need to create copies of these if the
        /// pre-existing vertices already form part of a separate geometry object.
        /// </summary>
        /// <param name="polygonA">The set of vertices representing the first polygon</param>
        /// <param name="polygonB">The set of vertices representing the second polygon</param>
        /// <param name="allVertices">Optional.  The collection of vertices, to which any vertices created during this process should be added</param>
        /// <returns></returns>
        public static IList<TPolygon> PolygonOverlapXY<TPolygon>(IList<Vertex> polygonA, IList<Vertex> polygonB, IList<Vertex> allVertices = null)
            where TPolygon: class, IList<Vertex>, new()
        {
            double tolerance = 0.000001;
            List<TPolygon> result = null;

            if (polygonA.Count > 0 && polygonB.Count > 0)
            {
                // Build sorted lists of intersections between A and B:

                var intersectionsA = new SortedList<double, LineLineIntersection>();
                var intersectionsB = new SortedList<double, LineLineIntersection>();

                bool inside = polygonB.PolygonContainmentXY(polygonA[0].Position);

                for (int i = 0; i < polygonA.Count; i++) // Loop through A's edges
                {
                    Vertex vA0 = polygonA[i];
                    Vertex vA1 = polygonA.GetWrapped(i + 1);
                    Vector pt0 = vA0.Position;
                    Vector v0 = vA1.Position - pt0;
                    if (!v0.IsZero())
                    {
                        for (int j = 0; j < polygonB.Count; j++) // Loop through B's edges
                        {
                            Vertex vB0 = polygonB[j];
                            Vertex vB1 = polygonB.GetWrapped(j + 1);
                            Vector pt1 = vB0.Position;
                            Vector v1 = vB1.Position - pt1;
                            if (!v1.IsZero())
                            {
                                double t0 = 0;
                                double t1 = 0;
                                Vector iPt = LineLineXY(pt0, v0, pt1, v1, ref t0, ref t1); // Find infinite line intersection
                                if (iPt.IsValid() && t0 >= -tolerance && t0 <= 1 + tolerance && t1 >= -tolerance && t1 <= 1 + tolerance)
                                {
                                    // Intersection lies within line segments - we have a genuine intersection
                                    LineLineIntersection intersect = new LineLineIntersection();
                                    if (t0 <= tolerance)
                                    {
                                        intersect.Vertex = vA0;
                                    }
                                    else if (t1 <= tolerance)
                                    {
                                        intersect.Vertex = vB0;
                                    }
                                    else
                                    {
                                        Vertex newVertex = new Vertex(iPt);
                                        intersect.Vertex = newVertex;
                                        if (allVertices != null) allVertices.Add(newVertex);
                                    }
                                    intersect.At = i + t0;
                                    intersect.Bt = j + t1;
                                    // Test for extry/exit:
                                    // The test point is the average of a point a little way along the current edge vector and a point a little further
                                    // around the polygon.
                                    Vector testPt = (polygonA.PolygonEdgePointAt(intersect.At + 0.001) + intersect.Vertex.Position + v0 * 0.001)/2;
                                    if (polygonB.PolygonContainmentXY(testPt)) intersect.Entry = true;
                                    else intersect.Entry = false;

                                    if (!intersectionsA.ContainsKey(intersect.At) && !intersectionsB.ContainsKey(intersect.Bt))
                                    {
                                        intersectionsA.Add(intersect.At, intersect);
                                        intersectionsB.Add(intersect.Bt, intersect);
                                    }
                                }
                            }
                        }
                    }
                }

                if (intersectionsA.Count < 1)
                {
                    // No intersections found - polygon A is either entirely inside or entirely outside B
                    if (inside || 
                        (polygonB.PolygonContainmentXY(polygonA[0].Position.Interpolate(polygonA.AveragePoint(),0.001)))) // A is inside B
                    {
                        result = new List<TPolygon>();
                        TPolygon polygon = new TPolygon();
                        foreach (Vertex v in polygonA)
                        {
                            polygon.Add(v);
                        }
                        result.Add(polygon);
                    }
                    else if (polygonA.PolygonContainmentXY(polygonB[0].Position)) // B is inside A
                    {
                        result = new List<TPolygon>();
                        TPolygon polygon = new TPolygon();
                        foreach (Vertex v in polygonB)
                        {
                            polygon.Add(v);
                        }
                        result.Add(polygon);
                    }
                    // Else no intersection!
                }
                else
                {
                    // Mark as entries & exits:
                    /*foreach (LineLineIntersection intersect in intersectionsA.Values)
                    {
                        inside = !inside; // Flip inside/outside
                        intersect.Entry = inside; // TODO: Check for degeneracy?
                    }*/

                    // Build resultant polygons
                    result = new List<TPolygon>();

                    while (intersectionsA.Count > 1) //TODO: Test and Review!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        LineLineIntersection startInt = intersectionsA.First().Value;// null;
                        // Find the first remaining intersection that is an entry point
                        /*foreach (LineLineIntersection intersect in intersectionsA.Values)
                        {
                            if (intersect.Entry)
                            {
                                startInt = intersect;
                                break;
                            }
                        }

                        if (startInt == null)
                        {
                            startInt = intersectionsA.First().Value;
                            //break; // Something has gone wrong and no entry point could be found!
                        }*/

                        TPolygon polygon = new TPolygon();
                        polygon.Add(startInt.Vertex);
                        result.Add(polygon);

                        LineLineIntersection prevInt = startInt;
                        LineLineIntersection nextInt;
                        if (startInt.Entry)
                            nextInt = intersectionsA.NextAfter(prevInt.At, true);
                        else nextInt = intersectionsB.NextAfter(prevInt.Bt, true);

                        //Alternate between polygons A and B to loop around overlap regions
                        while (nextInt != null)
                        {
                            intersectionsA.Remove(nextInt.At);
                            intersectionsB.Remove(nextInt.Bt);

                            if (!prevInt.Entry)
                            {
                                foreach (Vertex v in polygonB.AllBetween(prevInt.Bt, nextInt.Bt)) polygon.Add(v);
                            }
                            else
                            { 
                                foreach (Vertex v in polygonA.AllBetween(prevInt.At, nextInt.At)) polygon.Add(v);   
                            }

                            if (nextInt == startInt)
                            {
                                nextInt = null;
                            }
                            else
                            {
                                if (polygon.Last() != nextInt.Vertex) polygon.Add(nextInt.Vertex);
                                prevInt = nextInt;
                                if (nextInt.Entry)
                                    nextInt = intersectionsA.NextAfter(nextInt.At, true);
                                else
                                    nextInt = intersectionsB.NextAfter(nextInt.Bt, true);
                            }
                        }
                    }
                }
                        
            }

            return result;

        }

        /// <summary>
        /// A class for storing line-line intersection events
        /// </summary>
        [Serializable]
        private class LineLineIntersection
        {
            #region Fields

            /// <summary>
            /// The intersection point itself
            /// </summary>
            public Vertex Vertex;

            /// <summary>
            /// The intersection parameter on A
            /// </summary>
            public double At;

            /// <summary>
            /// The intersection parameter on B
            /// </summary>
            public double Bt;

            /// <summary>
            /// Is this intersection an entry from A into B (or, if false, an exit)?
            /// </summary>
            public bool Entry;

            #endregion

            #region Constructor

            /// <summary>
            /// Default constructor
            /// </summary>
            public LineLineIntersection() { }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="point"></param>
            /// <param name="at"></param>
            /// <param name="bt"></param>
            public LineLineIntersection(Vertex vert, double at, double bt)
            {
                Vertex = vert;
                At = at;
                Bt = bt;
            }

            #endregion
        }
    }
}

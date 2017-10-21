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

namespace Nucleus.Geometry
{
    /// <summary>
    /// A static helper class of functions to find the intersections between geometries of various types
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
            if (v0.X.Abs() <= 0.0000001)
            {
                if (v1.X.Abs() <= 0.0000001) return Vector.Unset;
                else
                {
                    double m2 = v1.Y / v1.X;
                    double c2 = pt1.Y - m2 * pt1.X;
                    double x = pt0.X;
                    double y = m2 * x + c2;
                    t0 = (y - pt0.Y) / v0.Y;
                    t1 = (x - pt1.X) / v1.X;
                    return new Vector(x,y,pt0.Z);
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

                if (m1 - m2 == 0) return Vector.Unset;
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
        public static Vector LineLineXY(Line lineA, Line lineB, bool bounded = false)
        {
            Vector pt0 = lineA.StartPoint;
            Vector v0 = lineA.EndPoint - pt0;
            Vector pt1 = lineB.StartPoint;
            Vector v1 = lineB.EndPoint - pt1;
            double t0 = 0;
            double t1 = 0;
            Vector result = LineLineXY(pt0, v0, pt1, v1, ref t0, ref t1);
            if (!bounded || (result.IsValid() && t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)) return result;
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
        /// <param name="line"></param>
        /// <param name="arc"></param>
        /// <param name="lineBounded"></param>
        /// <returns></returns>
        public static Vector[] LineArcXY(Vector lineOrigin, Vector lineDir, Arc arc, bool lineBounded = false)
        {
            double[] ts = LineCircleXY(lineOrigin, lineDir, arc.Circle.Origin, arc.Circle.Radius);
            
            // Sort out which intersections exist and (optionally) are within bounds:
            bool t0Valid = ts.Length > 0 && (!lineBounded || (ts[0] >= 0 && ts[0] <= 1));
            bool t1Valid = ts.Length > 1 && (!lineBounded || (ts[1] >= 0 && ts[1] <= 1));
            Vector pt0 = Vector.Unset;
            Vector pt1 = Vector.Unset;
            if (t0Valid)
            {
                pt0 = lineOrigin + lineDir * ts[0];
                if (!arc.IsInAngleRange(pt0)) t0Valid = false;
            }
            if (t1Valid)
            {
                pt1 = lineOrigin + lineDir * ts[1];
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
            if (distSqd > rsSqd && distSqd <= (r0 - r1).Squared()) // No intersections
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
        /// <returns></returns>
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
        /// <returns></returns>
        public static Vector[] ArcArcXY(Arc arc0, Arc arc1)
        {
            Vector[] v = CircleCircleXY(arc0.Circle, arc1.Circle);

            // Check that the points found lie on both of the arcs:

            bool v0Valid = (v.Length > 0 && arc0.IsInAngleRange(v[0]) && arc1.IsInAngleRange(v[0]));
            bool v1Valid = (v.Length > 1 && arc0.IsInAngleRange(v[1]) && arc1.IsInAngleRange(v[1]));

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
        /// Find the intersection between an infinite line and an infinite plane.
        /// Expressed as a parameter t, the multiple of the rayDirection away from the rayOrigin
        /// at which the intersection takes place.
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
                || (segStart.Y <= rayStart.Y && segEnd.Y > rayStart.Y))
            {
                //if (segStart.X == rayStart.X && segEnd.X == rayStart.X) onLine = true;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Find the section(s) of a line which lie(s) inside the specified polygon
        /// </summary>
        /// <param name="line"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static CurveCollection LineInPolygonXY(Line line, IList<Vertex> polygon)
        {
            CurveCollection result = new CurveCollection();
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
                else
                {
                    result.Add(new Line(line.StartPoint, intersections.Values[0]));
                    j += 1;
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

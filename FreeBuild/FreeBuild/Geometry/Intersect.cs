using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
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
            if (v0.X == 0)
            {
                if (v1.X == 0) return Vector.Unset;
                else
                {
                    double m2 = v1.Y / v1.X;
                    double c2 = pt1.Y - m2 * pt1.X;
                    double x = pt0.X;
                    return new Vector(x, m2 * x + c2, pt0.Z);
                }
            }
            else if (v1.X == 0)
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
        public static bool XRayLineSegmentXYCheck(ref Vector rayStart, ref Vector segStart, ref Vector segEnd)
        {
            if (segStart.X < rayStart.X)
            {
                if (segEnd.X < rayStart.X) return false; // Segment to the left!
                // Ray start falls within segment bounds - need to do additional check on whether
                // ray start is to the right or left
                else if (((segStart.Y >= rayStart.Y && segEnd.Y < rayStart.Y)
                || (segStart.Y < rayStart.Y && segEnd.Y >= rayStart.Y))
                && (segStart.X + (segEnd.X - segStart.X) * ((rayStart.Y - segStart.Y) / (segEnd.Y - segStart.Y)) < rayStart.X))
                {
                    return true;
                }
                else return false;
            }
            else if (segEnd.X < rayStart.X) //Ray start falls within segment bounds
            {
                if (((segStart.Y >= rayStart.Y && segEnd.Y < rayStart.Y)
                || (segStart.Y < rayStart.Y && segEnd.Y >= rayStart.Y))
                && (segStart.X + (segEnd.X - segStart.X) * ((rayStart.Y - segStart.Y) / (segEnd.Y - segStart.Y)) < rayStart.X))
                {
                    return true;
                }
                else return false;
            }
            //Segment is to the right of ray start
            else if ((segStart.Y >= rayStart.Y && segEnd.Y < rayStart.Y)
                || (segStart.Y < rayStart.Y && segEnd.Y >= rayStart.Y))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Find the overlapping region between two polygons, represented as sets of vertices
        /// </summary>
        /// <param name="polygonA"></param>
        /// <param name="polygonB"></param>
        /// <returns></returns>
        public static IList<TPolygon> PolygonOverlapXY<TPolygon>(IList<Vertex> polygonA, IList<Vertex> polygonB) where TPolygon: class, IList<Vertex>, new()
        {
            TPolygon current = null;
            bool previousInside = false;
            for (int i = 0; i <= polygonA.Count; i++) //Iterate through the vertices of polygon A
            {
                Vertex vA1 = polygonA.GetWrapped(i);
                bool inside = polygonB.PolygonContainmentXY(vA1.Position); //Check whether this polygon lies inside polygonB
                if (inside != previousInside && i > 0)
                {
                    //Boundaries cross - find intersection:
                    Vertex vA0 = polygonA[i - 1];
                    Vector startA = vA0.Position;
                    Vector endA = vA1.Position;
                    for (int j = 0; j <= polygonB.Count; j++)
                    {
                        Vector startB = polygonB[j].Position;
                        Vector endB = polygonB.GetWrapped(j + 1).Position;
                        Vector intersection = LineSegmentsXY(ref startA, ref endA, ref startB, ref endB);
                        if (intersection.IsValid()) //Intersection exists!
                        {
                            Vertex newVertex = new Vertex(intersection);
                            

                            break; //Don't need to check anymore??? - What if it crosses the same line twice?
                        }
                    }
                }
                //TODO: Add to current if inside

                //ABORT! ABORT! WILL NOT WORK IN CASES WHERE A LINE SEGMENT CROSSES RIGHT OVER THE OTHER POLYGON!

                previousInside = inside;
            }

            throw new NotImplementedException();
        }
    }
}

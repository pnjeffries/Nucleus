using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Static helper class containing common calculations
    /// to do with triangles
    /// </summary>
    public static class Triangle
    {
        /// <summary>
        /// Calculate the signed area of a triangle on the XY plane
        /// </summary>
        /// <param name="v0">The first vertex of the triangle</param>
        /// <param name="v1">The second vertex of the triangle</param>
        /// <param name="v2">The third vertex of the triangle</param>
        public static double XYArea(Vector v0, Vector v1, Vector v2)
        {
            return XYArea(v0.X, v0.Y, v1.X, v1.Y, v2.X, v2.Y);
        }

        /// <summary>
        /// Calculate the signed area of a triangle on the XY plane
        /// </summary>
        /// <param name="x0">The x coordinate of the first vertex of the triangle</param>
        /// <param name="y0">The y coordinate of the first vertex of the triangle</param>
        /// <param name="x1">The x coordinate of the second vertex of the triangle</param>
        /// <param name="y1">The y coordinate of the second vertex of the triangle</param>
        /// <param name="x2">The x coordinate of the third vertex of the triangle</param>
        /// <param name="y2">The y coordinate of the first vertex of the triangle</param>
        /// <returns></returns>
        public static double XYArea(double x0, double y0, double x1, double y1, double x2, double y2)
        {
            // Area = 0.5 * (-p1y * p2x + p0y * (-p1x + p2x) + p0x * (p1y - p2y) + p1x * p2y);
            return 0.5 * (-y1 * x2 + y0 * (-x1 + x2) + x0 * (y1 - y2) + x1 * y2);
        }

        /// <summary>
        /// Calculate the barycentric coordinates of a point with relation to
        /// a triangle on the XY plane.
        /// </summary>
        /// <param name="xP">The x coordinate of the point</param>
        /// <param name="yP">The y coordinate of the point</param>
        /// <param name="x0">The x coordinate of the first vertex of the triangle</param>
        /// <param name="y0">The y coordinate of the first vertex of the triangle</param>
        /// <param name="x1">The x coordinate of the second vertex of the triangle</param>
        /// <param name="y1">The y coordinate of the second vertex of the triangle</param>
        /// <param name="x2">The x coordinate of the third vertex of the triangle</param>
        /// <param name="y2">The y coordinate of the first vertex of the triangle</param>
        /// <param name="s">Output.  The barycentric s coordinate.</param>
        /// <param name="t">Output.  The barycentric t coordinate.</param>
        public static void BarycentricCoordinates(double xP, double yP, 
            double x0, double y0, double x1, double y1, double x2, double y2,
            out double s, out double t)
        {
            // s = 1 / (2 * Area) * (p0y * p2x - p0x * p2y + (p2y - p0y) * px + (p0x - p2x) * py);
            // t = 1 / (2 * Area) * (p0x * p1y - p0y * p1x + (p0y - p1y) * px + (p1x - p0x) * py);

            double oA = 1 / (2 * XYArea(x0, y0, x1, y1, x2, y2));
            s = oA * (y0 * x2 - x0 * y2 + (y2 - y0) * xP + (x0 - x2) * yP);
            t = oA * (x0 * y1 - y0 * x1 + (y0 - y1) * xP + (x1 - x0) * yP);

            //var s = p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y;
            //var t = p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y;
        }

        /// <summary>
        /// Calculate the barycentric coordinates of a point with relation
        /// to a triangle
        /// </summary>
        /// <param name="p">The point to calculate coordinates for</param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public static void BarycentricCoordinates(Vector p, Vector a, Vector b, Vector c,
            out double s, out double t)
        {
            Vector v0 = b - a, v1 = c - a, v2 = p - a;
            double d00 = v0.Dot(v0);
            double d01 = v0.Dot(v1);
            double d11 = v1.Dot(v1);
            double d20 = v2.Dot(v0);
            double d21 = v2.Dot(v1);
            double denom = d00 * d11 - d01 * d01;
            s = (d11 * d20 - d01 * d21) / denom;
            t = (d00 * d21 - d01 * d20) / denom;
            //u = 1.0 - s - t;
        }

        /// <summary>
        /// Test whether a point lies within or on a triangle on the XY plane
        /// </summary>
        /// <param name="xP">The x coordinate of the point</param>
        /// <param name="yP">The y coordinate of the point</param>
        /// <param name="x0">The x coordinate of the first vertex of the triangle</param>
        /// <param name="y0">The y coordinate of the first vertex of the triangle</param>
        /// <param name="x1">The x coordinate of the second vertex of the triangle</param>
        /// <param name="y1">The y coordinate of the second vertex of the triangle</param>
        /// <param name="x2">The x coordinate of the third vertex of the triangle</param>
        /// <param name="y2">The y coordinate of the first vertex of the triangle</param>
        /// <param name="edgePointsAreInside">If true, treat points on the triangle edge as being contained within the triangle.</param>
        /// <returns></returns>
        public static bool XYContainment(double xP, double yP,
            double x0, double y0, double x1, double y1, double x2, double y2, bool edgePointsAreInside = true)
        {
            double s, t;
            BarycentricCoordinates(xP, yP, x0, y0, x1, y1, x2, y2, out s, out t);
            if (edgePointsAreInside)
            {
                return s >= 0 && t >= 0 && (1 - s - t) >= 0;
            }
            else
            {
                return s > 0 && t > 0 && (1 - s - t) > 0;
            }
        }

        /// <summary>
        /// Test whether a point lies within or on a triangle on the XY plane
        /// </summary>
        /// <param name="pt">The point to test</param>
        /// <param name="t0">The position of the first vertex of the triangle</param>
        /// <param name="t1">The position of the second vertex of the triangle</param>
        /// <param name="t2">The position of the third vertex of the triangle</param>
        /// <param name="edgePointsAreInside">If true, treat points on the triangle edge as being contained within the triangle.</param>
        /// <returns></returns>
        public static bool XYContainment(Vector pt, Vector t0, Vector t1, Vector t2, bool edgePointsAreInside = true)
        {
            return XYContainment(pt.X, pt.Y, t0.X, t0.Y, t1.X, t1.Y, t2.X, t2.Y, edgePointsAreInside);
        }

        /// <summary>
        /// Test whether a point lies within or on a triangle on the XY plane
        /// </summary>
        /// <param name="pt">The point to test</param>
        /// <param name="t0">The position of the first vertex of the triangle</param>
        /// <param name="t1">The position of the second vertex of the triangle</param>
        /// <param name="t2">The position of the third vertex of the triangle</param>
        /// <param name="edgePointsAreInside">If true, treat points on the triangle edge as being contained within the triangle.</param>
        /// <returns></returns>
        public static bool XYContainment(Vertex pt, Vertex t0, Vertex t1, Vertex t2, bool edgePointsAreInside = true)
        {
            return XYContainment(pt.X, pt.Y, t0.X, t0.Y, t1.X, t1.Y, t2.X, t2.Y, edgePointsAreInside);
        }

        /// <summary>
        /// Find the Z coordinate of a point on the XY plane projected onto
        /// a 3D triangle
        /// </summary>
        /// <param name="xP">The x coordinate of the point</param>
        /// <param name="yP">The y coordinate of the point</param>
        /// <param name="v0">The first vertex of the triangle</param>
        /// <param name="v1">The second vertex of the triangle</param>
        /// <param name="v2">The third vertex of the triangle</param>
        /// <param name="bound">If true (default) the point will be bounded
        /// to the triangle and if it lies outside double.NaN will be
        /// returned.</param>
        /// <returns></returns>
        public static double ZCoordinateOfPoint(double xP, double yP,
            Vector v0, Vector v1, Vector v2, bool bound = true)
        {
            double s, t;
            BarycentricCoordinates(xP, yP, v0.X, v0.Y, v1.X, v1.Y, v2.X, v2.Y, out s, out t);
            double u = 1.0 - s - t;
            if (!bound || (s >= 0 && t >= 0 && u >= 0))
            {
                return u * v0.Z + s * v1.Z + t * v2.Z;
            }
            else return double.NaN;
        }

        /// <summary>
        /// Convert a point defined in Barycentric coordinates on a triangle to 
        /// cartesian coordinates in real space.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="u"></param>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static Vector PointFromBarycentric(double s, double t, double u, Vector pt0, Vector pt1, Vector pt2)
        {
            return u * pt0 + s * pt1 + t * pt2;
        }
    }
}

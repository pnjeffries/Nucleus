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

            double oA = 1 / 2 * XYArea(x0, y0, x1, y1, x2, y2);
            s = oA * (y0 * x2 - x0 * y2 + (y2 - y0) * xP + (x0 - x2) * yP);
            t = oA * (x0 * y1 - y0 * x1 + (y0 - y1) * xP + (x1 - x0) * yP);
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
        /// <returns></returns>
        public static bool XYContainment(double xP, double yP,
            double x0, double y0, double x1, double y1, double x2, double y2)
        {
            double s, t;
            BarycentricCoordinates(xP, yP, x0, y0, x1, y1, x2, y2, out s, out t);
            return s >= 0 && t >= 0 && (1 - s - t) >= 0;
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
            if (!bound || (s >= 0 && t >= 0 && (1 - s - t) >= 0))
            {
                return s * v0.Z + t * v1.Z + u * v2.Z;
            }
            else return double.NaN;
        }
    }
}

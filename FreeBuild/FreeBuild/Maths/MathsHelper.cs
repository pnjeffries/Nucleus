using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// Static maths helper functions
    /// </summary>
    public static class MathsHelper
    {
        /// <summary>
        /// Calculate the signed area 'under' the line segment between the two specified points
        /// on the XY plane - i.e. the area between the line and the X-axis.
        /// </summary>
        /// <param name="x0">The x-coordinate of the start of the line</param>
        /// <param name="y0">The y-coordinate of the start of the line</param>
        /// <param name="x1">The x-coordinate of the end of the line</param>
        /// <param name="y1">The y-coordinate of the end of the line</param>
        /// <param name="centroid">The cumulative centroid.  The centroid of the block under
        /// the line will be added to the value passed in here</param>
        /// <returns>The signed area as a double.</returns>
        public static double AreaUnder(double x0, double y0, double x1, double y1, ref Vector centroid)
        {
            //Area is calculated as a rectangle and a triangle combined:
            double areaRectangle = y0 * (x1 - x0);
            double areaTri = (y1 - y0) * (x1 - x0) * 0.5;
            centroid = centroid.Add(
                (x1 + x0) * 0.5 * areaRectangle + (x0 + (x1 - x0) * 2 / 3) * areaTri,
                y0 * 0.5 * areaRectangle + (y0 + (y1 - y0) / 3) * areaTri);
            return areaRectangle + areaTri;
        }

        /// <summary>
        /// Calculate the signed second moment of area 'under' the line segment between the
        /// two specified coordinates on the XY plane about the X-axis
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns>The signed Ixx as a double</returns>
        public static double IxxUnder(double x0, double y0, double x1, double y1)
        {
            double aR = y0 * (x1 - x0); //Area of rectangle
            double IxxR = ((x1 - x0) * y0 * y0 * y0) / 12; //(bh^3)/12
            double yCR = y0 / 2; //y of centroid of rectangle
            double aT = (y1 - y0) * (x1 - x0) * 0.5; //Area of triangle
            double IxxT = ((x1 - x0) * Math.Pow((y1 - y0), 3)) / 36; //(bh^3)/36
            double yCT = (y0 + (y1 - y0) / 3); //y of centroid triangle
            return IxxR + aR * yCR * yCR + IxxT + aT * yCT * yCT;
        }
    }
}

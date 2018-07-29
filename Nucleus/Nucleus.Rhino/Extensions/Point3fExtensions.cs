using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rhino
{
    /// <summary>
    /// Extension methods for the RhinoCommon Point3f struct
    /// </summary>
    public static class Point3fExtensions
    {
        /// <summary>
        /// Find the square of the distance on the XY plane between two points
        /// </summary>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static double XYSquaredDistanceTo(this Point3f ptA, Point3f ptB)
        {
            double dX = ptA.X - ptB.X;
            double dY = ptA.Y - ptB.Y;
            return dX * dX + dY * dY;
        }

        /// <summary>
        /// Find the distance on the XY plane between two points
        /// </summary>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static double XYDistanceTo(this Point3f ptA, Point3f ptB)
        {
            return Math.Sqrt(ptA.XYSquaredDistanceTo(ptB));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    public static class Intersect
    {
        /// <summary>
        /// Find the intersection point, if one exists, for two infinite lines on the XY plane.
        /// For 3D, use the ClosestPoint function instead.
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
        /// A quick check on whether a half-line starting at the specified point and travelling parallel to the X-axis
        /// will intersect the specified segment on the XY plane.  Used in planar containment testing.
        /// </summary>
        /// <param name="rayStart"></param>
        /// <param name="segStart"></param>
        /// <param name="segEnd"></param>
        /// <returns></returns>
        public static bool XHalfLineSegmentXYCheck(ref Vector rayStart, ref Vector segStart, ref Vector segEnd)
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
    }
}

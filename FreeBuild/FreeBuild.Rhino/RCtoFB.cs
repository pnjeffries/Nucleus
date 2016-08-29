using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC = Rhino.Geometry;

namespace Newt.RhinoCommon
{
    /// <summary>
    /// Helper class to convert from RhinoCommon to FreeBuild format
    /// </summary>
    public static class RCtoFB
    {
        /// <summary>
        /// Convert a Rhino point to a FreeBuild vector
        /// </summary>
        /// <param name="point">The point to convert</param>
        /// <returns>A new vector with the same components as the Rhino one.
        /// If the point is invalid, Vector.Unset will be returned instead.</returns>
        public static Vector Convert(RC.Point3d point)
        {
            if (point.IsValid)
                return new Vector(point.X, point.Y, point.Z);
            else return Vector.Unset;
        }

        /// <summary>
        /// Convert a Rhino line to a FreeBuild one
        /// </summary>
        /// <param name="line">The line to be converted</param>
        /// <returns>A new line if the input is valid, else null</returns>
        public static Line Convert(RC.Line line)
        {
            if (line.IsValid)
                return new Line(Convert(line.From), Convert(line.To));
            else return null;
        }

        /// <summary>
        /// Convert a Rhino line to a FreeBuild one
        /// </summary>
        /// <param name="line">The line to be converted</param>
        /// <returns>A new line if the input is valid, else null</returns>
        public static Line Convert(RC.LineCurve line)
        {
            if (line.IsValid)
                return new Line(Convert(line.PointAtStart), Convert(line.PointAtEnd));
            else return null;
        }

        /// <summary>
        /// Convert a Rhino polyline to a FreeBuild one
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static PolyLine Convert(RC.Polyline polyline)
        {
            if (polyline != null && polyline.IsValid)
            {
                var points = new List<Vector>();
                foreach (RC.Point3d pt in polyline)
                {
                    points.Add(Convert(pt));
                }
                return new PolyLine(points);
            }
            else return null;
        }

        /// <summary>
        /// Convert a Rhino curve to a FreeBuild one
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static Curve Convert(RC.Curve curve)
        {
            if (curve is RC.LineCurve) return Convert((RC.LineCurve)curve);
            else if (curve.IsPolyline())
            {
                RC.Polyline pLine;
                curve.TryGetPolyline(out pLine);
                return Convert(pLine);
            }
            else throw new NotImplementedException();
        }
    }
}

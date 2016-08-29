using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC = Rhino.Geometry;

namespace FreeBuild.Rhino
{
    /// <summary>
    /// Conversion class to convert FreeBuild object to RhinoCommon ones
    /// </summary>
    public static class FBtoRC
    {
        /// <summary>
        /// Convert a FreeBuild vector representing a point to a RhinoCommon point
        /// </summary>
        /// <param name="point">The vector to convert to a point</param>
        /// <returns>If the input point is valid, a new RhinoCommon Point3d, else Point3d.Unset</returns>
        public static RC.Point3d Convert(Vector point)
        {
            if (point.IsValid()) return new RC.Point3d(point.X, point.Y, point.Z);
            else return RC.Point3d.Unset;
        }

        /// <summary>
        /// Convert a FreeBuild line to a RhinoCommon one
        /// </summary>
        /// <param name="line">The line to convert</param>
        /// <returns>If the input line is valid, a new RhinoCommon LineCurve,
        /// else null</returns>
        public static RC.LineCurve Convert(Line line)
        {
            if (line.IsValid) return new RC.LineCurve(Convert(line.StartPoint), Convert(line.EndPoint));
            else return null;
        }

        /// <summary>
        /// Convert a FreeBuild polyline into a RhinoCommon PolylineCurve
        /// </summary>
        /// <param name="polyline">The polyline to convert</param>
        /// <returns></returns>
        public static RC.PolylineCurve Convert(PolyLine polyline)
        {
            if (polyline != null && polyline.IsValid)
            {
                var points = new List<RC.Point3d>();
                foreach (Vertex vertex in polyline.Vertices)
                {
                    points.Add(Convert(vertex.Position));
                }
                return new RC.PolylineCurve(points);
            }
            return null;
        }

    }
}

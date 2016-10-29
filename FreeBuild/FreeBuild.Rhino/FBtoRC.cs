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
        /// Convert a FreeBuild vector to a RhinoCommon vector
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns></returns>
        public static RC.Vector3d ConvertVector(Vector vector)
        {
            if (vector.IsValid()) return new RC.Vector3d(vector.X, vector.Y, vector.Z);
            else return RC.Vector3d.Unset;
        }

        /// <summary>
        /// Convert a FreeBuild plane to a RhinoCommon one
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static RC.Plane Convert(Plane plane)
        {
            return new RC.Plane(Convert(plane.Origin), ConvertVector(plane.X), ConvertVector(plane.Y));
        }

        /// <summary>
        /// Convert a FreeBuild circle to a RhinoCommon one
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static RC.Circle Convert(Circle circle)
        {
            return new RC.Circle(Convert(circle.Plane()), circle.Radius);
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

        /// <summary>
        /// Convert a FreeBuild Arc to a RhinoCommon ArcCurve
        /// </summary>
        /// <param name="arc">The arc to convert</param>
        /// <returns></returns>
        public static RC.ArcCurve Convert(Arc arc)
        {
            if (arc.IsValid)
            {
                if (arc.Closed) return new RC.ArcCurve(Convert(arc.Circle));
                else return new RC.ArcCurve(new RC.Arc(Convert(arc.StartPoint), Convert(arc.Vertices[1].Position), Convert(arc.EndPoint)));
            }
            return null;
        }

        /// <summary>
        /// Convert a FreeBuild curve into a RhinoCommon one
        /// </summary>
        /// <param name="curve">The curve to convert</param>
        /// <returns></returns>
        public static RC.Curve Convert(Curve curve)
        {
            if (curve is Line) return Convert((Line)curve);
            else if (curve is PolyLine) return Convert((PolyLine)curve);
            else if (curve is Arc) return Convert((Arc)curve);
            //TODO: Others
            else throw new Exception("Conversion of curve type to Rhino not yet supported.");
        }

    }
}

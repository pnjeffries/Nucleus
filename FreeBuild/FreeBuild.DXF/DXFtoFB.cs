using FreeBuild.Geometry;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.DXF
{
    /// <summary>
    /// A class of static helper functions to convert from netDXF datatypes
    /// to FreeBuild ones
    /// </summary>
    public static class DXFtoFB
    {
        /// <summary>
        /// Convert a netDXF Vector3 to a FreeBuild Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector Convert(Vector3 vector)
        {
            return new Vector(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Convert a netDXF Vector2 to a FreeBuild Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector Convert(Vector2 vector)
        {
            return new Vector(vector.X, vector.Y);
        }

        /// <summary>
        /// Convert a netDXF point to a FreeBuild cloud.
        /// NOTE: This may be modified if ever a FreeBuild single point
        /// equivalent is introduced.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Cloud Convert(netDxf.Entities.Point point)
        {
            return new Cloud(Convert(point.Position));
        }

        /// <summary>
        /// Convert a netDXF Line to a FreeBuild one
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line Convert(netDxf.Entities.Line line)
        {
            return new Line(Convert(line.StartPoint), Convert(line.EndPoint));
        }

        /// <summary>
        /// Convert a netDXF polyline to a FreeBuild one
        /// </summary>
        /// <param name="polyLine"></param>
        /// <returns></returns>
        public static PolyLine Convert(netDxf.Entities.LwPolyline polyLine)
        {
            var pts = new List<Vector>();
            foreach (netDxf.Entities.LwPolylineVertex plV in polyLine.Vertexes)
            {
                pts.Add(Convert(plV.Position));
            }
            return new PolyLine(pts, polyLine.IsClosed);
        }

        /// <summary>
        /// Convert a netDXF polyline to a FreeBuild one
        /// </summary>
        /// <param name="polyLine"></param>
        /// <returns></returns>
        public static PolyLine Convert(netDxf.Entities.Polyline polyLine)
        {
            var pts = new List<Vector>();
            foreach (netDxf.Entities.PolylineVertex plV in polyLine.Vertexes)
            {
                pts.Add(Convert(plV.Position));
            }
            return new PolyLine(pts, polyLine.IsClosed);
        }

        /// <summary>
        /// Convert a netDXF arc to a FreeBuild one
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static Arc Convert(netDxf.Entities.Arc arc)
        {
            Circle circ = new Circle(arc.Radius, Convert(arc.Center), Convert(arc.Normal));
            return new Geometry.Arc(circ, Angle.FromDegrees(arc.StartAngle), Angle.FromDegrees(arc.EndAngle));
        }

        /// <summary>
        /// Convert a netDXF circle to a FreeBuild arc
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static Arc Convert(netDxf.Entities.Circle circle)
        {
            return new Arc(new Circle(circle.Radius, Convert(circle.Center), Convert(circle.Normal)));
        }

        /// <summary>
        /// Convert a netDXF circle to a FreeBuild one
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static Circle ConvertCircle(netDxf.Entities.Circle circle)
        {
            return new Circle(circle.Radius, Convert(circle.Center), Convert(circle.Normal));
        }

        /// <summary>
        /// Convert a netDXF spline to a FreeBuild curve.
        /// Currently, as no spline implementation yet exists in FreeBuild,
        /// the Polyline representation is used.
        /// </summary>
        /// <param name="spline"></param>
        /// <returns></returns>
        public static Curve Convert(netDxf.Entities.Spline spline)
        {
            return Convert(spline.ToPolyline(50));
        }

    }
}

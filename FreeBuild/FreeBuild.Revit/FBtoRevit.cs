using Autodesk.Revit.DB;
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FB = FreeBuild.Geometry;
using AD = Autodesk.Revit.DB;

namespace FreeBuild.Revit
{
    /// <summary>
    /// Static conversion helper functions to convert FreeBuild types into Revit equivalents
    /// </summary>
    public static class FBtoRevit
    {
        /// <summary>
        /// Get or set the scaling factor to be used when converting FreeBuild entities to Revit ones.
        /// Default is m to ft.
        /// </summary>
        public static double ConversionScaling { get; set; } = 1/0.3048;

        /// <summary>
        /// Convert a FreeBuild vector to a Revit XYZ
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <param name="scaling">Optional.  The scaling conversion factor to be used.
        /// If unspecified or 0, the currently set ConversionScaling factor will be used.</param>
        /// <returns></returns>
        public static XYZ Convert(Vector vector, double scaling = 0)
        {
            if (scaling == 0) scaling = ConversionScaling;
            return new XYZ(vector.X * scaling, vector.Y * scaling, vector.Z * scaling);
        }

        /// <summary>
        /// Convert a list of FreeBuild vectors to a list of Revit XYZs 
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static IList<XYZ> Convert(IList<Vector> vectors)
        {
            var result = new List<XYZ>(vectors.Count);
            foreach(Vector vect in vectors)
            {
                result.Add(Convert(vect));
            }
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild plane to a Revit one
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static AD.Plane Convert(FB.Plane plane)
        {
            return new AD.Plane(Convert(plane.X, 1), Convert(plane.Y, 1), Convert(plane.Origin));
        }

        /// <summary>
        /// Convert a FreeBuild line to a Revit one
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static AD.Line Convert(FB.Line line)
        {
            return AD.Line.CreateBound(Convert(line.StartPoint), Convert(line.EndPoint));
        }

        /// <summary>
        /// Convert a FreeBuild polyline to a Revit one
        /// </summary>
        /// <param name="polyLine"></param>
        /// <returns></returns>
        public static AD.PolyLine Convert(FB.PolyLine polyLine)
        {
            IList<XYZ> pts = Convert(polyLine.Vertices.ExtractPoints());
            if (polyLine.Closed) pts.Add(Convert(polyLine.StartPoint));
            return AD.PolyLine.Create(pts);
        }

        /// <summary>
        /// Convert a FreeBuild arc to a Revit one
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static AD.Arc Convert(FB.Arc arc)
        {
            if (arc.Closed) //Circle
            {
                AD.Plane plane = Convert(arc.Circle.Plane());
                return AD.Arc.Create(plane, arc.Circle.Radius, 0, 2 * Math.PI);
            }
            else return AD.Arc.Create(Convert(arc.StartPoint),
                Convert(arc.EndPoint),
                Convert(arc.PointOnArc));
        }

        /// <summary>
        /// Convert a FreeBuild curve into a Revit one
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static AD.Curve Convert(FB.Curve curve)
        {
            if (curve is FB.Line) return Convert((FB.Line)curve);
            else if (curve is FB.Arc) return Convert((FB.Arc)curve);
            // TODO: Make polyline curve subtype?
            // else if (curve is FB.PolyLine) return Convert((FB.PolyLine)curve);
            else throw new NotImplementedException();
        }
    }
}

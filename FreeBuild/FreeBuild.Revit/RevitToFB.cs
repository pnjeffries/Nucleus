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
    /// Static conversion helper functions to convert Revit types into FreeBuild equivalents
    /// </summary>
    public static class RevitToFB
    {
        /// <summary>
        /// Get or set the scaling factor to be used when converting Revit entities to FreeBuild ones.
        /// Default is ft to m.
        /// </summary>
        public static double ConversionScaling { get; set; } = 0.3048;

        /// <summary>
        /// Convert a Revit XYZ to a FreeBuild vector
        /// </summary>
        /// <param name="xyz">The XYZ to convert</param>
        /// <param name="scaling">Optional.  The scaling conversion factor to be used.
        /// If unspecified or 0, the currently set ConversionScaling factor will be used.</param>
        /// <returns></returns>
        public static Vector Convert(XYZ xyz, double scaling = 0)
        {
            if (scaling == 0) scaling = ConversionScaling;
            return new Vector(xyz.X * scaling, xyz.Y * scaling, xyz.Z * scaling);
        }

        /// <summary>
        /// Convert a list of Revit XYZs into an array of FreeBuild vectors
        /// </summary>
        /// <param name="xyzs">The list of XYZs to convert</param>
        /// <returns></returns>
        public static Vector[] Convert(IList<XYZ> xyzs)
        {
            Vector[] result = new Vector[xyzs.Count];
            for (int i = 0; i < xyzs.Count; i++)
            {
                result[i] = Convert(xyzs[i]);
            }
            return result;
        }

        /// <summary>
        /// Convert a Revit line to a FreeBuild one
        /// </summary>
        /// <param name="line">The line to convert</param>
        /// <returns></returns>
        public static FB.Line Convert(AD.Line line)
        {
            return new FB.Line(Convert(line.GetEndPoint(0)), Convert(line.GetEndPoint(1)));
        }

        /// <summary>
        /// Convert a Revit polyline to a FreeBuild one
        /// </summary>
        /// <param name="polyLine">The polyline to convert</param>
        /// <returns></returns>
        public static FB.PolyLine Convert(AD.PolyLine polyLine)
        {
            return new FB.PolyLine(Convert(polyLine.GetCoordinates()));
        }

        /// <summary>
        /// Convert a Revit arc to a FreeBuild one
        /// </summary>
        /// <param name="arc">The arc to convert</param>
        /// <returns></returns>
        public static FB.Arc Convert(AD.Arc arc)
        {
            if (arc.IsCyclic)
                return new FB.Arc(
                new Circle(
                    arc.Radius,
                    new CylindricalCoordinateSystem(
                        Convert(arc.Center),
                        Convert(arc.XDirection, 1),
                        Convert(arc.YDirection, 1))));
            else
                return new FB.Arc(
                    Convert(arc.GetEndPoint(0)), 
                    Convert(arc.Evaluate(0.5, true)), 
                    Convert(arc.GetEndPoint(1)));
        }
    }
}

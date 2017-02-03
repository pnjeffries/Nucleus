using FreeBuild.Geometry;
using FreeBuild.Rendering;
using netDxf;
using netDxf.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nDE = netDxf.Entities;

namespace FreeBuild.DXF
{
    /// <summary>
    /// A class of static helper functions to convert from FreeBuild datatypes
    /// to netDXF ones
    /// </summary>
    public static class FBtoDXF
    {
        /// <summary>
        /// Get or set the scaling factor to be used when converting DXF entities to FreeBuild ones
        /// </summary>
        public static double ConversionScaling { get; set; } = 1.0;

        /// <summary>
        /// Set the display and other attributes of a netDXF entity from those of an FreeBuild object
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributes"></param>
        public static void SetAttributes(nDE.EntityObject entity, GeometryAttributes attributes)
        {
            if (attributes != null)
            {
                if (attributes.Brush != null) entity.Color = Convert(attributes.Brush.BaseColour);
            }
        }

        /// <summary>
        /// Convert a FreeBuild colour to a netDXF one
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static AciColor Convert(Colour colour)
        {
            return new AciColor(colour.R, colour.G, colour.B);
        }

        /// <summary>
        /// Convert a FreeBuild vector to a netDXF one
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scaling"></param>
        /// <returns></returns>
        public static Vector3 Convert(Vector vector, double scaling = 0)
        {
            if (scaling == 0) scaling = ConversionScaling;
            return new Vector3(vector.X * scaling, vector.Y * scaling, vector.Z * scaling);
        }

        /// <summary>
        /// Convert a set of FreeBuild vectors to a list of netDXF ones
        /// </summary>
        /// <param name="vectors"></param>
        /// <param name="scaling"></param>
        /// <returns></returns>
        public static IList<Vector3> Convert(IEnumerable<Vector> vectors, double scaling = 0)
        {
            List<Vector3> result = new List<Vector3>(vectors.Count());
            foreach (Vector v in vectors)
            {
                result.Add(Convert(v, scaling));
            }
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild vector to a netDXF Vector2
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scaling"></param>
        /// <returns></returns>
        public static Vector2 ConvertToVector2(Vector vector, double scaling = 0)
        {
            if (scaling == 0) scaling = ConversionScaling;
            return new Vector2(vector.X * scaling, vector.Y * scaling);
        }

        /// <summary>
        /// Convert a FreeBuild transformation matrix to a netDXF one
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Matrix3 Convert(Transform t)
        {
            return new Matrix3(
                t[0, 0], t[0, 1], t[0, 2],
                t[1, 0], t[1, 1], t[1, 2],
                t[2, 0], t[2, 1], t[2, 2]);
        }

        /// <summary>
        /// Convert a FreeBuild point object to a netDXF one
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static nDE.Point Convert(Point pt)
        {
            var result = new nDE.Point(Convert(pt.Position));
            SetAttributes(result, pt.Attributes);
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild point cloud to a set of netDXF points
        /// </summary>
        /// <param name="cloud"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IList<nDE.Point> Convert(Cloud cloud, IList<nDE.Point> result = null)
        {
            if (result == null) result = new List<nDE.Point>();
            foreach (Vertex v in cloud.Vertices)
            {
                var pt = new nDE.Point(Convert(v.Position));
                SetAttributes(pt, cloud.Attributes);
                result.Add(pt);
            }
            return result;
        }

        /// <summary>
        /// Convert FreeBuild horizontal and vertical set out values to a netDXF text alignment
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public static nDE.TextAlignment Convert(HorizontalSetOut horizontal, VerticalSetOut vertical)
        {
            if (horizontal == HorizontalSetOut.Left)
            {
                if (vertical == VerticalSetOut.Top) return nDE.TextAlignment.TopLeft;
                else if (vertical == VerticalSetOut.Bottom) return nDE.TextAlignment.BottomLeft;
                else return nDE.TextAlignment.MiddleLeft;
            }
            else if (horizontal == HorizontalSetOut.Right)
            {
                if (vertical == VerticalSetOut.Top) return nDE.TextAlignment.TopRight;
                else if (vertical == VerticalSetOut.Bottom) return nDE.TextAlignment.BottomRight;
                else return nDE.TextAlignment.MiddleRight;
            }
            else
            {
                if (vertical == VerticalSetOut.Top) return nDE.TextAlignment.TopCenter;
                else if (vertical == VerticalSetOut.Bottom) return nDE.TextAlignment.BottomCenter;
                else return nDE.TextAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Convert a FreeBuild label to a netDXF text object
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static nDE.Text Convert(Label label)
        {
            var result = new nDE.Text(label.Text, Convert(label.Position), label.TextSize * ConversionScaling);
            result.Alignment = Convert(label.HorizontalSetOut, label.VerticalSetOut);
            SetAttributes(result, label.Attributes);
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild line to a netDXF one
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static nDE.Line Convert(Line line)
        {
            var result = new nDE.Line(Convert(line.StartPoint), Convert(line.EndPoint));
            SetAttributes(result, line.Attributes);
            return result;
        }

        /// <summary>
        /// Convert a FreeBuid polyline to a netDXF one
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static nDE.Polyline Convert(PolyLine polyline)
        {
            var result = new nDE.Polyline(Convert(polyline.Vertices.ExtractPoints()));
            result.IsClosed = polyline.Closed;
            SetAttributes(result, polyline.Attributes);
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild arc to a netDXF one
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static nDE.EntityObject Convert(Arc arc)
        {
            if (arc.Closed)
            {
                var result =  new nDE.Circle(Convert(arc.Circle.Origin), arc.Circle.Radius * ConversionScaling);
                result.Normal = Convert(arc.Circle.L, 1);
                SetAttributes(result, arc.Attributes);
                return result;
            }
            else
            {
                Plane plane = new Plane(arc.Circle.Origin, arc.Circle.L);
                var result = new nDE.Arc(Convert(arc.Circle.Origin), arc.Circle.Radius * ConversionScaling,
                    plane.GlobalToLocal(arc.StartPoint).Angle.Degrees, plane.GlobalToLocal(arc.EndPoint).Angle.Degrees);
                result.Normal = Convert(plane.Z, 1);
                SetAttributes(result, arc.Attributes);
                return result;
            }
        }

        /// <summary>
        /// Convert a FreeBuild polycurve to a list of netDXF objects
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="result">Optional.  A list to be populated and returned as the output.</param>
        /// <returns></returns>
        public static IList<nDE.EntityObject> Convert(PolyCurve curve, IList<nDE.EntityObject> result = null)
        {
            if (result == null) result = new List<nDE.EntityObject>(curve.SubCurves.Count);
            foreach (Curve subCrv in curve.SubCurves)
            {
                Convert(subCrv, result);
            }
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild curve to a netDXF HatchBoundaryPath
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static nDE.HatchBoundaryPath ConvertToBoundary(Curve curve)
        {
            IEnumerable<nDE.EntityObject> pathObjs;
            if (!(curve is PolyLine))
                pathObjs = Convert(curve);
            else
                pathObjs = Convert(((PolyLine)curve).ToPolyCurve());
            var result = new nDE.HatchBoundaryPath(pathObjs);//Convert(curve));
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild planar region to a netDXF hatch
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static nDE.Hatch Convert(PlanarRegion region)
        {
            var result = new nDE.Hatch(nDE.HatchPattern.Solid, false);
            result.BoundaryPaths.Add(ConvertToBoundary(region.Perimeter));
            foreach (Curve voidCrv in region.Voids)
            {
                result.BoundaryPaths.Add(ConvertToBoundary(voidCrv));
            }
            SetAttributes(result, region.Attributes);
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild layer to a netDXF one
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static Layer Convert(GeometryLayer layer)
        {
            var result = new Layer(layer.Name);
            if (layer.Brush != null)
                result.Color = Convert(layer.Brush.BaseColour);
            result.IsVisible = layer.Visible;
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild geometry object to a netDXF
        /// </summary>
        /// <param name="geo"></param>
        /// <returns></returns>
        public static IList<nDE.EntityObject> Convert(VertexGeometry geo, IList<nDE.EntityObject> result = null)
        {
            if (result == null) result = new List<nDE.EntityObject>();
            
            if (geo is Line) result.Add(Convert((Line)geo));
            else if (geo is PolyLine) result.Add(Convert((PolyLine)geo));
            else if (geo is Arc) result.Add(Convert((Arc)geo));
            else if (geo is PolyCurve) Convert((PolyCurve)geo, result);
            else if (geo is PlanarRegion) result.Add(Convert((PlanarRegion)geo));
            else if (geo is Label) result.Add(Convert((Label)geo));
            else if (geo is Point) result.Add(Convert((Point)geo));
            else if (geo is Cloud) Convert((Cloud)geo, result);
            return result;
        }
    }
}

using Nucleus.Geometry;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Rendering;
using nDE = netDxf.Entities;

namespace Nucleus.DXF
{
    /// <summary>
    /// A class of static helper functions to convert from netDXF datatypes
    /// to Nucleus ones
    /// </summary>
    public static class DXFtoFB
    {
        /// <summary>
        /// Get or set the scaling factor to be used when converting DXF entities to Nucleus ones
        /// </summary>
        public static double ConversionScaling { get; set; } = 1.0;

        /// <summary>
        /// Extract and convert geometry attributes from the netDXF entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static GeometryAttributes ExtractAttributes(netDxf.Entities.EntityObject entity)
        {
            AciColor color = entity.Color;
            if (color == null || color.IsByLayer) color = entity.Layer?.Color;
            return new GeometryAttributes(
                entity.Handle, 
                entity.Layer?.Name,
                new ColourBrush(Convert(color)));
        }

        /// <summary>
        /// Convert a netDXF AciColor to a Nucleus Colour
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Colour Convert(AciColor color)
        {
            if (color.Index == AciColor.Default.Index) return Colour.Black;
            else return new Colour(color.R, color.G, color.B);
        }

        /// <summary>
        /// Convert a netDXF Vector3 to a Nucleus Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector Convert(Vector3 vector, double scaling = 0)
        {
            if (scaling == 0) scaling = ConversionScaling;
            return new Vector(vector.X * scaling, vector.Y * scaling, vector.Z * scaling);
        }

        /// <summary>
        /// Convert a netDXF Vector2 to a Nucleus Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector Convert(Vector2 vector)
        {
            return new Vector(vector.X * ConversionScaling, vector.Y * ConversionScaling);
        }

        /// <summary>
        /// Convert a netDXF transformation matrix to a Nucleus transform
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Transform Convert(Matrix3 t)
        {
            return new Transform(
                t.M11, t.M12, t.M13,
                t.M21, t.M22, t.M23,
                t.M31, t.M32, t.M33);
        }

        /// <summary>
        /// Convert a netDXF point to a Nucleus cloud.
        /// NOTE: This may be modified if ever a Nucleus single point
        /// equivalent is introduced.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point Convert(nDE.Point point)
        {
            return new Point(Convert(point.Position), ExtractAttributes(point));
        }

        /// <summary>
        /// Convert a netDXF attachment point to a horizontal set-out
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static HorizontalSetOut ConvertHorizontal(nDE.MTextAttachmentPoint attachment)
        {
            if (attachment == nDE.MTextAttachmentPoint.BottomLeft ||
                attachment == nDE.MTextAttachmentPoint.MiddleLeft ||
                attachment == nDE.MTextAttachmentPoint.TopLeft)
                return HorizontalSetOut.Left;
            else if (attachment == nDE.MTextAttachmentPoint.BottomRight ||
                attachment == nDE.MTextAttachmentPoint.MiddleRight ||
                attachment == nDE.MTextAttachmentPoint.TopRight)
                return HorizontalSetOut.Right;
            else return HorizontalSetOut.MidPoint;
        }

        /// <summary>
        /// Convert a netDXF attachment point to a vertical set-out
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static VerticalSetOut ConvertVertical(nDE.MTextAttachmentPoint attachment)
        {
            if (attachment == nDE.MTextAttachmentPoint.BottomCenter ||
                attachment == nDE.MTextAttachmentPoint.BottomLeft ||
                attachment == nDE.MTextAttachmentPoint.BottomRight)
                return VerticalSetOut.Bottom;
            else if (attachment == nDE.MTextAttachmentPoint.TopCenter ||
                attachment == nDE.MTextAttachmentPoint.TopLeft ||
                attachment == nDE.MTextAttachmentPoint.TopRight)
                return VerticalSetOut.Top;
            else
                return VerticalSetOut.MidPoint;
        }

        /// <summary>
        /// Convert a netDXF text object to a Nucleus label.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Label Convert(netDxf.Entities.Text text)
        {
            return new Label(Convert(text.Position), text.Value, text.Height * ConversionScaling);
        }

        /// <summary>
        /// Convert a netDXF MText object to a Nucleus label
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Label Convert(netDxf.Entities.MText text)
        {
            return new Label(Convert(text.Position), text.Value, text.Height * ConversionScaling, 
                ConvertVertical(text.AttachmentPoint), ConvertHorizontal(text.AttachmentPoint));
        }

        /// <summary>
        /// Convert a netDXF Line to a Nucleus one
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line Convert(netDxf.Entities.Line line)
        {
            return new Line(Convert(line.StartPoint), Convert(line.EndPoint), ExtractAttributes(line));
        }

        /// <summary>
        /// Convert a netDXF polyline to a Nucleus one
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
            return new PolyLine(pts, polyLine.IsClosed, ExtractAttributes(polyLine));
        }

        /// <summary>
        /// Convert a netDXF polyline to a Nucleus one
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
            return new PolyLine(pts, polyLine.IsClosed, ExtractAttributes(polyLine));
        }

        /// <summary>
        /// Convert a netDXF arc to a Nucleus one
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static Arc Convert(netDxf.Entities.Arc arc)
        {
            Circle circ = new Circle(arc.Radius * ConversionScaling, Convert(arc.Center), Convert(arc.Normal, 1));
            return new Arc(circ, Angle.FromDegrees(arc.StartAngle), Angle.FromDegrees(arc.EndAngle), ExtractAttributes(arc));
        }

        /// <summary>
        /// Convert a netDXF circle to a Nucleus arc
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static Arc Convert(netDxf.Entities.Circle circle)
        {
            return new Arc(new Circle(circle.Radius * ConversionScaling, Convert(circle.Center), Convert(circle.Normal, 1)), ExtractAttributes(circle));
        }

        /// <summary>
        /// Convert a netDXF circle to a Nucleus one
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static Circle ConvertCircle(netDxf.Entities.Circle circle)
        {
            return new Circle(circle.Radius * ConversionScaling, Convert(circle.Center), Convert(circle.Normal, 1));
        }

        /// <summary>
        /// Convert a netDXF spline to a Nucleus curve.
        /// Currently, as no spline implementation yet exists in Nucleus,
        /// the Polyline representation is used.
        /// </summary>
        /// <param name="spline"></param>
        /// <returns></returns>
        public static Curve Convert(netDxf.Entities.Spline spline)
        {
            return Convert(spline.ToPolyline(50)); //TEMP?
        }

        /// <summary>
        /// Convert a netDXF HatchBoundaryPath to a Nucleus PolyCurve
        /// or PolyLine
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Curve Convert(nDE.HatchBoundaryPath path)
        {
            PolyCurve result = new PolyCurve();
            foreach (nDE.HatchBoundaryPath.Edge edge in path.Edges)
            {
                VertexGeometry edgeRep = Convert(edge.ConvertTo());
                if (edgeRep != null && edgeRep is Curve)
                {
                    result.Add((Curve)edgeRep);
                }
            }
            if (result.IsPolyline()) return result.ToPolyLine();
            else return result;
        }

        /// <summary>
        /// Convert a netDXF hatch to a set of Nucleus planar regions
        /// </summary>
        /// <param name="hatch"></param>
        /// <returns></returns>
        public static PlanarRegion[] Convert(nDE.Hatch hatch)
        {
            PlanarRegion[] result = new PlanarRegion[hatch.BoundaryPaths.Count];
            for (int i = 0; i < hatch.BoundaryPaths.Count; i++)
            {
                result[i] = new PlanarRegion(Convert(hatch.BoundaryPaths[i]), ExtractAttributes(hatch));
            }
            return result;
        }

        /// <summary>
        /// Convert a netDXF entity to a Nucleus geometry object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static VertexGeometry Convert(nDE.EntityObject entity)
        {
            if (entity is nDE.Point) return Convert((nDE.Point)entity);
            else if (entity is nDE.Line) return Convert((nDE.Line)entity);
            else if (entity is nDE.Arc) return Convert((nDE.Arc)entity);
            else if (entity is nDE.Circle) return Convert((nDE.Circle)entity);
            else if (entity is nDE.LwPolyline) return Convert((nDE.LwPolyline)entity);
            else if (entity is nDE.Polyline) return Convert((nDE.Polyline)entity);
            else if (entity is nDE.Spline) return Convert((nDE.Spline)entity);
            else if (entity is nDE.Text) return Convert((nDE.Text)entity);
            else if (entity is nDE.MText) return Convert((nDE.MText)entity);
            else return null;
        }

    }
}

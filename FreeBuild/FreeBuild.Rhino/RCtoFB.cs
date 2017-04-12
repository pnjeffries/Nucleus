using FreeBuild.Geometry;
using FreeBuild.Rendering;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC = Rhino.Geometry;
using R = Rhino;

namespace FreeBuild.Rhino
{
    /// <summary>
    /// Helper class to convert from RhinoCommon to FreeBuild format
    /// </summary>
    public static class RCtoFB
    {
        /// <summary>
        /// Get the current length conversion factor from the current Rhino document units to 
        /// SI units (m) in which FreeBuild objects are defined.
        /// </summary>
        public static double ConversionFactor
        {
            get
            {
                return RhinoMath.UnitScale(RhinoDoc.ActiveDoc.ModelUnitSystem, UnitSystem.Meters);
            }
        }

        /// <summary>
        /// Convert a Rhino point to a FreeBuild vector.
        /// This will automatically be converted into the current Rhino units.
        /// </summary>
        /// <param name="point">The point to convert</param>
        /// <returns>A new vector with the same components as the Rhino one.
        /// If the point is invalid, Vector.Unset will be returned instead.</returns>
        public static Vector Convert(RC.Point3d point)
        {
            if (point.IsValid)
            {
                double f = ConversionFactor;
                return new Vector(point.X * f, point.Y * f, point.Z * f);
            }
            else return Vector.Unset;
        }

        /// <summary>
        /// Convert a Rhino vector to a FreeBuild one.
        /// This is assumed to be unitless and will *not* automatically be
        /// converted into the current Rhino units.
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns></returns>
        public static Vector Convert(RC.Vector3d vector)
        {
            return new Vector(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Convert a Rhino plane to a FreeBuild one
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Plane Convert(RC.Plane plane)
        {
            return new Plane(Convert(plane.Origin),
                Convert(plane.XAxis), Convert(plane.YAxis));
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
        /// Convert a Rhino point object to a FreeBuild one
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point Convert(RC.Point point)
        {
            if (point.IsValid)
                return new Point(Convert(point.Location));
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
        /// Convert a Rhino arc to a FreeBuild one
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static Arc Convert(RC.Arc arc)
        {
            if (arc.IsCircle)
            {
                //TODO
                throw new NotImplementedException();
            }
            else return new Arc(Convert(arc.StartPoint), Convert(arc.MidPoint), Convert(arc.EndPoint));
            throw new NotImplementedException();
        }

        public static Circle Convert(RC.Circle circle)
        {
            //TODO
            throw new NotImplementedException();
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
        /// Convert a RhinoCommon PolyCurve to a FreeBuild one
        /// </summary>
        /// <param name="polyCurve"></param>
        /// <returns></returns>
        public static PolyCurve Convert(RC.PolyCurve polyCurve)
        {
            if (polyCurve != null)
            {
                PolyCurve result = new PolyCurve();
                RC.Curve[] subCrvs = polyCurve.Explode();
                foreach (RC.Curve subCrv in subCrvs)
                {
                    Curve crv = Convert(subCrv);
                    if (crv != null) result.Add(crv);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// Convert a Rhino curve to a FreeBuild one
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static Curve Convert(RC.Curve curve)
        {
            if (curve is RC.LineCurve) return Convert((RC.LineCurve)curve);
            else if (curve.IsLinear()) return Convert(new RC.Line(curve.PointAtStart, curve.PointAtEnd));
            else if (curve.IsPolyline())
            {
                RC.Polyline pLine;
                if (curve.TryGetPolyline(out pLine))
                    return Convert(pLine);
            }
            else if (curve is RC.PolyCurve) return Convert((RC.PolyCurve)curve);
            else if (curve.IsArc())
            {
                RC.Arc arc;
                if (curve.TryGetArc(out arc))
                    return Convert(arc);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert a Rhino mesh to a FreeBuild one
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static Mesh Convert(RC.Mesh mesh)
        {
            Mesh result = new Mesh();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                result.AddVertex(Convert(mesh.Vertices[i]));
                // TODO: Vertex colours?
            }
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                RC.MeshFace mF = mesh.Faces[i];
                if (mF.IsTriangle)
                    result.AddFace(mF.A, mF.B, mF.C);
                else
                    result.AddFace(mF.A, mF.B, mF.C, mF.D);
            }
            return result;
        }

        /// <summary>
        /// Convert a planar Rhino surface to a FreeBuild planar region
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static PlanarRegion ConvertToPlanarRegion(RC.Surface surface)
        {
            throw new NotImplementedException();
            //TODO!
        }

        /// <summary>
        /// Convert a Rhino surface to a FreeBuild one
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static Surface Convert(RC.Surface surface)
        {
            if (surface.IsPlanar()) return ConvertToPlanarRegion(surface);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Convert RhinoCommon geometry to FreeBuild geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static VertexGeometry Convert(RC.GeometryBase geometry)
        {
            if (geometry is RC.Curve) return Convert((RC.Curve)geometry);
            if (geometry is RC.Point) return Convert((RC.Point)geometry);
            else throw new NotImplementedException();
        }

        /// <summary>
        /// Convert a Rhino object reference to FreeBuild geometry with attached
        /// attributes.
        /// </summary>
        /// <param name="objRef"></param>
        /// <returns></returns>
        public static VertexGeometry Convert(ObjRef objRef)
        {
            VertexGeometry result = Convert(objRef.Geometry());
            if (result != null)
            {
                GeometryAttributes attributes = new GeometryAttributes();
                int layerIndex = objRef.Object().Attributes.LayerIndex;
                attributes.LayerName = RhinoDoc.ActiveDoc.Layers[layerIndex].Name;
                attributes.SourceID = objRef.ObjectId.ToString();
                result.Attributes = attributes;
            }
            return result;
        }
    }
}

using Nucleus.Geometry;
using Nucleus.Rendering;
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
using Nucleus.Model;

namespace Nucleus.Rhino
{
    /// <summary>
    /// Helper class to convert from RhinoCommon to Nucleus format
    /// </summary>
    public static class RCtoN
    {
        /// <summary>
        /// Get the current length conversion factor from the current Rhino document units to 
        /// SI units (m) in which Nucleus objects are defined.
        /// </summary>
        public static double ConversionFactor
        {
            get
            {
                return RhinoMath.UnitScale(RhinoDoc.ActiveDoc.ModelUnitSystem, UnitSystem.Meters);
            }
        }

        /// <summary>
        /// Convert a Rhino point to a Nucleus vector.
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
        /// Convert a Rhino vector to a Nucleus one.
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
        /// Convert a system Color to a Nucleus Colour
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Colour Convert(System.Drawing.Color color)
        {
            return new Colour(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Convert a Rhino plane to a Nucleus one
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Plane Convert(RC.Plane plane)
        {
            return new Plane(Convert(plane.Origin),
                Convert(plane.XAxis), Convert(plane.YAxis));
        }

        /// <summary>
        /// Convert a Rhino line to a Nucleus one
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
        /// Convert a Rhino point object to a Nucleus one
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
        /// Convert a Rhino line to a Nucleus one
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
        /// Convert a Rhino arc to a Nucleus one
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static Arc Convert(RC.Arc arc)
        {
            if (arc.IsCircle)
            {
                return new Arc(Convert(new RC.Circle(arc)));
            }
            else return new Arc(Convert(arc.StartPoint), Convert(arc.MidPoint), Convert(arc.EndPoint));
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert a Rhino circle to a Nucleus one
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static Circle Convert(RC.Circle circle)
        {
            return new Circle(circle.Radius, Convert(circle.Plane));
        }

        /// <summary>
        /// Convert a Rhino polyline to a Nucleus one
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
        /// Convert a RhinoCommon PolyCurve to a Nucleus one
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
        /// Convert a Rhino curve to a Nucleus one
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
        /// Convert a Rhino mesh to a Nucleus one
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
        /// Convert a planar Rhino surface to a Nucleus planar region
        /// </summary>
        /// <param name="brep"></param>
        /// <returns></returns>
        public static PlanarRegion ConvertToPlanarRegion(RC.Brep brep, RC.BrepFace face)
        {
            //if (face.IsPlanar())
            //{
                int[] iEdges = face.AdjacentEdges();
                RC.Curve[] curves = new RC.Curve[iEdges.Length];
                for (int i = 0; i < iEdges.Length; i++)
                {
                    RC.BrepEdge bEdge = brep.Edges[i];
                    curves[i] = bEdge;
                }
                var joined = RC.Curve.JoinCurves(curves);
                if (joined.Length > 0)
                {
                    RC.Curve outer = outerCurve(joined);
                    if (outer != null)
                    {
                        Curve boundary = Convert(outer);
                        var result = new PlanarRegion(boundary);
                        for (int i = 0; i < joined.Length; i++)
                        {
                            if (joined[i] != outer)
                            {
                                Curve voidCrv = Convert(joined[i]);
                                result.Voids.Add(voidCrv);
                            }
                        }
                        return result;
                    }
                }
            //}
            return null;
        }

        /// <summary>
        /// Find the curve in this collection which encloses the others
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        private static RC.Curve outerCurve(IList<RC.Curve> curves)
        {
            if (curves.Count == 0) return null;
            RC.Curve result = curves[0];
            RC.Plane plane;
            if (!result.TryGetPlane(out plane)) return null;
            for (int i = 1; i < curves.Count; i++)
            {
                RC.Curve curve = curves[i];
                var containment = RC.Curve.PlanarClosedCurveRelationship(result, curve, plane, Tolerance.Distance);
                if (containment == RC.RegionContainment.AInsideB)
                {
                    result = curve;
                }
            }
            return result;
        }

        /// <summary>
        /// Convert a Rhino surface to a Nucleus one
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static Surface Convert(RC.Surface surface)
        {
            return Convert(surface.ToBrep());
        }

        /// <summary>
        /// Convert a Rhino BRep to a Nucleus surface
        /// </summary>
        /// <param name="brep"></param>
        /// <returns></returns>
        public static Surface Convert(RC.Brep brep)
        {
            if (brep.Faces.Count > 0)
            {
                // TODO: Convert ALL faces
                RC.BrepFace face = brep.Faces[0];
                if (face.IsPlanar()) return ConvertToPlanarRegion(brep, face);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert RhinoCommon geometry to Nucleus geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static VertexGeometry Convert(RC.GeometryBase geometry)
        {
            if (geometry is RC.Curve) return Convert((RC.Curve)geometry);
            else if (geometry is RC.Point) return Convert((RC.Point)geometry);
            else if (geometry is RC.Surface) return Convert((RC.Surface)geometry);
            else if (geometry is RC.Mesh) return Convert((RC.Mesh)geometry);
            else if (geometry is RC.Brep)
                return Convert(((RC.Brep)geometry));
            else throw new NotImplementedException();
        }

        /// <summary>
        /// Convert a Rhino object reference to Nucleus geometry with attached
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

        /// <summary>
        /// Convert a collection of rhino geometry to a VertexGeometryCollection
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static VertexGeometryCollection Convert(IList<RC.GeometryBase> geometry)
        {
            var result = new VertexGeometryCollection();
            foreach (var item in geometry)
            {
                var subRes = Convert(item);
                result.Add(subRes);
            }
            return result;
        }

        /// <summary>
        /// Convert a single Rhino geometry object to a VertexGeometryCollection
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static VertexGeometryCollection ConvertToCollection(RC.GeometryBase geometry)
        {
            return new VertexGeometryCollection(Convert(geometry));
        }

    }
}

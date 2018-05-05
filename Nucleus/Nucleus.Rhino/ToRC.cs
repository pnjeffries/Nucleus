using Nucleus.Geometry;
using Nucleus.Rendering;
using Rhino.Display;
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
    /// Conversion class to convert Nucleus object to RhinoCommon ones
    /// </summary>
    public static class ToRC
    {
        /// <summary>
        /// Get the current length conversion factor from SI units (m) in which Nucleus objects are defined
        /// to the current Rhino document units.
        /// </summary>
        public static double ConversionFactor
        {
            get
            {
                return R.RhinoMath.UnitScale(R.UnitSystem.Meters, R.RhinoDoc.ActiveDoc.ModelUnitSystem);
            }
        }
        
        /// <summary>
        /// Convert a length in SI units (m) to the current Rhino unit scale
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static double Convert(double length)
        {
            return length * ConversionFactor;
        }

        /// <summary>
        /// Convert a Nucleus vector representing a point to a RhinoCommon point.
        /// This will be automatically scaled to the current Rhino unit system.
        /// </summary>
        /// <param name="point">The vector to convert to a point</param>
        /// <param name="unitless">If true, the vector will not be scaled</param>
        /// <returns>If the input point is valid, a new RhinoCommon Point3d, else Point3d.Unset</returns>
        public static RC.Point3d Convert(Vector point)
        {
            double f = ConversionFactor;
            if (point.IsValid()) return new RC.Point3d(point.X * f, point.Y * f, point.Z * f);
            else return RC.Point3d.Unset;
        }

        /// <summary>
        /// Convert a Nucleus vector representing a point to a RhinoCommon point.
        /// This will be automatically scaled to the current Rhino unit system unless the
        /// optional unitless parameter is set to true.
        /// </summary>
        /// <param name="point">The vector to convert to a point</param>
        /// <param name="unitless">If true, the vector will not be scaled</param>
        /// <returns>If the input point is valid, a new RhinoCommon Point3d, else Point3d.Unset</returns>
        public static RC.Point3d Convert(Vector point, bool unitless)
        {
            double f = 1.0;
            if (!unitless) f = ConversionFactor;
            if (point.IsValid()) return new RC.Point3d(point.X * f, point.Y * f, point.Z * f);
            else return RC.Point3d.Unset;
        }

        /// <summary>
        /// Convert a Nucleus vector to a RhinoCommon vector
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns></returns>
        public static RC.Vector3d ConvertVector(Vector vector)
        {
            if (vector.IsValid()) return new RC.Vector3d(vector.X, vector.Y, vector.Z);
            else return RC.Vector3d.Unset;
        }

        /// <summary>
        /// Convert a Nucleus bounding box to a RhinoCommon one
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static RC.BoundingBox Convert(BoundingBox box)
        {
            double f = ConversionFactor;
            return new RC.BoundingBox(box.MinX * f, box.MinY * f, box.MinZ * f, box.MaxX * f, box.MaxY * f, box.MaxZ * f);
        }

        /// <summary>
        /// Convert a Nucleus plane to a RhinoCommon one
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static RC.Plane Convert(Plane plane)
        {
            return new RC.Plane(Convert(plane.Origin), ConvertVector(plane.X), ConvertVector(plane.Y));
        }

        /// <summary>
        /// Convert a Nucleus colour to a System.Drawing.Colour
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static System.Drawing.Color Convert(Colour colour)
        {
            return System.Drawing.Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        }

        /// <summary>
        /// Convert a Nucleus DisplayBrush to a Rhino DisplayMaterial
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static DisplayMaterial Convert(DisplayBrush brush)
        {
            DisplayMaterial result = new DisplayMaterial();
            result.Diffuse = Convert(brush.BaseColour);
            result.Emission = Convert(brush.BaseColour);
            result.Transparency = 1.0 - brush.BaseColour.A / 255.0;
            return result;
        }

        /// <summary>
        /// Convert a Nucleus circle to a RhinoCommon one
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static RC.Circle Convert(Circle circle)
        {
            return new RC.Circle(Convert(circle.Plane()), circle.Radius * ConversionFactor);
        }

        /// <summary>
        /// Convert a Nucleus line to a RhinoCommon one
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
        /// Convert a Nucleus line to a RhinoCommon line struct
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static RC.Line ConvertToLine(Line line)
        {
            if (line.IsValid) return new RC.Line(Convert(line.StartPoint), Convert(line.EndPoint));
            else return RC.Line.Unset;
        }

        /// <summary>
        /// Convert a Nucleus polyline into a RhinoCommon PolylineCurve
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
                if (polyline.Closed) points.Add(Convert(polyline.StartPoint));
                return new RC.PolylineCurve(points);
            }
            return null;
        }

        /// <summary>
        /// Convert a Nucleus Arc to a RhinoCommon ArcCurve
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
        /// Convert a Nucleus polyCurve into a RhinoCommon one
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static RC.PolyCurve Convert(PolyCurve curve)
        {
            RC.PolyCurve result = new RC.PolyCurve();
            foreach (Curve subCrv in curve.SubCurves)
            {
                RC.Curve rCrv = Convert(subCrv);
                if (rCrv != null) result.Append(rCrv);
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus curve into a RhinoCommon one
        /// </summary>
        /// <param name="curve">The curve to convert</param>
        /// <returns></returns>
        public static RC.Curve Convert(Curve curve)
        {
            if (curve == null) return null;
            else if (curve is Line) return Convert((Line)curve);
            else if (curve is PolyLine) return Convert((PolyLine)curve);
            else if (curve is Arc) return Convert((Arc)curve);
            else if (curve is PolyCurve) return Convert((PolyCurve)curve);
            //TODO: Others
            else throw new Exception("Conversion of curve type to Rhino not yet supported.");
        }

        /// <summary>
        /// Convert a list of Nucleus curves to a list of Rhino common curves
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static IList<RC.Curve> Convert(IList<Curve> curves)
        {
            var result = new List<RC.Curve>();
            foreach (Curve crv in curves)
            {
                result.Add(Convert(crv));
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus PlanarSurface into a Rhino BRep
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static RC.Brep Convert(PlanarRegion surface)
        {
            if (surface == null || !surface.IsValid) return null;
            else
            {
                var rCrvs = new List<RC.Curve>();
                rCrvs.Add(Convert(surface.Perimeter));
                if (surface.HasVoids)
                {
                    foreach (Curve voidCrv in surface.Voids)
                    {
                        rCrvs.Add(Convert(voidCrv));
                    }
                }
                RC.Brep[] result = RC.Brep.CreatePlanarBreps(rCrvs);
                if (result == null)
                {
                    result = new RC.Brep[] 
                    { RC.Brep.CreatePatch(rCrvs, 1, 1, Tolerance.Distance) };
                }
                if (result != null && result.Length > 0) return result[0];
             }
            return null;
        }

        /// <summary>
        /// Convert a list of Nucleus planar regions to a list of Rhino common BReps
        /// </summary>
        /// <param name="surfaces"></param>
        /// <returns></returns>
        public static IList<RC.Brep> Convert(IList<PlanarRegion> surfaces)
        {
            var result = new List<RC.Brep>();
            foreach (PlanarRegion srf in surfaces)
            {
                result.Add(Convert(srf));
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus Linear Element to a Rhino Extrusion, if possible
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static RC.Extrusion ConvertToExtrusion(LinearElement element)
        {
            Curve perimeter = element?.Family?.Profile?.Perimeter;
            CurveCollection voids = element?.Family?.Profile?.Voids;
            if (perimeter != null && element.Geometry != null)
            {
                RC.Curve profile = Convert(perimeter);
                var cSystem = element.Geometry.LocalCoordinateSystem(0, element.Orientation);
                if (element.Geometry is Line)
                {
                    //If a line, create an extrusion:
                    RC.Extrusion ext = new RC.Extrusion();
                    ext.SetPathAndUp(
                        Convert(element.Geometry.EndPoint), Convert(element.Geometry.StartPoint),
                        ConvertVector(cSystem.Z));
                    ext.SetOuterProfile(profile, true);
                    if (voids != null)
                    {
                        var voidCrvs = Convert(voids);
                        foreach (var rCrv in voidCrvs) ext.AddInnerProfile(rCrv);
                    }
                    //RC.Surface surface = RC.Extrusion.CreateExtrusion(profile, new RC.Vector3d(Convert(element.End.Position - element.Start.Position)));
                    return ext;
                }
            }
            return null;
        }

        /// <summary>
        /// Convert a Nucleus extrusion volume to a Rhino one
        /// </summary>
        /// <param name="extrusion"></param>
        /// <returns></returns>
        public static RC.Extrusion Convert(Extrusion extrusion)
        {
            if (extrusion.IsValid)
            {
                Curve perimeter = extrusion.Profile?.Perimeter;
                CurveCollection voids = extrusion.Profile?.Voids;
                if (perimeter != null)
                {
                    RC.Curve profile = Convert(perimeter);
                   // var cSystem = new CartesianCoordinateSystem(new Vector(), extrusion.Path);
                        //If a line, create an extrusion:
                        RC.Extrusion ext = new RC.Extrusion();

                    ext.SetPathAndUp(new RC.Point3d(0, 0, 0), Convert(extrusion.Path), RC.Vector3d.YAxis); //TODO: Test!
                            //ConvertVector(cSystem.Z));
                        ext.SetOuterProfile(profile, true);
                        if (voids != null)
                        {
                            var voidCrvs = Convert(voids);
                            foreach (var rCrv in voidCrvs) ext.AddInnerProfile(rCrv);
                        }
                        //RC.Surface surface = RC.Extrusion.CreateExtrusion(profile, new RC.Vector3d(Convert(element.End.Position - element.Start.Position)));
                        return ext;
                }
            }
            return null;
        }

        /// <summary>
        /// Convert a LinearElement to a Rhino Brep
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static RC.Brep ConvertToBrep(LinearElement element)
        {
            if (element.Geometry is Line)
            {
                return ConvertToExtrusion(element)?.ToBrep();
            }
            else
            {
                Curve perimeter = element?.Family?.Profile?.Perimeter;
                CurveCollection voids = element?.Family?.Profile?.Voids;
                if (perimeter != null && element.Geometry != null)
                {
                    //TODO: Deal with voids!

                    RC.Curve profile = Convert(perimeter);
                    var cSystem = element.Geometry.LocalCoordinateSystem(0, element.Orientation);

                    RC.Plane startPlane = Convert(cSystem.YZPlane());
                    profile.Transform(RC.Transform.PlaneToPlane(RC.Plane.WorldXY, startPlane));
                    RC.Brep[] breps = RC.Brep.CreateFromSweep(Convert(element.Geometry), profile, false, 0.001); //TODO: Change tolerance
                    if (breps.Length > 0) return breps[0];
                }
            }
            return null;
        }

        /// <summary>
        /// Convert a Nucleus Mesh to a RhinoCommon one
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static RC.Mesh Convert(Mesh mesh)
        {
            var builder = new RhinoMeshBuilder();
            builder.AddMesh(mesh);
            builder.Finalize();
            return builder.Mesh;
        }

        /// <summary>
        /// Convert a Nucleus surface to Rhino geometry
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static RC.GeometryBase Convert(Surface surface)
        {
            if (surface is Mesh) return Convert((Mesh)surface);
            else if (surface is PlanarRegion) return Convert((PlanarRegion)surface);
            return null;
        }

        /// <summary>
        /// Convert Nucleus geometry into RhinoCommon geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static RC.GeometryBase Convert(VertexGeometry geometry)
        {
            if (geometry is Curve) return Convert((Curve)geometry);
            else if (geometry is Surface) return Convert((Surface)geometry);
            else if (geometry is Extrusion) return Convert((Extrusion)geometry);
            return null;
        }

        /// <summary>
        /// Convert a collection of Nucleus geometry into a collection of RhinoCommon geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static IList<RC.GeometryBase> Convert(VertexGeometryCollection geometry)
        {
            var result = new List<RC.GeometryBase>();
            foreach (var vG in geometry)
            {
                result.Add(Convert(vG));
            }
            return result;
        }


    }
}

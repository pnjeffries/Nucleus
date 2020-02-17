using Nucleus.Base;
using Nucleus.Geometry;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nucleus.DXF
{
    /// <summary>
    /// A reader class that can read geometry from .DXF format files
    /// </summary>
    public class DXFReader
    {

        /// <summary>
        /// Read a DXF and convert it into native Nucleus geometry types.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns></returns>
        public VertexGeometryCollection ReadDXF(Stream stream)
        {
            DxfDocument doc = DxfDocument.Load(stream);
            return ReadDXF(doc);
        }

        /// <summary>
        /// Read a DXF and convert it into native Nucleus geometry types.
        /// </summary>
        /// <param name="path">The filepath to read from</param>
        /// <returns></returns>
        public VertexGeometryCollection ReadDXF(FilePath path)
        {
            DxfDocument doc = DxfDocument.Load(path);
            return ReadDXF(doc);
        }

        /// <summary>
        /// Read a DXF and convert it into native Nucleus geometry types.
        /// </summary>
        /// <param name="doc">The document to read from</param>
        /// <returns></returns>
        private VertexGeometryCollection ReadDXF(DxfDocument doc)
        {
            VertexGeometryCollection result = new VertexGeometryCollection();

            double scale = 1.0;
            if (doc.DrawingVariables.InsUnits == netDxf.Units.DrawingUnits.Millimeters) scale = 0.001;
            else if (doc.DrawingVariables.InsUnits == netDxf.Units.DrawingUnits.Centimeters) scale = 0.01;
            FromDXF.ConversionScaling = scale;

            // Hatches
            foreach (netDxf.Entities.Hatch hatch in doc.Hatches)
            {
                result.AddRange(FromDXF.Convert(hatch));
            }

            // Lines
            foreach (netDxf.Entities.Line line in doc.Lines)
            {
                result.Add(FromDXF.Convert(line));
            }

            // Polylines
            foreach (netDxf.Entities.LwPolyline pLine in doc.LwPolylines)
            {
                result.Add(FromDXF.Convert(pLine));
            }
            foreach (netDxf.Entities.Polyline pLine in doc.Polylines)
            {
                result.Add(FromDXF.Convert(pLine));
            }

            // Arcs
            foreach (netDxf.Entities.Arc arc in doc.Arcs)
            {
                result.Add(FromDXF.Convert(arc));
            }
            foreach (netDxf.Entities.Circle circle in doc.Circles)
            {
                result.Add(FromDXF.Convert(circle));
            }

            // Splines
            foreach (netDxf.Entities.Spline spline in doc.Splines)
            {
                result.Add(FromDXF.Convert(spline));
            }

            // Points
            foreach (netDxf.Entities.Point point in doc.Points)
            {
                result.Add(FromDXF.Convert(point));
            }

            // Meshes
            foreach (netDxf.Entities.Mesh mesh in doc.Meshes)
            {
                result.Add(FromDXF.Convert(mesh));
            }

            // Polyface meshes
            foreach (var mesh in doc.PolyfaceMeshes)
            {
                result.Add(FromDXF.Convert(mesh));
            }

            // Text
            foreach (netDxf.Entities.Text text in doc.Texts)
            {
                result.Add(FromDXF.Convert(text));
            }
            foreach (netDxf.Entities.MText text in doc.MTexts)
            {
                result.Add(FromDXF.Convert(text));
            }


            // Block inserts
            foreach (netDxf.Entities.Insert insert in doc.Inserts)
            {
                // Explode:
                // Note: There is some commented-out code in the library to do this:
                // see: https://netdxf.codeplex.com/SourceControl/latest#netDxf/Entities/Insert.cs
                // TODO: Review and improve?
                Vector translation = FromDXF.Convert(insert.Position);
                Transform transform = FromDXF.Convert(insert.GetTransformation(netDxf.Units.DrawingUnits.Meters));

                foreach (netDxf.Entities.EntityObject entity in insert.Block.Entities)
                {
                    VertexGeometry shape = FromDXF.Convert(entity);
                    if (shape != null)
                    {
                        shape.Transform(transform);
                        shape.Move(translation);
                        result.Add(shape);
                    }
                }
            }


            return result;
        }
    }
}

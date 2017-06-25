using Nucleus.Base;
using Nucleus.Geometry;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DXF
{
    /// <summary>
    /// A reader class that can read geometry from .DXF format files
    /// </summary>
    public class DXFReader
    {
        public VertexGeometryCollection ReadDXF(FilePath path)
        {
            VertexGeometryCollection result = new VertexGeometryCollection();

            DxfDocument doc = DxfDocument.Load(path);
            double scale = 1.0;
            if (doc.DrawingVariables.InsUnits == netDxf.Units.DrawingUnits.Millimeters) scale = 0.001;
            else if (doc.DrawingVariables.InsUnits == netDxf.Units.DrawingUnits.Centimeters) scale = 0.01;
            DXFtoFB.ConversionScaling = scale;

            // Hatches
            foreach (netDxf.Entities.Hatch hatch in doc.Hatches)
            {
                result.AddRange(DXFtoFB.Convert(hatch));
            }

            // Lines
            foreach (netDxf.Entities.Line line in doc.Lines)
            {
                result.Add(DXFtoFB.Convert(line));
            }

            // Polylines
            foreach (netDxf.Entities.LwPolyline pLine in doc.LwPolylines)
            {
                result.Add(DXFtoFB.Convert(pLine));
            }
            foreach (netDxf.Entities.Polyline pLine in doc.Polylines)
            {
                result.Add(DXFtoFB.Convert(pLine));
            }

            // Arcs
            foreach (netDxf.Entities.Arc arc in doc.Arcs)
            {
                result.Add(DXFtoFB.Convert(arc));
            }
            foreach (netDxf.Entities.Circle circle in doc.Circles)
            {
                result.Add(DXFtoFB.Convert(circle));
            }

            // Splines
            foreach (netDxf.Entities.Spline spline in doc.Splines)
            {
                result.Add(DXFtoFB.Convert(spline));
            }

            // Points
            foreach (netDxf.Entities.Point point in doc.Points)
            {
                result.Add(DXFtoFB.Convert(point));
            }

            // Text
            foreach (netDxf.Entities.Text text in doc.Texts)
            {
                result.Add(DXFtoFB.Convert(text));
            }
            foreach (netDxf.Entities.MText text in doc.MTexts)
            {
                result.Add(DXFtoFB.Convert(text));
            }


            // Block inserts
            foreach (netDxf.Entities.Insert insert in doc.Inserts)
            {
                // Explode:
                // Note: There is some commented-out code in the library to do this:
                // see: https://netdxf.codeplex.com/SourceControl/latest#netDxf/Entities/Insert.cs
                // TODO: Review and improve?
                Vector translation = DXFtoFB.Convert(insert.Position);
                Transform transform = DXFtoFB.Convert(insert.GetTransformation(netDxf.Units.DrawingUnits.Meters));

                foreach (netDxf.Entities.EntityObject entity in insert.Block.Entities)
                {
                    VertexGeometry shape = DXFtoFB.Convert(entity);
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

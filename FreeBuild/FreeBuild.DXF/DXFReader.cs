using FreeBuild.Base;
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
    /// A reader class that can read geometry from .DXF format files
    /// </summary>
    public class DXFReader
    {
        public ShapeCollection ReadDXF(FilePath path)
        {
            ShapeCollection result = new ShapeCollection();

            DxfDocument doc = DxfDocument.Load(path);
            
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

            return result;
        }
    }
}

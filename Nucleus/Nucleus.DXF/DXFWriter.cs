using Nucleus.Base;
using Nucleus.Geometry;
using netDxf;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DXF
{
    /// <summary>
    /// Class to write Nucleus geometry to DXF format via netDXF.
    /// </summary>
    public class DXFWriter
    {
        /// <summary>
        /// Write the given geometry table to a DXF file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public bool WriteDXF(FilePath filePath, GeometryLayerTable geometry)
        {
            var header = new HeaderVariables();
            header.InsUnits = netDxf.Units.DrawingUnits.Meters;
            DxfDocument doc = new DxfDocument(header);

            foreach (GeometryLayer layer in geometry)
            {
                Layer dxfLayer = FBtoDXF.Convert(layer);
                doc.Layers.Add(dxfLayer);

                foreach (VertexGeometry geo in layer)
                {
                    foreach (EntityObject entity in FBtoDXF.Convert(geo))
                    {
                        entity.Layer = dxfLayer;
                        doc.AddEntity(entity);
                    }
                }
            }

            return doc.Save(filePath, true);
        }
    }
}

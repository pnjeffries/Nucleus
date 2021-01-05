﻿using Nucleus.Base;
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
        public bool WriteDXF(FilePath filePath, GeometryLayerTable geometry, bool binary = false)
        {
            var header = new HeaderVariables();
            header.InsUnits = netDxf.Units.DrawingUnits.Meters;
            // Use AutoCad2007 or later to avoid Windows encoding issues (AutoCad2007+ uses UTF8)
            header.AcadVer = DxfVersion.AutoCad2007;
            DxfDocument doc = new DxfDocument(header);

            foreach (GeometryLayer layer in geometry)
            {
                Layer dxfLayer = ToDXF.Convert(layer);
                doc.Layers.Add(dxfLayer);

                foreach (VertexGeometry geo in layer)
                {
                    if (geo.VertexCount > 0)
                    {
                        foreach (EntityObject entity in ToDXF.Convert(geo))
                        {
                            entity.Layer = dxfLayer;
                            doc.AddEntity(entity);
                        }
                    }
                }
            }

            return doc.Save(filePath, binary);
        }
    }
}

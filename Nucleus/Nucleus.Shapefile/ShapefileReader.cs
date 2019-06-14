using DotSpatial.Data;
using Nucleus.Base;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Shapefile
{
    /// <summary>
    /// A class to read data from ESRI shapefile format and convert it
    /// to .NUCLEUS standard geometry types.
    /// </summary>
    /// <remarks> Utilises the DotSpatial library: 
    /// https://github.com/DotSpatial/DotSpatial </remarks>
    public class ShapefileReader
    {
        /// <summary>
        /// Read all ESRI Shapefiles found within the specified location
        /// and convert and compile them into a
        /// .NUCLEUS geometry layer table.  Each separate filePath will form a separate
        /// layer in the table.
        /// </summary>
        /// <param name="filePath">The location to load shapefiles from.  This may be a single
        /// shapefile or a folder which contains shapefiles.</param>
        /// <param name="includeSubFolders">If true, shapefiles within subfolders within the main
        /// location will also be included.</param>
        /// <param name="extension">The extension of the shapefiles to be read.  If null, all files
        /// of any type will be read.</param>
        /// <returns></returns>
        public GeometryLayerTable ReadShapefiles(FilePath filePath, bool includeSubFolders = false, string extension = ".shp")
        {
            var subFiles = filePath.Explode(includeSubFolders, extension);
            return ReadShapeFiles(subFiles);
        }

        /// <summary>
        /// Read a list of ESRI Shapefiles and convert and compile them into a
        /// .NUCLEUS geometry layer table.  Each separate filePath will form a separate
        /// layer in the table.
        /// </summary>
        /// <param name="filePaths">A list of filepaths of the shapefiles to be read</param>
        /// <returns></returns>
        public GeometryLayerTable ReadShapeFiles(IList<FilePath> filePaths)
        {
            var result = new GeometryLayerTable();
            foreach (FilePath filePath in filePaths)
            {
                result.Add(ReadShapefile(filePath));
            }
            return result;
        }

        /// <summary>
        /// Read data from an ESRI Shapefile and convert it into a .NUCLEUS geometry layer
        /// </summary>
        /// <param name="filePath">The filepath of the shapefile</param>
        /// <returns></returns>
        public GeometryLayer ReadShapefile(FilePath filePath)
        {
            var result = new GeometryLayer(filePath.FileName);
            var shapeFile = DotSpatial.Data.Shapefile.OpenFile(filePath);
            foreach (IFeature feature in shapeFile.Features)
            {
                VertexGeometry vGeo = ToNucleus.Convert(feature);
                if (vGeo != null)
                    result.Add(vGeo);
            }
            return result;
        }
    }
}

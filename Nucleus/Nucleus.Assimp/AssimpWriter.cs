using Assimp;
using Nucleus.Base;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Assimp
{
    /// <summary>
    /// Writer class for saving to files in Assimp-supported formats
    /// </summary>
    [Serializable]
    public class AssimpWriter
    {
        /// <summary>
        /// Write the specified geometry to an FBX file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="geometry"></param>
        public bool WriteFBX(FilePath filePath, GeometryLayerTable geometry, bool binary = false)
        {
            var formatID = binary ? "fbx":"fbxa";
            
            /*var exporter = new AssimpContext();
            var supportedFormats = exporter.GetSupportedExportFormats();
            var format = supportedFormats.Where(f => f.FileExtension == "fbx");
            formatID = format.First().FormatId;*/

            return WriteFile(filePath, geometry, formatID);
        }

        /// <summary>
        /// Write the specified geometry to a file type determined by the Assimp format id string specified
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="geometry"></param>
        /// <param name="formatID"></param>
        public bool WriteFile(FilePath filePath, GeometryLayerTable geometry, string formatID)
        {
            var exporter = new AssimpContext();
            var scene = ToAssimp.Convert(geometry);
            return exporter.ExportFile(scene, filePath, formatID);
        }
    }
}

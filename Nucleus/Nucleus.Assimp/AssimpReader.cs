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
    /// Reader class for opening files from Assimp-supported formats
    /// </summary>
    public class AssimpReader
    {
        public VertexGeometryCollection ReadFile(FilePath filePath)
        {
            var importer = new AssimpContext();
            var scene = importer.ImportFile(filePath);
            return FromAssimp.Convert(scene);
        }
    }
}

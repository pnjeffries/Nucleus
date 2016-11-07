using FreeBuild.Meshing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC = Rhino.Geometry;
using FreeBuild.Geometry;

namespace FreeBuild.Rhino
{
    public class RhinoMeshBuilder : MeshBuilderBase
    {
        private RC.Mesh _Mesh = null;

        /// <summary>
        /// The mesh that is currently being built
        /// </summary>
        public RC.Mesh Mesh
        {
            get
            {
                if (_Mesh == null) _Mesh = new RC.Mesh();
                return _Mesh;
            }
            set { _Mesh = value; }
        }

        public override int AddVertex(Vector pt)
        {
            return Mesh.Vertices.Add(FBtoRC.Convert(pt));
        }

        public override int AddFace(int v1, int v2, int v3)
        {
            return Mesh.Faces.AddFace(v1, v2, v3);
        }

        public override int AddFace(int v1, int v2, int v3, int v4)
        {
            return Mesh.Faces.AddFace(v1, v2, v3, v4);
        }
    }
}

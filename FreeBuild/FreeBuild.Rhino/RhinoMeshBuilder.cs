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
    public class RhinoMeshBuilder : MeshBuilderBase<RC.Mesh>
    {

        public RhinoMeshBuilder()
        {
            _Mesh = new RC.Mesh();
        }

        public override int AddVertex(Vector pt)
        {
            return _Mesh.Vertices.Add(FBtoRC.Convert(pt));
        }

        public override int AddVertex(Vertex v)
        {
            int index = AddVertex(v.Position);
            v.VertexIndex = index;
            return index;
        }

        public override int AddFace(int v1, int v2, int v3)
        {
            return _Mesh.Faces.AddFace(v1, v2, v3);
        }

        public override int AddFace(int v1, int v2, int v3, int v4)
        {
            return _Mesh.Faces.AddFace(v1, v2, v3, v4);
        }
    }
}

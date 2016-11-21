using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;

namespace FreeBuild.Meshing
{
    /// <summary>
    /// Builder object for FreeBuild meshes
    /// </summary>
    public class MeshBuilder : MeshBuilderBase<Mesh>
    {
        public MeshBuilder()
        {
            _Mesh = new Mesh();
        }

        public override int AddFace(int v1, int v2, int v3)
        {
            MeshFace face = new MeshFace(_Mesh.Vertices[v1], _Mesh.Vertices[v2], _Mesh.Vertices[v3]);
            _Mesh.Faces.Add(face);
            return _Mesh.Faces.Count - 1;
        }

        public override int AddFace(int v1, int v2, int v3, int v4)
        {
            MeshFace face = new MeshFace(_Mesh.Vertices[v1], _Mesh.Vertices[v2], _Mesh.Vertices[v3], _Mesh.Vertices[v4]);
            _Mesh.Faces.Add(face);
            return _Mesh.Faces.Count - 1;
        }

        public override int AddVertex(Vertex v)
        {
            _Mesh.Vertices.Add(new Vertex(v));
            return _Mesh.Vertices.Count - 1;
        }

        public override int AddVertex(Vector pt)
        {
            _Mesh.Vertices.Add(new Vertex(pt));
            return _Mesh.Vertices.Count - 1;
        }
    }
}

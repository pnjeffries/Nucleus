using Nucleus.Meshing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using System.Windows.Media.Media3D;

namespace Nucleus.WPF
{
    public class WPFMeshBuilder : MeshBuilderBase<MeshGeometry3D>
    {
        public WPFMeshBuilder()
        {
            _Mesh = new MeshGeometry3D
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new System.Windows.Media.Int32Collection()
            };
        }

        public override int AddFace(int v1, int v2, int v3)
        {
            _Mesh.TriangleIndices.Add(v1);
            _Mesh.TriangleIndices.Add(v2);
            _Mesh.TriangleIndices.Add(v3);
            return _Mesh.TriangleIndices.Count - 1;
        }

        public override int AddFace(int v1, int v2, int v3, int v4)
        {
            // Resolved as two tris:
            AddFace(v1, v2, v4);
            return AddFace(v2, v3, v4);
        }

        public override int AddVertex(Vector pt)
        {
            Mesh.Positions.Add(new Point3D(pt.X, pt.Y, pt.Z));
            return Mesh.Positions.Count - 1;
        }

        public override int AddVertex(Vertex v)
        {
            int i = AddVertex(v.Position);
            v.Number = i;
            return i;
        }
    }
}

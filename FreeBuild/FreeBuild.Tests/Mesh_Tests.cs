using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    public static class Mesh_Tests
    {
        public static TimeSpan DelaunayTest(int size)
        {
            Stopwatch sw = new Stopwatch();
            Random rng = new Random();
            BoundingBox box = new BoundingBox(0, 100, 0, 100, 0, 100);

            Vector[] points = box.RandomPointsInside(rng, size);
            VertexCollection verts = new VertexCollection(points);
            sw.Start();
            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(verts);
            sw.Stop();

            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            Mesh.VoronoiFromDelaunay(verts, faces);
            sw2.Stop();

            Core.Print("Triangulation: " + sw.Elapsed.ToString() + " " + faces.Count + " Tris, Voronoi: " + sw2.Elapsed.ToString());
            return sw.Elapsed;
        }
    }
}

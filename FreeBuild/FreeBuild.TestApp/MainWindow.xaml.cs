using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeBuild.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DelaunayButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            BoundingBox box = new BoundingBox(0, 10, -10, 0, 0, 0);

            int size = 200;
            Geometry.Vector[] points = box.RandomPointsInside(rng, size);
            VertexCollection verts = new VertexCollection(points);
            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(verts);
            //Dictionary<Vertex, MeshFace> voronoi = Mesh.VoronoiFromDelaunay(verts, faces);
            //ShapeCollection geometry = new MeshFaceCollection(voronoi.Values).ExtractFaceBoundaries();
            ShapeCollection geometry = faces.ExtractFaceBoundaries();
            geometry.Add(new Cloud(verts.ExtractPoints()));
            DelaunayCanvas.Geometry = geometry;
        }

        private void VoronoiButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            BoundingBox box = new BoundingBox(0, 10, -10, 0, 0, 0);

            int size = 20;
            Geometry.Vector[] points = box.RandomPointsInside(rng, size);
            VertexCollection verts = new VertexCollection(points);
            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(verts, null, box, false);
            Dictionary<Vertex, MeshFace> voronoi = Mesh.VoronoiFromDelaunay(verts, faces);
            MeshFaceCollection outFaces = new MeshFaceCollection(voronoi.Values);
            PolyLine rect = PolyLine.Rectangle(0,-10,10, 0);
            outFaces = outFaces.TrimToPolygonXY(rect.Vertices);
            outFaces = outFaces.TrimToPolygonXY(rect.Vertices); //Test duplicate edges
            ShapeCollection geometry = outFaces.ExtractFaceBoundaries();
            //ShapeCollection geometry = faces.ExtractFaceBoundaries();
            geometry.Add(new Cloud(verts.ExtractPoints()));
            VoronoiCanvas.Geometry = geometry;
        }
    }
}

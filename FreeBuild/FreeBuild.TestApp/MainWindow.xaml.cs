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
using FreeBuild.Extensions;
using Microsoft.Win32;
using FreeBuild.DXF;

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
            VertexGeometryCollection geometry = faces.ExtractFaceBoundaries();
            geometry.Add(new Cloud(verts.ExtractPoints()));
            DelaunayCanvas.Geometry = geometry;
        }

        private void VoronoiButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = null;
            if (VoronoiSeedBox.Text.IsNumeric())
            {
                int seed = int.Parse(VoronoiSeedBox.Text);
                rng = new Random(seed);
            }
            else rng = new Random();

            BoundingBox box = new BoundingBox(0, 10, -10, 0, 0, 0);

            int size = 500;
            Geometry.Vector[] points = box.RandomPointsInside(rng, size);
            VertexCollection verts = new VertexCollection(points);
            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(verts, null, box, false);
            Dictionary<Vertex, MeshFace> voronoi = Mesh.VoronoiFromDelaunay(verts, faces);
            MeshFaceCollection outFaces = new MeshFaceCollection(voronoi.Values);
            PolyLine rect = PolyLine.Rectangle(0,-10,10, 0);
            outFaces = outFaces.TrimToPolygonXY(rect.Vertices);
            outFaces = outFaces.TrimToPolygonXY(rect.Vertices); //Test duplicate edges
            VertexGeometryCollection geometry = outFaces.ExtractFaceBoundaries();
            //ShapeCollection geometry = faces.ExtractFaceBoundaries();
            geometry.Add(new Cloud(verts.ExtractPoints()));
            VoronoiCanvas.Geometry = geometry;
        }

        private void DXFLoadButton_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Title = "Select .DXF file to open";
            openDialog.Filter = "AutoCAD DXF File (*.dxf)|*dxf";

            if (openDialog.ShowDialog(this) == true)
            {
                var dxfReader = new DXFReader();
                VertexGeometryCollection geometry = dxfReader.ReadDXF(openDialog.FileName);
                GeometryLayerTable layers = geometry.Layered();
                DXFCanvas.ViewBounds = new BoundingBox(geometry);
                DXFCanvas.Layers = layers;
                LayerBox.ItemsSource = layers;
            }
        }
    }
}

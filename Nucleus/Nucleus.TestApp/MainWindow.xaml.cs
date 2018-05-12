using Nucleus.Geometry;
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
using Nucleus.Extensions;
using Microsoft.Win32;
using Nucleus.DXF;
using Nucleus.WPF;
using Nucleus.Maths;
using Nucleus.Rendering;
using System.Threading;

namespace Nucleus.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GenerateSpider();
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
            VertexGeometryCollection geometry = new VertexGeometryCollection(faces.ExtractFaceBoundaries());
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
            PolyLine rect = PolyLine.Rectangle(0, -10, 10, 0);
            outFaces = outFaces.TrimToPolygonXY(rect.Vertices);
            outFaces = outFaces.TrimToPolygonXY(rect.Vertices); //Test duplicate edges
            VertexGeometryCollection geometry = new VertexGeometryCollection(outFaces.ExtractFaceBoundaries());
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

        private void GenerateSpider()
        {
            Random rng = new Random();
            //Set-up spider diagram:
            var data = new Dictionary<string, Interval>();
            data.Add("Daylighting", new Interval(rng.NextDouble()));
            data.Add("Overshadowing", new Interval(rng.NextDouble()));
            data.Add("Travel Distance", new Interval(rng.NextDouble()));
            data.Add("Base Loads", new Interval(rng.NextDouble()));
            data.Add("Modularity", new Interval(rng.NextDouble()));
            data.Add("Craneage", new Interval(rng.NextDouble()));
            var diagramData = new NamedDataSet("Option 1", data, Colour.RambollCyan);

            var data2 = new Dictionary<string, Interval>();
            data2.Add("Daylighting", new Interval(rng.NextDouble()));
            data2.Add("Overshadowing", new Interval(rng.NextDouble()));
            data2.Add("Travel Distance", new Interval(rng.NextDouble()));
            data2.Add("Base Loads", new Interval(rng.NextDouble()));
            data2.Add("Modularity", new Interval(rng.NextDouble()));
            data2.Add("Craneage", new Interval(rng.NextDouble()));
            var diagramData2 = new NamedDataSet("Option 2", data2, Colour.RambollLimeGreen);

            var data3 = new Dictionary<string, Interval>();
            data3.Add("Daylighting", new Interval(rng.NextDouble()));
            data3.Add("Overshadowing", new Interval(rng.NextDouble()));
            data3.Add("Travel Distance", new Interval(rng.NextDouble()));
            data3.Add("Base Loads", new Interval(rng.NextDouble()));
            data3.Add("Modularity", new Interval(rng.NextDouble()));
            data3.Add("Craneage", new Interval(rng.NextDouble()));
            var diagramData3 = new NamedDataSet("Option 3", data3, Colour.RambollMagenta);

            var data4 = new Dictionary<string, Interval>();
            data4.Add("Daylighting", new Interval(rng.NextDouble()));
            data4.Add("Overshadowing", new Interval(rng.NextDouble()));
            data4.Add("Travel Distance", new Interval(rng.NextDouble()));
            data4.Add("Base Loads", new Interval(rng.NextDouble()));
            data4.Add("Modularity", new Interval(rng.NextDouble()));
            data4.Add("Craneage", new Interval(rng.NextDouble()));
            var diagramData4 = new NamedDataSet("Option 4", data4, Colour.RambollWarmRed);


            var diagramDataCollection = new NamedDataSetCollection();
            diagramDataCollection.Add(diagramData);
            diagramDataCollection.Add(diagramData2);
            diagramDataCollection.Add(diagramData3);
            diagramDataCollection.Add(diagramData4);
            SpiderDiagram.SourceData = diagramDataCollection;
            BarChart.SourceData = diagramDataCollection;
        }

        private void GenerateSpiderButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateSpider();
        }

        private void GenerateBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DummyRun_Click(object sender, RoutedEventArgs e)
        {
            AsyncAlertLog log = new AsyncAlertLog();
            log.Window = this;
            LogViewer.DataContext = log;

            new Thread(() =>
                {
                    log.RaiseAlert("AL1", "Starting Process...");

                    for (int i = 0; i <= 1000; i++)
                    {
                        log.RaiseAlert("PROC", "Process Running...", (double)i / (double)1000);
                        Thread.Sleep(2);
                    }

                    log.RaiseAlert("WAR", "Warning: An Error may be about to occur!", Alerts.AlertLevel.Warning);

                    log.RaiseAlert("ERR", "An Error has occurred!", Alerts.AlertLevel.Error);

                    for (int i = 0; i <= 1000; i++)
                    {
                        log.RaiseAlert("PROC3", "Process 3 Running...", (double)i / (double)1000);
                        Thread.Sleep(3);
                    }

                    log.RaiseAlert("AL1", "Process Complete.");

                    log.RaiseAlert("PASS", "Condition 1: Passed", Alerts.AlertLevel.Pass);

                    log.RaiseAlert("FAIL", "Condition 2: Failed", Alerts.AlertLevel.Fail);

                }).Start();

            new Thread(() =>
            {
                log.RaiseAlert("AL1", "Starting Process...");


                for (int i = 0; i <= 1000; i++)
                {
                    log.RaiseAlert("PROC2", "Process 2 Running...", (double)i / (double)1000);
                    Thread.Sleep(3);
                }

                log.RaiseAlert("AL1", "Process Complete.");

            }).Start();
        }
    }
}

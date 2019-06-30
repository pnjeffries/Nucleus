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
using System.Diagnostics;

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

            Geometry.Line line = new Geometry.Line(0, 0, 1000, 1000);
            //GeometryVisualiserDialog.ShowDialog(line);
            VertexGeometry arc = new Geometry.Arc(new Circle(30, new Geometry.Vector(-50, -50)));
            //GeometryVisualiserDialog.ShowDialog(arc);
            
            VertexGeometryCollection coll = new VertexGeometryCollection(line, arc);
            //GeometryVisualiserDialog.ShowDialog(coll);
        }

        private void DelaunayButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            BoundingBox box = new BoundingBox(1, 9, -9, -1, 0, 0);

            int size = 100;
            //Geometry.Vector[] points = box.RandomPointsInside(rng, size);
            //VertexCollection verts = new VertexCollection(points);

            VertexCollection verts = new VertexCollection();
            //verts.Add(new Vertex(1, -1));
            int divs = 5;
            for (int i = 0; i <= divs; i++)
            {
                for (int j = 0; j <= divs; j++)
                {
                    Geometry.Vector pt = new Geometry.Vector(box.X.ValueAt(((double)i) / divs), box.Y.ValueAt(((double)j) / divs));
                    verts.Add(new Vertex(pt));
                }
            }

            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(verts);
            faces.Quadrangulate();
            //Dictionary<Vertex, MeshFace> voronoi = Mesh.VoronoiFromDelaunay(verts, faces);
            //ShapeCollection geometry = new MeshFaceCollection(voronoi.Values).ExtractFaceBoundaries();
            CurveCollection edges = faces.ExtractFaceBoundaries();
            VertexGeometryCollection geometry = new VertexGeometryCollection();
            foreach (var edge in edges)
            {
                var region = new PlanarRegion(edge);
                geometry.Add(region);
                region.Attributes = new GeometryAttributes(new Colour(128, 0, 255, 128));
                geometry.Add(edge);
                edge.Attributes = new GeometryAttributes(new Colour(64, 0, 0, 0));
            }
            geometry.Add(new Cloud(verts.ExtractPoints()));
            DelaunayCanvas.Geometry = geometry;
        }

        private void AnalysisMeshButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            BoundingBox box = new BoundingBox(1, 9, -9, -1, 0, 0);

            int size = 100;
            //Geometry.Vector[] points = box.RandomPointsInside(rng, size);
            //VertexCollection verts = new VertexCollection(points);

            VertexCollection verts = new VertexCollection();
            //verts.Add(new Vertex(1, -1));
            int divs = 5;
            for (int i = 0; i <= divs; i++)
            {
                for (int j = 0; j <= divs; j++)
                {
                    Geometry.Vector pt = new Geometry.Vector(box.X.ValueAt(((double)i) / divs), box.Y.ValueAt(((double)j) / divs));
                    verts.Add(new Vertex(pt));
                }
            }

            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(verts);
            faces.Quadrangulate();
            //Dictionary<Vertex, MeshFace> voronoi = Mesh.VoronoiFromDelaunay(verts, faces);
            //ShapeCollection geometry = new MeshFaceCollection(voronoi.Values).ExtractFaceBoundaries();
            CurveCollection edges = faces.ExtractFaceBoundaries();
            VertexGeometryCollection geometry = new VertexGeometryCollection();
            foreach (var edge in edges)
            {
                var region = new PlanarRegion(edge);
                geometry.Add(region);
                region.Attributes = new GeometryAttributes(new Colour(128, 0, 255, 128));
                geometry.Add(edge);
                edge.Attributes = new GeometryAttributes(new Colour(64, 0, 0, 0));
            }
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
                    log.RaiseAlert("PROC", "Process Complete", 1);

                    log.RaiseAlert("WAR", "Warning: An Error may be about to occur!", Alerts.AlertLevel.Warning);

                    log.RaiseAlert("ERR", "An Error has occurred!", Alerts.AlertLevel.Error);

                    for (int i = 0; i <= 1000; i++)
                    {
                        log.RaiseAlert("PROC3", "Process 3 Running...", (double)i / (double)1000);
                        Thread.Sleep(3);
                    }
                    log.RaiseAlert("PROC3", "Process 3 Complete", 1);

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
                log.RaiseAlert("PROC2", "Process 2 Complete", 1);

                log.RaiseAlert("AL1", "Process Complete.");

            }).Start();
        }

        private void DelaunayRefineButton_Click(object sender, RoutedEventArgs e)
        {
            DelaunayRefine(1);
        }

        private void DelaunayRefine(int scenario)
        {
            var sw = new Stopwatch();
            sw.Start();
            Random rng = new Random();
            BoundingBox box = new BoundingBox(0, 10, -10, 0, 0, 0);

            int size = 50;
            Geometry.Vector[] points;
            if (scenario == 1)
                points = box.RandomPointsInside(rng, size);
            else if (scenario == 2)
            {
                points = new Geometry.Vector[]
                {
                    new Geometry.Vector(10,-9.5),
                    new Geometry.Vector(0,-9.5),
                    new Geometry.Vector(2,-0.5),
                    new Geometry.Vector(8,-0.5)
                    //new Geometry.Vector(4.5,-0.5),
                    //new Geometry.Vector(5.5,-0.5)
                };
            }
            else
            {
                points = new Geometry.Vector[]
                {
                    new Geometry.Vector(10, -6),
                    new Geometry.Vector(10,-2),
                    new Geometry.Vector(0, -6),
                    new Geometry.Vector(0, -6)
                };
            }
            VertexCollection verts = new VertexCollection(points);
            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(verts);
            faces.Quadrangulate();
            faces = faces.Refine(0.2);
            //Dictionary<Vertex, MeshFace> voronoi = Mesh.VoronoiFromDelaunay(verts, faces);
            //ShapeCollection geometry = new MeshFaceCollection(voronoi.Values).ExtractFaceBoundaries();
            CurveCollection edges = faces.ExtractFaceBoundaries();
            VertexGeometryCollection geometry = new VertexGeometryCollection();
            foreach (var edge in edges)
            {
                var region = new PlanarRegion(edge);
                geometry.Add(region);
                region.Attributes = new GeometryAttributes(new Colour(128, 0, 255, 128));
                geometry.Add(edge);
                edge.Attributes = new GeometryAttributes(new Colour(64, 0, 0, 0));
            }
            geometry.Add(new Cloud(verts.ExtractPoints()));
            sw.Stop();
            MeshingTimeText.Text = "Completed: " + faces.Count + " faces in " + sw.Elapsed;
            DelaunayCanvas.Geometry = geometry;
        }

        private void RefineQuad_Click(object sender, RoutedEventArgs e)
        {
            DelaunayRefine(2);
        }

        private void RefineTri_Click(object sender, RoutedEventArgs e)
        {
            DelaunayRefine(3);
        }
    }
}

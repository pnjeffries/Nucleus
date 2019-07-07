using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.IO;
using Nucleus.Meshing;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for GeometryVisualiserDialog.xaml
    /// </summary>
    public partial class GeometryVisualiserDialog : Window
    {
        #region Static Fields

        // Temporary store for objects to allow overlaying
        private static VertexGeometryCollection _Store = null;

        /// <summary>
        /// The filepath where geometry is temporarily stored
        /// </summary>
        private static readonly FilePath _StorePath = new FilePath("C:/TEMP/DebugVisualisationStore.nucleus");

        #endregion

        #region Constructors

        public GeometryVisualiserDialog()
        {
            InitializeComponent();
        }

        public GeometryVisualiserDialog(object visualise) : this()
        {
            VertexGeometryCollection geometry = new VertexGeometryCollection();


            if (_StorePath.Exists)
            {
                try
                {
                    Stream stream = new FileStream(_StorePath,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                    stream.Seek(0, SeekOrigin.Begin);

                    IFormatter formatter = new BinaryFormatter();
                    formatter.Binder = new CustomSerializationBinder();
                    var storedGeo = formatter.Deserialize(stream) as VertexGeometryCollection;
                    stream.Close();

                    if (storedGeo != null) geometry.AddRange(storedGeo);
                }
                catch { }
            }
        
            if (visualise is MeshFace) visualise = new MeshFaceCollection((MeshFace)visualise);
            //else if (visualise is Mesh) visualise = ((Mesh)visualise).Faces;

            if (visualise is IList<VertexGeometry>) geometry.TryAddRange((IList<VertexGeometry>)visualise);
            else if (visualise is IList<Curve>) geometry.TryAddRange((IList<Curve>)visualise);
            else if (visualise is VertexGeometry) geometry.TryAdd((VertexGeometry)visualise);
            else if (visualise is MeshFaceCollection)
            {
                var faces = (MeshFaceCollection)visualise;
                CurveCollection edges = faces.ExtractFaceBoundaries();
                foreach (var edge in edges)
                {
                    var region = new PlanarRegion(edge);
                    geometry.Add(region);
                    region.Attributes = new GeometryAttributes(new Colour(128, 0, 255, 128));
                    geometry.Add(edge);
                    edge.Attributes = new GeometryAttributes(new Colour(64, 0, 0, 0));
                }
            }
            else if (visualise is IList<Vertex>)
            {
                var verts = (IList<Vertex>)visualise;
                var cloud = new Cloud(verts.GetPositions());
                geometry.Add(cloud);
            }
            else if (visualise is IList<Geometry.Vector>)
            {
                var v = (IList<Geometry.Vector>)visualise;
                var cloud = new Cloud(v);
                geometry.Add(cloud);
            }
            else if (visualise is MeshDivisionEdge)
            {
                var mDE = (MeshDivisionEdge)visualise;
                var line = new Geometry.Line(mDE.Start, mDE.End);
                geometry.Add(line);
                if (mDE.Vertices != null && mDE.Vertices.Count > 0)
                {
                    var cloud = new Cloud(mDE.Vertices.GetPositions());
                    geometry.Add(cloud);
                }
            }
            else if (visualise is IList<MeshDivisionEdge>)
            {
                foreach (var mDC in (IList<MeshDivisionEdge>)visualise)
                {
                    var mDE = (MeshDivisionEdge)visualise;
                    var line = new Geometry.Line(mDE.Start, mDE.End);
                    geometry.Add(line);
                    if (mDE.Vertices != null && mDE.Vertices.Count > 0)
                    {
                        var cloud = new Cloud(mDE.Vertices.GetPositions());
                        geometry.Add(cloud);
                    }
                }
            }
            // TODO: Convert other types to vertexgeometry


            BoundingBox bBox = geometry.BoundingBox;
            /*MinX.Text = bBox.MinX.ToString();
            MaxX.Text = bBox.MaxX.ToString();
            MinY.Text = bBox.MinY.ToString();
            MaxY.Text = bBox.MaxY.ToString();*/

            Canvas.CurveThickness = 0.005 * bBox.SizeVector.Magnitude();

            Canvas.Geometry = geometry;
            /*var xDivs = bBox.X.ReasonableDivisions(10);
            var yDivs = bBox.Y.ReasonableDivisions(10);

            foreach (double x in xDivs)
            {
                var line = new Geometry.Line(x, bBox.MinY - 10, x, bBox.MaxY + 10);
                line.Attributes = new GeometryAttributes(Rendering.Colour.Green, 0.1);
                Canvas.AddContents(line);
            }*/
        }

        #endregion

        #region Static Methods

        public static void ShowDialog(object visualise)
        {
            var dialog = new GeometryVisualiserDialog(visualise);
            dialog.ShowDialog();
        }

        #endregion

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var pt = Mouse.GetPosition(Canvas);
            this.Title = string.Format("({0},{1})", pt.X, -pt.Y);
        }

        private void StoreMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Stream stream = new FileStream(_StorePath,
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, Canvas.Geometry);

                    stream.Flush();
                    stream.Close();
                }
            }
            catch
            {
                //TODO: Notify user of error
            }
        }

        private void ClearStoreMI_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(_StorePath);
        }
    }
}

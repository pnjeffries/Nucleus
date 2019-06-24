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
using System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for GeometryVisualiserDialog.xaml
    /// </summary>
    public partial class GeometryVisualiserDialog : Window
    {
        #region Constructors

        public GeometryVisualiserDialog()
        {
            InitializeComponent();
        }

        public GeometryVisualiserDialog(object visualise) : this()
        {
            VertexGeometryCollection geometry = null;
            if (visualise is VertexGeometryCollection) geometry = (VertexGeometryCollection)visualise;
            else geometry = new VertexGeometryCollection();

            if (visualise is VertexGeometry) geometry.Add((VertexGeometry)visualise);
            // TODO: Convert other types to vertexgeometry
            
            Canvas.Geometry = geometry;
        }

        #endregion

        #region Static Methods

        public static void ShowDialog(object visualise)
        {
            var dialog = new GeometryVisualiserDialog(visualise);
            dialog.ShowDialog();
        }

        #endregion
    }
}

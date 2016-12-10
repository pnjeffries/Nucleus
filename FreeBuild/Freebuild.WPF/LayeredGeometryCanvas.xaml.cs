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

namespace FreeBuild.WPF
{
    /// <summary>
    /// Interaction logic for LayeredGeometryCanvas.xaml
    /// </summary>
    public partial class LayeredGeometryCanvas : UserControl
    {

        #region Events

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback LayersChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseLayersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LayersChanged?.Invoke(d, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Static callback function to raise a ProfilesChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnLayersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LayeredGeometryCanvas)d).RaiseLayersChanged(d, e);
            //((LayeredGeometryCanvas)d).RefreshContents();
        }

        /// <summary>
        /// Profiles dependency property
        /// </summary>
        public static DependencyProperty LayersProperty =
            DependencyProperty.Register("Layers", typeof(GeometryLayerTable), typeof(LayeredGeometryCanvas),
                 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnLayersChanged)));

        /// <summary>
        /// The profiles to be displayed on this canvas
        /// </summary>
        public GeometryLayerTable Layers
        {
            get { return (GeometryLayerTable)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static void OnCurveThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Curve thickness dependency property
        /// </summary>
        public static DependencyProperty CurveThicknessProperty =
            DependencyProperty.Register("CurveThickness", typeof(double), typeof(LayeredGeometryCanvas),
            new FrameworkPropertyMetadata(0.1, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnCurveThicknessChanged)));

        /// <summary>
        /// The default stroke thickness of curves drawn on this canvas
        /// </summary>
        public double CurveThickness
        {
            get { return (double)GetValue(CurveThicknessProperty); }
            set { SetValue(CurveThicknessProperty, value); }
        }

        public static void OnViewBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LayeredGeometryCanvas canvas = ((LayeredGeometryCanvas)d);
            canvas.AdaptToBounds();
        }

        public static DependencyProperty ViewBoundsProperty =
            DependencyProperty.Register("ViewBounds", typeof(BoundingBox), typeof(LayeredGeometryCanvas),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnViewBoundsChanged)));

        /// <summary>
        /// The bounds within which geometry should be drawn on this canvas
        /// </summary>
        public BoundingBox ViewBounds
        {
            get { return (BoundingBox)GetValue(ViewBoundsProperty); }
            set { SetValue(ViewBoundsProperty, value); }
        }

        #endregion

        #region Constructors

        public LayeredGeometryCanvas()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }

        #endregion

        #region Methods

        public void AdaptToBounds()
        {
            if (ViewBounds != null)
            {
                BoundingBox bBox = new BoundingBox(ViewBounds);
                bBox.Scale(1.1);
                ItemsControl.Width = bBox.SizeX;
                ItemsControl.Height = bBox.SizeY;
                ItemsControl.RenderTransform = new TranslateTransform(-bBox.MinX, bBox.MaxY);
                CurveThickness = bBox.MaxSize * 0.0012;
            }
        }

        #endregion
    }
}

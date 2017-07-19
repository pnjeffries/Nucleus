using Nucleus.Maths;
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
using NG = Nucleus.Geometry;

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for SpiderDiagram.xaml
    /// </summary>
    public partial class SpiderDiagram : UserControl
    {
        #region Properties

        /// <summary>
        /// Static callback function when data property changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static object OnSourceDataChanged(DependencyObject d, object baseValue)
        {
            ((SpiderDiagram)d).Refresh();
            return baseValue;
        }

        /// <summary>
        /// Data dependency property
        /// </summary>
        public static DependencyProperty SourceDataProperty =
            DependencyProperty.Register("SourceData", typeof(DiagramDataCollection), typeof(SpiderDiagram),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, null, new CoerceValueCallback(OnSourceDataChanged)));

        /// <summary>
        /// The data to display
        /// </summary>
        public DiagramDataCollection SourceData
        {
            get { return (DiagramDataCollection)GetValue(SourceDataProperty); }
            set { SetValue(SourceDataProperty, value); }
        }

        #endregion

        #region Constructors

        public SpiderDiagram()
        {
            InitializeComponent();

            Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh and redraw the diagram
        /// </summary>
        public void Refresh()
        {
            MainCanvas.Children.Clear(); //Clear canvas

            if (SourceData != null)
            {
                //Draw axes:
                IList<string> axes = SourceData.GetAllKeys();
                for(int i = 0; i < axes.Count; i++)
                {
                    string axisName = axes[i];
                    double angle = ((double)i) * (Math.PI * 2 / axes.Count);
                    Line axis = new Line();
                    axis.X2 = Math.Sin(angle) * 100.0;
                    axis.Y2 = -Math.Cos(angle) * 100.0;
                    axis.Stroke = Brushes.Black;
                    axis.StrokeThickness = 1;
                    axis.Opacity = 0.6;

                    MainCanvas.Children.Add(axis);

                    TextBlock tB = new TextBlock();
                    tB.Text = axisName;
                    tB.Width = 97;
                    tB.FontSize = 6;
                    if (angle <= Math.PI)
                    {
                        tB.TextAlignment = TextAlignment.Right;
                        var rTrans = new RotateTransform(angle * 180 / Math.PI - 90);
                        rTrans.CenterX = 0;
                        rTrans.CenterY = 0;
                        tB.RenderTransform = rTrans;
                    }
                    else
                    {
                        Canvas.SetRight(tB, 0);
                        tB.TextAlignment = TextAlignment.Left;
                        var rTrans = new RotateTransform(angle * 180 / Math.PI + 90);
                        rTrans.CenterX = 97;
                        rTrans.CenterY = 0;
                        tB.RenderTransform = rTrans;
                    }
                    tB.Opacity = 0.8;

                    MainCanvas.Children.Add(tB);
                }

                foreach (var dataSet in SourceData)
                {
                    Polygon pgon = new Polygon();
                    Color color = FBtoWPF.Convert(dataSet.Colour);
                    pgon.Stroke = new SolidColorBrush(color);
                    pgon.StrokeThickness = 1;
                    pgon.StrokeLineJoin = PenLineJoin.Bevel;
                    Color outColor = color;
                    Color inColor = color;
                    outColor.A = 100;
                    inColor.A = 50;
                        pgon.Fill = new RadialGradientBrush(inColor, outColor);
                    pgon.Opacity = 0.75;
                    pgon.ToolTip = dataSet.Name;
                    PointCollection outerPts = new PointCollection();

                    for (int i = 0; i < axes.Count; i++)
                    {
                        string axisName = axes[i];
                        double angle = ((double)i) * (Math.PI * 2 / axes.Count);

                        double value = 0;

                        if (dataSet.Data.ContainsKey(axisName))
                        {
                            value = dataSet.Data[axisName].End * 100;
                        }

                        Point point = new Point(Math.Sin(angle) * value, -Math.Cos(angle) * value);
                        outerPts.Add(point);

                        
                    }
                    pgon.Points = outerPts;

                    MainCanvas.Children.Add(pgon);
                }
            }
        }

        #endregion
    }
}

using Nucleus.Base;
using Nucleus.Maths;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            ((SpiderDiagram)d).WatchSource(baseValue);
            ((SpiderDiagram)d).Refresh();
            return baseValue;
        }

        /// <summary>
        /// Data dependency property
        /// </summary>
        public static DependencyProperty SourceDataProperty =
            DependencyProperty.Register("SourceData", typeof(INamedDataSetCollection), typeof(SpiderDiagram),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, null, new CoerceValueCallback(OnSourceDataChanged)));

        /// <summary>
        /// The data to display.  Should be a collection of NamedDataSets or derivative types
        /// </summary>
        public INamedDataSetCollection SourceData
        {
            get { return (INamedDataSetCollection)GetValue(SourceDataProperty); }
            set { SetValue(SourceDataProperty, value); }
        }

        public static DependencyProperty AxisRangesProperty =
            DependencyProperty.Register("AxisRanges", typeof(NamedDataSet), typeof(SpiderDiagram),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, null));

        /// <summary>
        /// The range of values on each axis.  Determines scaling of the plot polygon.
        /// </summary>
        public NamedDataSet AxisRanges
        {
            get { return (NamedDataSet)GetValue(AxisRangesProperty); }
            set { SetValue(AxisRangesProperty, value); }
        }

        public static DependencyProperty FillOpacityProperty =
            DependencyProperty.Register("FillOpacity", typeof(double), typeof(SpiderDiagram));

        /// <summary>
        /// The opacity of the spider diagram fill gradient
        /// </summary>
        public double FillOpacity
        {
            get { return (double)GetValue(FillOpacityProperty); }
            set { SetValue(FillOpacityProperty, value); }
        }

        public static DependencyProperty ColourBrightnessCapProperty =
            DependencyProperty.Register("ColourBrightnessCap", typeof(double), typeof(SpiderDiagram),
                new FrameworkPropertyMetadata(1.0));

        /// <summary>
        /// The maximum permissible brightness value for line colours.
        /// Dataset display colours which exceed this brightness will be
        /// automatically adjusted to help them to stand out against the
        /// white background.
        /// </summary>
        public double ColourBrightnessCap
        {
            get { return (double)GetValue(ColourBrightnessCapProperty); }
            set { SetValue(ColourBrightnessCapProperty, value); }
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

        private void WatchSource(object newValue)
        {
            INamedDataSetCollection dColl = SourceData;
            if (dColl != null && dColl is INotifyCollectionChanged)
            {
                INotifyCollectionChanged nColl = (INotifyCollectionChanged)dColl;
                nColl.CollectionChanged -= SourceData_CollectionChanged;
            }
            if (newValue != null && newValue is INotifyCollectionChanged)
            {
                INotifyCollectionChanged nColl = (INotifyCollectionChanged)newValue;
                nColl.CollectionChanged += SourceData_CollectionChanged;
            }
        }

        private void SourceData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Refresh and redraw the diagram
        /// </summary>
        public void Refresh()
        {
            MainCanvas.Children.Clear(); //Clear canvas

            if (SourceData != null)
            {
                // Size of the diagram (in canvas units):
                double radius = 100;

                NamedDataSet axisRanges = AxisRanges;
                if (axisRanges == null)
                {
                    axisRanges = SourceData.GetValueRanges();
                }

                //Draw axes:
                List<string> axes = SourceData.GetAllKeys().ToList();
                axes.Sort();

                for (int i = 0; i < axes.Count; i++)
                {
                    string axisName = axes[i];
                    double angle = ((double)i) * (Math.PI * 2 / axes.Count);
                    Line axis = new Line();
                    axis.X2 = Math.Sin(angle) * radius;
                    axis.Y2 = -Math.Cos(angle) * radius;
                    axis.Stroke = Brushes.Black;
                    axis.StrokeThickness = 1;
                    axis.Opacity = 0.6;

                    MainCanvas.Children.Add(axis);

                    TextBlock tB = new TextBlock();
                    tB.Text = axisName;
                    tB.Width = radius - 3;
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
                        rTrans.CenterX = radius - 3;
                        rTrans.CenterY = 0;
                        tB.RenderTransform = rTrans;
                    }
                    tB.Opacity = 0.8;

                    MainCanvas.Children.Add(tB);
                }

                // Draw map:

                foreach (NamedDataSet dataSet in SourceData)
                {
                    Polygon pgon = new Polygon();
                    Color color = FBtoWPF.Convert(dataSet.Colour.CapBrightness(ColourBrightnessCap));
                    pgon.Stroke = new SolidColorBrush(color);
                    pgon.StrokeThickness = 2;
                    pgon.StrokeLineJoin = PenLineJoin.Bevel;
                    Color outColor = color;
                    Color inColor = color;
                    outColor.A = 100;
                    inColor.A = 100;
                    var fill = new RadialGradientBrush(inColor, outColor);
                    fill.Opacity = FillOpacity;
                    pgon.Fill = fill;
                    pgon.Opacity = 0.95;
                    pgon.ToolTip = dataSet.Name;
                    PointCollection outerPts = new PointCollection();
                    PointCollection innerPts = new PointCollection();
                    bool hollow = false;

                    for (int i = 0; i < axes.Count; i++)
                    {
                        string axisName = axes[i];
                        double angle = ((double)i) * (Math.PI * 2 / axes.Count);

                        double value = 0;
                        double value2 = 0;

                        if (dataSet.Data.ContainsKey(axisName))
                        {
                            Interval range;
                            if (axisRanges.Data.ContainsKey(axisName)) range = axisRanges.Data[axisName];
                            else range = new Interval(0, 1.0);
                            if (range.IsSingularity) range = new Interval(range.Start - 0.1, range.End);

                            Interval data = dataSet.Data[axisName];
                            value = range.ParameterOf(data.End) * radius;
                            value2 = range.ParameterOf(data.Start) * radius;
                            if (value2 > 0.001) hollow = true;
                        }

                        Point point = new Point(Math.Sin(angle) * value, -Math.Cos(angle) * value);
                        outerPts.Add(point);
                        Point point2 = new Point(Math.Sin(angle) * value2, -Math.Cos(angle) * value2);
                        innerPts.Add(point2);
                    }
                    pgon.Points = outerPts;
                    //TODO: Include 'hollow' polygons that have minimum values

                    MainCanvas.Children.Add(pgon);
                }
            }
        }

        #endregion
    }
}

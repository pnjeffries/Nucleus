using Nucleus.Extensions;
using Nucleus.Maths;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// A custom canvas that is used for drawing line graphs
    /// </summary>
    public class LineGraphCanvas : Canvas
    {
        #region Properties

        /// <summary>
        /// Static callback function when data property changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LineGraphCanvas)d).Refresh();
        }

        /// <summary>
        /// Data dependency property
        /// </summary>
        public static DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(GraphLineDataCollection), typeof(LineGraphCanvas),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// The data to display
        /// </summary>
        public GraphLineDataCollection Data
        {
            get { return (GraphLineDataCollection)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// XAxisLabels dependency property
        /// </summary>
        public static DependencyProperty XAxisLabelsProperty =
            DependencyProperty.Register("XAxisLabels", typeof(IDictionary<double, string>), typeof(LineGraphCanvas),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// A set of labels to display along the X-Axis.
        /// If not populated these will be automatically generated.
        /// </summary>
        public IDictionary<double, string> XAxisLabels
        {
            get { return (IDictionary<double, string>)GetValue(XAxisLabelsProperty); }
            set { SetValue(XAxisLabelsProperty, value); }
        }

        /// <summary>
        /// YAxisLabels dependency property
        /// </summary>
        public static DependencyProperty YAxisLabelsProperty =
            DependencyProperty.Register("YAxisLabels", typeof(IDictionary<double, string>), typeof(LineGraphCanvas),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// A set of labels to display along the Y-Axis.
        /// If not populated these will be automatically generated.
        /// </summary>
        public IDictionary<double, string> YAxisLabels
        {
            get { return (IDictionary<double, string>)GetValue(YAxisLabelsProperty); }
            set { SetValue(YAxisLabelsProperty, value); }
        }

        /// <summary>
        /// X-axis Label Suffix Dependency Property
        /// </summary>
        public static DependencyProperty XAxisLabelSuffixProperty =
            DependencyProperty.Register("XAxisLabelSuffix", typeof(string), typeof(LineGraphCanvas));

        /// <summary>
        /// The suffix to be applied to auto-generated labels on the X-Axis
        /// </summary>
        public string XAxisLabelSuffix
        {
            get { return (string)GetValue(XAxisLabelSuffixProperty); }
            set { SetValue(XAxisLabelSuffixProperty, value); }
        }

        /// <summary>
        /// X-axis Label Suffix Dependency Property
        /// </summary>
        public static DependencyProperty YAxisLabelSuffixProperty =
            DependencyProperty.Register("YAxisLabelSuffix", typeof(string), typeof(LineGraphCanvas));

        /// <summary>
        /// The suffix to be applied to auto-generated labels on the X-Axis
        /// </summary>
        public string YAxisLabelSuffix
        {
            get { return (string)GetValue(YAxisLabelSuffixProperty); }
            set { SetValue(YAxisLabelSuffixProperty, value); }
        }

        /// <summary>
        /// If true, the keys will be used for Y and the values for X,
        /// else the other way around.
        /// </summary>
        public bool FlipXY { get; set; } = false;

        /// <summary>
        /// The target number of grid divisions when auto-generating gridlines.
        /// </summary>
        public int GridDivisions { get; set; } = 8;

        #endregion

        #region Methods

        /// <summary>
        /// Create a point based on the specified key and value variables,
        /// mapped to the current graph space.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="xFactor"></param>
        /// <param name="yFactor"></param>
        /// <returns></returns>
        protected Point ToPoint(double key, double value, Interval xRange, Interval yRange)
        {
            double height = ActualHeight;
            double width = ActualWidth;
            double x, y;
            if (FlipXY)
            {
                x = value;
                y = key;
            }
            else
            {
                x = key;
                y = value;
            }
            x = xRange.ParameterOf(x) * width;
            y = (1 - yRange.ParameterOf(y)) * height;
            return new Point(x, y);
        }

        /// <summary>
        /// Generate a set of labels to be displayed in the absense of any preset values
        /// </summary>
        /// <param name="range"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        protected IDictionary<double, string> GenerateAxisLabels(Interval range, string suffix = "")
        {
            var result = new SortedList<double, string>();
            var values = range.ReasonableDivisions(GridDivisions);
            foreach (double value in values)
            {
                result.Add(value, value.ToString() + (suffix ?? ""));
            }
            return result;
        }

        /// <summary>
        /// Refresh the graph contents
        /// </summary>
        public void Refresh()
        {
            Children.Clear();

            double width = ActualWidth;
            double height = ActualHeight;

            if (Data != null && Data.Count > 0 && height > 0 && width > 0)
            {
                // Calculate transform:
                Interval xRange, yRange;
                if (FlipXY)
                {
                    xRange = Data.ValueRange.Include(0);
                    yRange = Data.KeyRange.Include(0);
                }
                else
                {
                    yRange = Data.ValueRange.Include(0);
                    xRange = Data.KeyRange.Include(0);
                }

                if (xRange.IsValid && yRange.IsValid)
                {
                    //Extend range to include any axis label presets:
                    if (YAxisLabels != null)
                        yRange = yRange.Union(YAxisLabels.KeyRange());

                    if (XAxisLabels != null)
                        xRange = xRange.Union(XAxisLabels.KeyRange());


                    // Draw grid:

                    var xAxisLabels = XAxisLabels;
                    var yAxisLabels = YAxisLabels;
                    if (xAxisLabels == null) xAxisLabels = GenerateAxisLabels(xRange, XAxisLabelSuffix);
                    if (yAxisLabels == null) yAxisLabels = GenerateAxisLabels(yRange, YAxisLabelSuffix);


                    // Y-axis labels:
                    if (xAxisLabels != null)
                    {
                        foreach (KeyValuePair<double, string> kvp in xAxisLabels)
                        {
                            double x = xRange.ParameterOf(kvp.Key) * width;
                            Line gridLine = new Line();
                            gridLine.X1 = x;
                            gridLine.X2 = x;
                            gridLine.Y1 = 0;
                            gridLine.Y2 = height;
                            gridLine.Stroke = Brushes.Black;
                            gridLine.Opacity = 0.25;
                            Children.Add(gridLine);

                            TextBlock tB = new TextBlock();
                            tB.Text = kvp.Value;
                            tB.TextAlignment = TextAlignment.Right;
                            FormattedText fT = new FormattedText(kvp.Value, CultureInfo.CurrentCulture, tB.FlowDirection,
                            new Typeface(tB.FontFamily, tB.FontStyle, tB.FontWeight, tB.FontStretch), tB.FontSize, tB.Foreground);

                            Children.Add(tB);
                            Canvas.SetLeft(tB, x - fT.Width/2);
                            Canvas.SetTop(tB, (1 - yRange.ParameterOf(0)) * height + 5);
                        }
                    }

                    // Y-axis labels:
                    if (yAxisLabels != null)
                    {
                        foreach (KeyValuePair<double, string> kvp in yAxisLabels)
                        {
                            double y = (1 - yRange.ParameterOf(kvp.Key)) * height;
                            Line gridLine = new Line();
                            gridLine.X1 = 0;
                            gridLine.X2 = width;
                            gridLine.Y1 = y;
                            gridLine.Y2 = y;
                            gridLine.Stroke = Brushes.Black;
                            gridLine.Opacity = 0.25;
                            Children.Add(gridLine);

                            TextBlock tB = new TextBlock();
                            tB.Text = kvp.Value;
                            tB.TextAlignment = TextAlignment.Right;
                            Children.Add(tB);
                            Canvas.SetRight(tB, (1 - xRange.ParameterOf(0)) * width + 5);
                            Canvas.SetTop(tB, y - tB.FontSize / 2);
                        }
                    }


                    // X-axis:
                    Line xAxis = new Line();
                    xAxis.X1 = 0;
                    xAxis.X2 = width;
                    xAxis.Y1 = (1 - yRange.ParameterOf(0)) * height;
                    xAxis.Y2 = xAxis.Y1;
                    xAxis.Stroke = Brushes.Black;
                    xAxis.StrokeThickness = 2.0;
                    Children.Add(xAxis);

                    // Y-axis:
                    Line yAxis = new Line();
                    yAxis.X1 = xRange.ParameterOf(0) * width;
                    yAxis.X2 = yAxis.X1;
                    yAxis.Y1 = 0;
                    yAxis.Y2 = height;
                    yAxis.Stroke = Brushes.Black;
                    yAxis.StrokeThickness = 2.0;
                    Children.Add(yAxis);

                    

                    // Draw graph lines:
                    for (int i = 0; i < Data.Count; i++)
                    {
                        GraphLineData data = Data[i];
                        Colour col = Colour.FromHSV(i * 360.0 / Data.Count, 255, 255);

                        Polyline pLine = new Polyline();
                        pLine.Stroke = new SolidColorBrush(ToWPF.Convert(col));
                        //pLine.StrokeThickness = 1.0;
                        pLine.ToolTip = data.Name;
                        ToolTipService.SetInitialShowDelay(pLine, 0);
                        ToolTipService.SetShowDuration(pLine, 60000);

                        //Generate points:
                        PointCollection points = new PointCollection();
                        var graph = data.Data;
                        foreach (KeyValuePair<double, Interval> kvp in graph)
                        {
                            points.Add(ToPoint(kvp.Key, kvp.Value.End, xRange, yRange));
                        }
                        if (graph.IsEnvelope)
                        {
                            foreach (KeyValuePair<double, Interval> kvp in graph.Reverse())
                            {
                                points.Add(ToPoint(kvp.Key, kvp.Value.Start, xRange, yRange));
                            }
                        }

                        pLine.Points = points;

                        Children.Add(pLine);
                    }
                }
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Refresh();
        }

        #endregion

    }
}

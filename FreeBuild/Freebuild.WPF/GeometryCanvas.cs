using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using FB = FreeBuild.Geometry;
using WPFM = System.Windows.Media;

namespace FreeBuild.WPF
{
    public class GeometryCanvas : Canvas
    {
        #region Events

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback GeometryChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GeometryChanged?.Invoke(d, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Static callback function to raise a ProfilesChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GeometryCanvas)d).RaiseGeometryChanged(d, e);
            ((GeometryCanvas)d).RefreshContents();
        }

        /// <summary>
        /// Profiles dependency property
        /// </summary>
        public static DependencyProperty GeometryProperty =
            DependencyProperty.Register("Geometry", typeof(VertexGeometryCollection), typeof(GeometryCanvas),
                 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnGeometryChanged)));

        /// <summary>
        /// The profiles to be displayed on this canvas
        /// </summary>
        public VertexGeometryCollection Geometry
        {
            get { return (VertexGeometryCollection)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }


        public static void OnRedrawChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GeometryCanvas)d).RefreshContents();
        }

        public static DependencyProperty CurveThicknessProperty =
            DependencyProperty.Register("CurveThickness", typeof(double), typeof(GeometryCanvas),
            new FrameworkPropertyMetadata(0.01, FrameworkPropertyMetadataOptions.None));//,
                //new PropertyChangedCallback(OnCurveThicknessChanged)));

        /// <summary>
        /// The default stroke thickness of curves drawn on this canvas
        /// </summary>
        public double CurveThickness
        {
            get { return (double)GetValue(CurveThicknessProperty); }
            set { SetValue(CurveThicknessProperty, value); }
        }

        /// <summary>
        /// Default Brush dependency property
        /// </summary>
        public static DependencyProperty DefaultBrushProperty =
            DependencyProperty.Register("DefaultBrush", typeof(Brush), typeof(GeometryCanvas),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnRedrawChange)));

        /// <summary>
        /// The default brush used to draw objects on this canvas
        /// </summary>
        public Brush DefaultBrush
        {
            get { return (Brush)GetValue(DefaultBrushProperty); }
            set { SetValue(DefaultBrushProperty, value); }
        }

        /// <summary>
        /// The default diameter of points drawn on this canvas
        /// </summary>
        public double PointDiameter { get; set; } = 0.25;

        /// <summary>
        /// The default brush used to fill closed regions
        /// </summary>
        public Brush FillBrush { get; set; } = null;

        ///// <summary>
        ///// Should the canvas be scaled to fit it's contents?
        ///// </summary>
        //public bool ScaleToFit { get; set; } = true;

        ///// <summary>
        ///// Scale the thickness of curves and the diameter of points to
        ///// a proportion of the overall size of the canvas
        ///// </summary>
        //public bool ProportionalThickness { get; set; } = true;

        

       

        #endregion

        public void RefreshContents()
        {
            Children.Clear();

            VertexGeometryCollection geometry = Geometry;
            //BoundingBox bBox = new BoundingBox(geometry);

            double scaleFactor = 1;
            //if (ProportionalThickness) scaleFactor = bBox.MaxSize;
            
            if (geometry != null)
            {
                //Brush fillBrush = new SolidColorBrush(Color.FromArgb(64, 255, 0, 0));

                foreach (FB.VertexGeometry shape in geometry)
                {
                    if (shape is Curve)
                    {
                        Curve crv = (Curve)shape;
                        PathGeometry pathGeo = new PathGeometry();
                        pathGeo.Figures.Add(FBtoWPF.Convert(crv));

                        Path path = new Path();
                        path.DataContext = this;
                        if (crv.Attributes == null || crv.Attributes.Brush == null)
                        {
                            path.Stroke = DefaultBrush;
                        }
                        else path.Stroke = FBtoWPF.Convert(crv.Attributes.Brush);
                        path.SetBinding(Path.StrokeThicknessProperty, new Binding("CurveThickness"));

                        //path.StrokeThickness = CurveThickness * scaleFactor;

                        path.Data = pathGeo;
                        path.StrokeLineJoin = PenLineJoin.Round;

                        if (crv.Closed && FillBrush != null) path.Fill = FillBrush;
                        //fillBrush = null;

                        Children.Add(path);
                    }
                    else if (shape is PlanarRegion)
                    {
                        PlanarRegion reg = (PlanarRegion)shape;
                        Curve perimeter = reg.Perimeter;
                        CurveCollection voids = reg.Voids;

                        PathGeometry perimeterPath = new PathGeometry();
                        perimeterPath.Figures.Add(FBtoWPF.Convert(perimeter));

                        CombinedGeometry cg = new CombinedGeometry();
                        cg.GeometryCombineMode = GeometryCombineMode.Exclude;
                        cg.Geometry1 = perimeterPath;

                        if (voids != null && voids.Count > 0)
                        {
                            PathGeometry inside = new PathGeometry();
                            foreach (Curve vCrv in voids)
                            {
                                inside.Figures.Add(FBtoWPF.Convert(vCrv));
                            }
                            cg.Geometry2 = inside;
                        }

                        Path path = new Path();
                        if (reg.Attributes == null || reg.Attributes.Brush == null)
                        {
                            path.Fill = DefaultBrush;
                        }
                        else path.Fill = FBtoWPF.Convert(reg.Attributes.Brush, 192);
                        //path.Stroke = Brushes.Black;
                        //path.StrokeThickness = 0.01;
                        path.Data = cg;

                        Children.Add(path);
                    }
                    else if (shape is FB.Label)
                    {
                        FB.Label label = (FB.Label)shape;

                        TextBlock tB = new TextBlock();
                        tB.DataContext = label;
                        tB.Padding = new Thickness(0);
                        tB.Text = label.Text;
                        tB.SetBinding(TextBlock.TextProperty, new Binding("Text"));
                        tB.FontSize = 1;//label.TextSize;
                        tB.RenderTransform = new ScaleTransform(label.TextSize, label.TextSize);
                        //tB.RenderTransformOrigin = new System.Windows.Point(0, 1);

                        if (shape.Attributes != null && shape.Attributes.Brush != null)
                        {
                            tB.Foreground = FBtoWPF.Convert(shape.Attributes.Brush);
                        }
                        else tB.Foreground = DefaultBrush;

                        FormattedText fT = new FormattedText(label.Text, CultureInfo.CurrentCulture, tB.FlowDirection,
                            new Typeface(tB.FontFamily, tB.FontStyle, tB.FontWeight, tB.FontStretch), 1, tB.Foreground);

                        double xOffset;
                        if (label.HorizontalSetOut == HorizontalSetOut.Left) xOffset = 0;
                        else if (label.HorizontalSetOut == HorizontalSetOut.Right) xOffset = fT.Width * label.TextSize;
                        else xOffset = label.TextSize * fT.Width / 2;
                        
                        double yOffset;
                        if (label.VerticalSetOut == VerticalSetOut.Top) yOffset = 0;
                        else if (label.VerticalSetOut == VerticalSetOut.Bottom) yOffset = fT.Height * label.TextSize;
                        else yOffset = label.TextSize * fT.Height / 2;

                        SetLeft(tB, label.Position.X - xOffset);
                        SetTop(tB, -label.Position.Y - yOffset);

                        Children.Add(tB);
                    }
                    else if (shape is Cloud || shape is FB.Point)
                    {
                        Brush pointBrush = DefaultBrush;
                        if (shape.Attributes != null && shape.Attributes.Brush != null)
                        {
                            pointBrush = FBtoWPF.Convert(shape.Attributes.Brush);
                        }
                        foreach (Vertex v in shape.Vertices)
                        {
                            double diameter = PointDiameter * scaleFactor;
                            Ellipse ellipse = new Ellipse();
                            ellipse.Width = diameter;
                            ellipse.Height = diameter;
                            ellipse.Fill = pointBrush;

                            SetLeft(ellipse, v.X - diameter / 2);
                            SetTop(ellipse, -v.Y - diameter / 2);

                            Children.Add(ellipse);
                        }
                    }
                }
            }

            /*if (ScaleToFit)
            {
                //Width = bBox.SizeX;
                //Height = bBox.SizeY;
                //RenderTransform = new TranslateTransform(-bBox.MinX, bBox.MaxY);
            }*/
        }
    }
}

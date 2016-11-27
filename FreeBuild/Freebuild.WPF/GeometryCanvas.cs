﻿using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using FB = FreeBuild.Geometry;

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
            DependencyProperty.Register("Geometry", typeof(ShapeCollection), typeof(GeometryCanvas),
                 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnGeometryChanged)));

        /// <summary>
        /// The profiles to be displayed on this canvas
        /// </summary>
        public ShapeCollection Geometry
        {
            get { return (ShapeCollection)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        /// <summary>
        /// The default stroke thickness of curves drawn on this canvas
        /// </summary>
        public double CurveThickness { get; set; } = 1.0;

        /// <summary>
        /// The default diameter of points drawn on this canvas
        /// </summary>
        public double PointDiameter { get; set; } = 1.0;

        /// <summary>
        /// The default brush used to draw curves on this canvas
        /// </summary>
        public Brush CurveBrush { get; set; } = Brushes.Black;

        /// <summary>
        /// The default brush used to draw points on this canvas
        /// </summary>
        public Brush PointBrush { get; set; } = Brushes.Black;

        #endregion

        public void RefreshContents()
        {
            Children.Clear();

            ShapeCollection geometry = Geometry;
            if (geometry != null)
            {
                foreach (FB.Shape shape in geometry)
                {
                    if (shape is Curve)
                    {
                        Curve crv = (Curve)shape;
                        PathGeometry pathGeo = new PathGeometry();
                        pathGeo.Figures.Add(FBtoWPF.Convert(crv));

                        Path path = new Path();
                        //path.Fill = Brushes.LightGray;
                        path.Stroke = CurveBrush;
                        path.StrokeThickness = CurveThickness;
                        path.Data = pathGeo;
                        path.StrokeLineJoin = PenLineJoin.Round;

                        Children.Add(path);
                    }
                    else if (shape is Cloud)
                    {
                        foreach (Vertex v in shape.Vertices)
                        {
                            double diameter = PointDiameter;
                            Ellipse ellipse = new Ellipse();
                            ellipse.Width = diameter;
                            ellipse.Height = diameter;
                            ellipse.Fill = PointBrush;
                            Canvas.SetLeft(ellipse, v.X - diameter / 2);
                            Canvas.SetTop(ellipse, -v.Y - diameter / 2);

                            Children.Add(ellipse);
                        }
                    }
                }
            }
        }
    }
}
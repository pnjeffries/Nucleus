﻿using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using W = System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// A canvas which is used to draw a preview of a section property
    /// </summary>
    public class ProfileCanvas : Canvas
    {
        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback ProfilesChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseProfilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = ProfilesChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ProfilesChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnProfilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProfileCanvas)d).RaiseProfilesChanged(d, e);
            ((ProfileCanvas)d).RefreshContents();
        }

        /// <summary>
        /// Profiles dependency property
        /// </summary>
        public static DependencyProperty ProfilesProperty =
            DependencyProperty.Register("Profiles", typeof(SectionProfileCollection), typeof(ProfileCanvas),
                 new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnProfilesChanged)));

        /// <summary>
        /// The profiles to be displayed on this canvas
        /// </summary>
        public SectionProfileCollection Profiles
        {
            get { return (SectionProfileCollection)GetValue(ProfilesProperty); }
            set { SetValue(ProfilesProperty, value); }
        }

        /// <summary>
        /// Recreate the displayed geometry inside this canvas
        /// </summary>
        public void RefreshContents()
        {
            BoundingBox bBox = new BoundingBox(0,0,0);
            Children.Clear();

            var steelGrad = new GradientStopCollection();
            steelGrad.Add(new GradientStop(Colors.LightGray, 0));
            steelGrad.Add(new GradientStop(Colors.White, 0.5));
            steelGrad.Add(new GradientStop(Colors.LightGray, 1));

            var steelBrush = new LinearGradientBrush(steelGrad, 45);

            var topLayer = new List<UIElement>();
            if (Profiles != null)
            {
                foreach (SectionProfile sp in Profiles)
                {
                    Curve perimeter = sp.Perimeter;
                    if (perimeter != null)
                    {
                        CurveCollection voids = sp.Voids;
                        bBox.Include(perimeter.BoundingBox);

                        PathGeometry perimeterPath = new PathGeometry();
                        perimeterPath.Figures.Add(ToWPF.Convert(perimeter));

                        CombinedGeometry cg = new CombinedGeometry();
                        cg.GeometryCombineMode = GeometryCombineMode.Exclude;
                        cg.Geometry1 = perimeterPath;

                        if (voids != null && voids.Count > 0)
                        {
                            PathGeometry inside = new PathGeometry();
                            foreach (Curve vCrv in voids)
                            {
                                inside.Figures.Add(ToWPF.Convert(vCrv));
                            }
                            cg.Geometry2 = inside;
                        }

                        Path path = new Path();
                        path.Fill = steelBrush;
                        path.Stroke = Brushes.Black;
                        path.StrokeThickness = 0.01;
                        path.Data = cg;

                        topLayer.Add(path);
                    }
                }

                bBox.Expand(Math.Max(bBox.SizeX, bBox.SizeY) * 0.1);
                double maxSize = Math.Max(bBox.SizeX, bBox.SizeY);

                //Draw grid:
                /*var axisBrush = new RadialGradientBrush(Colors.Black, Colors.Transparent);

                var hAxis = new W.Line();
                hAxis.X1 = -maxSize;
                hAxis.X2 = maxSize;
                hAxis.Stroke = axisBrush;
                hAxis.StrokeThickness = maxSize*0.005;
                Children.Add(hAxis);*/

                //Adjust stroke:
                foreach (UIElement el in topLayer)
                {
                    if (el is Path)
                    {
                        ((Path)el).StrokeThickness = maxSize * 0.005;
                    }
                }

                foreach (UIElement el in topLayer) Children.Add(el);

                //Draw set-out point
                Ellipse setOutPt = new Ellipse();
                setOutPt.Width = maxSize * 0.03;
                setOutPt.Height = setOutPt.Width;
                SetLeft(setOutPt, -setOutPt.Width / 2);
                SetTop(setOutPt, -setOutPt.Height / 2);
                setOutPt.Fill = Brushes.OrangeRed;
                Children.Add(setOutPt);
            }
            Width = bBox.SizeX;
            Height = bBox.SizeY;
            RenderTransform = new TranslateTransform(bBox.SizeX / 2 - bBox.Mid.X, bBox.SizeY / 2 + bBox.Mid.Y);
        }
    }
}

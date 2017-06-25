using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W = System.Windows;
using Shapes = System.Windows.Shapes;
using Media = System.Windows.Media;
using Nucleus.Rendering;
using System.Windows.Controls;
using System.Windows.Data;
using Nucleus.Base;

namespace Nucleus.WPF
{
    /// <summary>
    /// Helper class of static functions to convert Nucleus objects to WPF shapes
    /// </summary>
    public static class FBtoWPF
    {
        /// <summary>
        /// Convert a Nucleus vector to a WPF point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static W.Point Convert(Vector pt)
        {
            return new W.Point(pt.X, -pt.Y);
        }

        /// <summary>
        /// Convert a Nucleus colour to a WPF color
        /// </summary>
        /// <param name="col">The colour to be converterd</param>
        /// <param name="alphaLimit">Optional.  The maximum value to which to limit the alpha channel of the colour.
        /// Use this to specify that the colour should be displayed as semitransparent even if the original to be converted
        /// was not.
        /// </param>
        /// <returns></returns>
        public static Media.Color Convert(Colour col, byte alphaLimit = 255)
        {
            return Media.Color.FromArgb((byte)Math.Min(col.A, alphaLimit), col.R, col.G, col.B);
        }

        /// <summary>
        /// Convert a Nucleus ColourBrush to a WPF SolidColorBrush
        /// </summary>
        /// <param name="brush">The brush to convert</param>
        /// <param name="alphaLimit">Optional.  The maximum value to which to limit the alpha channel of the colour.
        /// Use this to specify that the colour should be displayed as semitransparent even if the original to be converted
        /// was not.
        /// <returns></returns>
        public static Media.SolidColorBrush Convert(ColourBrush brush, byte alphaLimit = 255)
        {
            return new Media.SolidColorBrush(Convert(brush.Colour, alphaLimit));
        }

        /// <summary>
        /// Convert a Nucleus brush to a WPF one
        /// </summary>
        /// <param name="brush">The brush to convert</param>
        /// <param name="alphaLimit">Optional.  The maximum value to which to limit the alpha channel of the colour.
        /// Use this to specify that the colour should be displayed as semitransparent even if the original to be converted
        /// was not.
        /// <returns></returns>
        public static Media.Brush Convert(DisplayBrush brush, byte alphaLimit = 255)
        {
            if (brush is ColourBrush) return Convert((ColourBrush)brush, alphaLimit);
            else return null;
        }

        /// <summary>
        /// Convert a Nucleus binding to a WPF one
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static Binding Convert(PathBinding binding)
        {
            var result = new Binding(binding.Path);
            result.Source = binding.Source;
            return result;
        }

        /// <summary>
        /// Convert a Nucleus line to a WPF one
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Shapes.Line ConvertToLine(Line line)
        {
            if(line == null) return null;
            Shapes.Line result = new Shapes.Line();
            result.X1 = line.StartPoint.X;
            result.Y1 = -line.StartPoint.Y;
            result.X2 = line.EndPoint.X;
            result.Y2 = -line.EndPoint.Y;
            return result;
        }

        /// <summary>
        /// Convert a Nucleus line to a WPF PathFigure
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Media.PathFigure Convert(Line line)
        {
            Media.PathFigure result = new Media.PathFigure();
            result.StartPoint = Convert(line.StartPoint);
            result.Segments.Add(new Media.LineSegment(Convert(line.EndPoint), true));
            return result;
        }

        /// <summary>
        /// Convert a Nucleus arc into a WPF PathFigure
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static Media.PathFigure Convert(Arc arc)
        {
            Media.PathFigure result = new Media.PathFigure();
            result.StartPoint = Convert(arc.StartPoint);
            double radius = arc.Circle.Radius;
            if (arc.Closed)
            {
                //Circle!
                result.Segments.Add(new Media.ArcSegment(Convert(arc.PointAt(0.5)), new W.Size(radius, radius), 0, true,
                Media.SweepDirection.Clockwise, true));
                result.Segments.Add(new Media.ArcSegment(Convert(arc.EndPoint), new W.Size(radius, radius), 0, false,
                   Media.SweepDirection.Clockwise, true));
            }
            else
            {
                bool largeArc = arc.RadianMeasure.IsReflex;
                Media.SweepDirection dir = Media.SweepDirection.Clockwise;
                if (!arc.IsClockwise) dir = Media.SweepDirection.Counterclockwise;
                result.Segments.Add(new Media.ArcSegment(Convert(arc.EndPoint), new W.Size(radius, radius), 0, largeArc, dir, true));
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus PolyLine into a WPF PathFigure
        /// </summary>
        /// <param name="polyLine"></param>
        /// <returns></returns>
        public static Media.PathFigure Convert(PolyLine polyLine)
        {
            Media.PathFigure result = new Media.PathFigure();
            result.StartPoint = Convert(polyLine.StartPoint);
            for (int i = 1; i < polyLine.Vertices.Count; i++)
            {
                Vertex v = polyLine.Vertices[i];
                result.Segments.Add(new Media.LineSegment(Convert(v.Position), true));
            }
            if (polyLine.Closed && polyLine.Vertices.Count > 0)
                result.Segments.Add(new Media.LineSegment(Convert(polyLine.Vertices[0].Position), true));
            return result;
        }

        /// <summary>
        /// Convert a Nucleus PolyCurve into a WPF PathFigure
        /// </summary>
        /// <param name="polyCurve"></param>
        /// <returns></returns>
        public static Media.PathFigure Convert(PolyCurve polyCurve)
        {
            Media.PathFigure result = new Media.PathFigure();
            if (polyCurve.SubCurves.Count > 0)
            {
                result.StartPoint = Convert(polyCurve.StartPoint);
                foreach (Curve crv in polyCurve.SubCurves)
                {
                    if (crv is Line)
                        result.Segments.Add(new Media.LineSegment(Convert(crv.EndPoint), true));
                    else if (crv is Arc)
                    {
                        Arc arc = ((Arc)crv);
                        double radius = arc.Circle.Radius;
                        if (arc.Closed)
                        {
                            //Circle!
                            result.Segments.Add(new Media.ArcSegment(Convert(arc.PointAt(0.5)), new W.Size(radius, radius), 0, true,
                            Media.SweepDirection.Clockwise, true));
                            result.Segments.Add(new Media.ArcSegment(Convert(arc.EndPoint), new W.Size(radius, radius), 0, false,
                               Media.SweepDirection.Clockwise, true));
                        }
                        else
                        {
                            bool largeArc = arc.RadianMeasure.IsReflex;
                            Media.SweepDirection dir = Media.SweepDirection.Clockwise;
                            if (!arc.IsClockwise) dir = Media.SweepDirection.Counterclockwise;
                            result.Segments.Add(new Media.ArcSegment(Convert(arc.EndPoint), new W.Size(radius, radius), 0, largeArc, dir, true));
                        }
                    }
                    else if (crv is PolyLine)
                    {
                        for (int i = 1; i < crv.Vertices.Count; i++)
                        {
                            Vertex v = crv.Vertices[i];
                            result.Segments.Add(new Media.LineSegment(Convert(v.Position), true));
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus curve to a WPF PathFigure
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static Media.PathFigure Convert(Curve curve)
        {
            if (curve is Line) return Convert((Line)curve);
            else if (curve is PolyLine) return Convert((PolyLine)curve);
            else if (curve is Arc) return Convert((Arc)curve);
            else if (curve is PolyCurve) return Convert((PolyCurve)curve);
            else return null;
        }


    }
}

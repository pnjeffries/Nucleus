using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W = System.Windows;
using Shapes = System.Windows.Shapes;
using Media = System.Windows.Media;

namespace FreeBuild.WPF
{
    /// <summary>
    /// Helper class to convert FreeBuild objects to WPF shapes
    /// </summary>
    public static class FBtoWPF
    {
        /// <summary>
        /// Convert a FreeBuild vector to a WPF point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static W.Point Convert(Vector pt)
        {
            return new W.Point(pt.X, -pt.Y);
        }

        /// <summary>
        /// Convert a FreeBuild line to a WPF one
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
        /// Convert a FreeBuild line to a WPF PathFigure
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
        /// Convert a FreeBuild arc into a WPF PathFigure
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
                if (arc.IsClockwise) dir = Media.SweepDirection.Counterclockwise;
                result.Segments.Add(new Media.ArcSegment(Convert(arc.EndPoint), new W.Size(radius, radius), 0, largeArc, dir, true));
            }
            return result;
        }

        /// <summary>
        /// Convert a FreeBuild PolyLine into a WPF PathFigure
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
        /// Convert a FreeBuild PolyCurve into a WPF PathFigure
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
                            if (arc.IsClockwise) dir = Media.SweepDirection.Counterclockwise;
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
        /// Convert a FreeBuild curve to a WPF PathFigure
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

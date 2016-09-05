// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using FreeBuild.Maths;
using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Base class for curves.
    /// Curves are 1-Dimensional geometries defined by a set of ordered vertices. 
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <remarks>This class implements a default set of interrogation methods, which will treat
    /// this curve as a polyline of straight segments between vertices.</remarks>
    [Serializable]
    public abstract class Curve: Shape
    {
        #region Properties

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Default (for most curve types) is false.
        /// </summary>
        public abstract bool Closed { get; protected set; }

        /// <summary>
        /// Get the number of segments that this curve posesses.
        /// Segments are stretches of the curve that can be evaluated independantly 
        /// of the rest of the curve.
        /// </summary
        public virtual int SegmentCount
        {
            get
            {
                if (Vertices.Count > 0)
                {
                    if (Closed) return Vertices.Count + 1;
                    return Vertices.Count;
                }
                else return 0;
            }
        }

        /// <summary>
        /// Get the vertex at the start of the curve (if there is one)
        /// </summary>
        public virtual Vertex Start
        {
            get
            {
                if (Vertices.Count > 0)
                    return Vertices.First();
                else return null;
            }
        }

        /// <summary>
        /// Get the vertex at the end of the curve (if there is one)
        /// </summary>
        public virtual Vertex End
        {
            get
            {
                if (Closed) return Start;
                else if (Vertices.Count > 0) return Vertices.Last();
                else return null;
            }
        }

        /// <summary>
        /// Get the start point of this curve
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public Vector StartPoint
        {
            get
            {
                Vertex start = Start;
                if (start != null) return start.Position;
                else return Vector.Unset;
            }
        }

        /// <summary>
        /// Get the end point of this curve
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public Vector EndPoint
        {
            get
            {
                Vertex end = End;
                if (end != null) return end.Position;
                else return Vector.Unset;
            }
        }

        /// <summary>
        /// Get the length of this curve
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double Length
        {
            get
            {
                return CalculateLength();
            }
        }

        #endregion

        /// <summary>
        /// Evaluate a point on this curve defined by a parameter t
        /// </summary>
        /// <param name="t">A normalised parameter defining a point along this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = curve start, 1 = curve end.
        /// For open curves, parameters outside the range 0-1 will be invalid.
        /// For closed curves, parameters outside this range will 'wrap'.</param>
        /// <returns>The vector coordinates describing a point on the curve at the specified parameter,
        /// if the curve definition and parameter are valid.  Else, an unset vector.</returns>
        /// <remarks>The base implementation treats the curve as being defined as a polyline, with straight lines
        /// between vertices.</remarks>
        public virtual Vector PointAt(double t)
        {
            if (Closed) { t = t % 1.0; }

            //Calculate span
            double tSpan = t * SegmentCount;
            int span = (int)Math.Floor(tSpan);
            tSpan = tSpan % 1.0; //Position in span
            return PointAt(span, tSpan);
        }

        /// <summary>
        /// Evaluate a point defined by a parameter within a specified span.
        /// </summary>
        /// <param name="span">The index of the span.  Valid range 0 to SegmentCount - 1</param>
        /// <param name="tSpan">A normalised parameter defining a point along this span of this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.
        /// </param>
        /// <returns>The vector coordinates describing a point on the curve span at the specified parameter,
        /// if the curve definition and parameter are valid.  Else, null.</returns>
        /// <remarks>The base implementation treats the curve as being defined as a polyline, with straight lines
        /// between vertices.</remarks>
        public virtual Vector PointAt(int span, double tSpan)
        {
            if (!IsValid) return Vector.Unset; //No spans!
            Vertex start = SegmentStart(span); //Find the span start vertex
            if (start == null) return Vector.Unset; //If the start vertex doesn't exist, abort
            if (tSpan == 0) return start.Position;  //If tSpan is at the start of the span, just return the start position
            else
            {
                Vertex end = SegmentEnd(span); //Find the span end vertex
                return start.Position.Interpolate(end.Position, tSpan); //Interpolate position
            }
        }

        /// <summary>
        /// Get the vertex (if any) which defines the start of the specified segment.
        /// </summary>
        /// <param name="index">The segment index.  Valid range 0 to SegmentCount - 1</param>
        /// <returns>The start vertex of the given segment, if it exists.  Else null.</returns>
        public virtual Vertex SegmentStart(int index)
        {
            if (index < Vertices.Count) return Vertices[index];
            else return null;
        }

        /// <summary>
        /// Get the vertex (if any) which defines the end of the specified segment.
        /// </summary>
        /// <param name="index">The segment index.  Valid range 0 to SegmentCount - 1</param>
        /// <returns>The end vertex of the given segment, if it exists.  Else null.</returns>
        public virtual Vertex SegmentEnd(int index)
        {
            if (Closed && index == Vertices.Count) return Start;
            else if (index < Vertices.Count) return Vertices[index + 1];
            else return null;
        }

        /// <summary>
        /// Calculate the length of the segment specified by the index
        /// </summary>
        /// <param name="index">The segment index.  Valid range 0 to SegmentCount - 1</param>
        /// <returns>The length of the segment, or Double.NaN if the segment isnot valid.</returns>
        public virtual double CalculateSegmentLength(int index)
        {
            Vertex end1 = SegmentStart(index);
            Vertex end2 = SegmentEnd(index);
            if (end1 != null && end2 != null)
            {
                return end1.Position.DistanceTo(end2.Position);
            }
            return double.NaN;
        }

        /// <summary>
        /// Calculate the length of this curve
        /// </summary>
        /// <returns>The calculated length of </returns>
        public virtual double CalculateLength()
        {
            double result = 0;
            for (int i = 0; i < SegmentCount; i++)
            {
                result += CalculateSegmentLength(i);
            }
            return result;
        }

        /// <summary>
        /// Calculate the area enclosed by this curve, were the start and end points to be 
        /// joined by a straight line segment.
        /// A plane may optionally be specified, otherwise by default the projected area on 
        /// the XY plane will be used.
        /// </summary>
        /// <param name="onPlane">The plane to use to calculate the area.
        /// If not specified, the XY plane will be used.</param>
        /// <returns>The signed area enclosed by this curve on the specified plane,
        /// as a double.</returns>
        public virtual double CalculateEnclosedArea(out Vector centroid, Plane onPlane = null)
        {
            double result = 0;
            centroid = new Vector();
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector start = Vertices[i].Position;
                Vector end = i == Vertices.Count - 1 ? Vertices[0].Position : Vertices[i + 1].Position;
                if (onPlane != null)
                {
                    start = onPlane.GlobalToLocal(start);
                    end = onPlane.GlobalToLocal(end);
                }
                result += XYAreaUnder(start.X, start.Y, end.X, end.Y, ref centroid);
            }
            centroid /= result;
            return result;
        }

        /// <summary>
        /// Calculate the signed area 'under' the line segment between the two specified points
        /// on the XY plane - i.e. the area between the line and the X-axis.
        /// </summary>
        /// <param name="x0">The x-coordinate of the start of the line</param>
        /// <param name="y0">The y-coordinate of the start of the line</param>
        /// <param name="x1">The x-coordinate of the end of the line</param>
        /// <param name="y1">The y-coordinate of the end of the line</param>
        /// <param name="centroid">The cumulative centroid.  The centroid of the block under
        /// the line will be added to the value passed in here</param>
        /// <returns>The signed area as a double.</returns>
        protected static double XYAreaUnder(double x0, double y0, double x1, double y1, ref Vector centroid)
        {
            //Area is calculated as a rectangle and a triangle combined:
            double areaRectangle = y0 * (x1 - x0);
            double areaTri = (y1 - y0) * (x1 - x0) * 0.5;
            centroid = centroid.Add(
                (x1 + x0) * 0.5 * areaRectangle + (x0 + (x1 - x0) * 2 / 3) * areaTri,
                y0 * 0.5 * areaRectangle + (y0 + (y1 - y0) / 3) * areaTri);
            return areaRectangle + areaTri;
        }

        /// <summary>
        /// Calculate the second moment of area enclosed by this curve on a the XY about the
        /// X-axis, were the start and end points joined by a straight line segment.
        /// A coordinate system may be specified, otherwise by default the global XY plane and
        /// X-axis will be used.
        /// </summary>
        /// <param name="onPlane">The coordinate system in which the second moment of area is
        /// to be calculated.  The second moment of area will be calculated on the XY plane and
        /// about the X-axis of this system.</param>
        /// <returns>The signed second moment of area enclosed by this curve, as a
        /// double.</returns>
        public virtual double CalculateEnclosedIxx(ICoordinateSystem onPlane = null)
        {
            double result = 0;
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector start = Vertices[i].Position;
                Vector end = i == Vertices.Count - 1 ? Vertices[0].Position : Vertices[i + 1].Position;
                if (onPlane != null)
                {
                    start = onPlane.GlobalToLocal(start);
                    end = onPlane.GlobalToLocal(end);
                }
                result += IxxUnder(start.X, start.Y, end.X, end.Y);
            }
            return result;
        }

        /// <summary>
        /// Calculate the signed second moment of area 'under' the line segment between the
        /// two specified coordinates on the XY plane about the X-axis
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns>The signed Ixx as a double</returns>
        protected static double IxxUnder(double x0, double y0, double x1, double y1)
        {
            double aR = y0 * (x1 - x0); //Area of rectangle
            double IxxR = ((x1 - x0) * y0*y0*y0 ) / 12; //(bh^3)/12
            double yCR = y0 / 2; //y of centroid of rectangle
            double aT = (y1 - y0) * (x1 - x0) * 0.5; //Area of triangle
            double IxxT = ((x1 - x0) * Math.Pow((y1 - y0),3)) / 36; //(bh^3)/36
            double yCT = (y0 + (y1 - y0) / 3); //y of centroid triangle
            return IxxR + aR * yCR * yCR + IxxT + aT * yCT * yCT;
        }
    }
}

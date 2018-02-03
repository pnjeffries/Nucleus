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

using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Maths;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A curve consisting of straight lines between vertices
    /// </summary>
    [Serializable]
    public class PolyLine : Curve
    {
        #region Properties

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Default is false.
        /// </summary>
        public override bool Closed { get; protected set; }

        /// <summary>
        /// Is this polyline valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (Vertices.Count > 1) return true;
                else return false;
            }
        }

        /// <summary>
        /// Private backing field for Vertices collection
        /// </summary>
        private VertexCollection _Vertices;

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this polyline.
        /// The polyline will be defined as straight lines in between the vertices
        /// in this collection.
        /// </summary>
        public override VertexCollection Vertices
        {
            get { return _Vertices; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PolyLine()
        {
            _Vertices = new VertexCollection(this);
        }

        /// <summary>
        /// Points constructor.
        /// Creates a polyline between the specified set of points
        /// </summary>
        /// <param name="points"></param>
        /// <param name="close"></param>
        public PolyLine(IEnumerable<Vector> points, bool close, GeometryAttributes attributes = null) : this()
        {
            foreach(Vector pt in points)
            {
                Vertices.Add(new Vertex(pt));
            }
            Closed = close;
            Attributes = attributes;
        }

        /// <summary>
        /// Points constructor.
        /// Creates a polyline between the specified set of points
        /// </summary>
        /// <param name="points"></param>
        /// <param name="close"></param>
        public PolyLine(IEnumerable<Vector> points, GeometryAttributes attributes = null) : this()
        {
            foreach (Vector pt in points)
            {
                Vertices.Add(new Vertex(pt));
            }
            if (Vertices.Count > 1 && Vertices.First().DistanceToSquared(Vertices.Last()) <= Tolerance.Distance * Tolerance.Distance)
            {
                Vertices.RemoveAt(Vertices.Count - 1);
                Closed = true;
            }
            Attributes = attributes;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Close this polyline, so that a line segment joins the last vertex and the first one.
        /// </summary>
        /// <param name="close">If true, polyline will be made closed.  If false, will be made unclosed.</param>
        public void Close(bool close = true)
        {
            Closed = close;
        }

        /// <summary>
        /// Add a new point to the end of the polyline 
        /// (creating a new polyline segment if this is not the first point added)
        /// </summary>
        /// <param name="pt"></param>
        public Vertex Add(Vector pt)
        {
            Vertex result = new Vertex(pt);
            Vertices.Add(result);
            return result;
        }

        public override Curve Offset(IList<double> distances)
        {
            Vector[] pts = new Vector[Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector p0 = Vector.Unset; // Last point
                Vector p1 = Vertices[i].Position; // This point
                Vector p2 = Vector.Unset; // Next point

                if (i > 0) p0 = Vertices[i - 1].Position;
                else if (Closed) p0 = Vertices.Last().Position;

                if (i < Vertices.Count - 1) p2 = Vertices[i + 1].Position;
                else if (Closed) p2 = Vertices.First().Position;

                Vector v1 = Vector.Unset;
                Vector v2 = Vector.Unset;

                if (p0.IsValid()) v1 = (p1 - p0).Unitize();
                if (p2.IsValid()) v2 = (p2 - p1).Unitize();

                double d1 = distances.GetWrapped(i - 1);
                double d2 = distances.GetBounded(i);

                if (v1.IsValid() && !v1.IsZero() && v2.IsValid() && !v2.IsZero() && !v1.IsParallelTo(v2))
                {
                    // Calculate intersection of offset lines:
                    Vector o1 = v1.PerpendicularXY() * d1;
                    Vector o2 = v2.PerpendicularXY() * d2;
                    Vector pOff = Intersect.LineLineXY(p0 + o1, v1, p2 + o2, v2);
                    pts[i] = pOff;
                }
                else
                {
                    // Can't intersect, so translate instead
                    // Figure out which span to use:
                    Vector sV;
                    double d;
                    if (v1.IsValidNonZero())
                    {
                        sV = v1;
                        d = d1;
                    }
                    else
                    {
                        sV = v2;
                        d = d2;
                    }

                    if (sV.IsValid() && !sV.IsZero())
                    {
                        Vector pOff = p1 + sV.PerpendicularXY() * d;
                        pts[i] = pOff;
                    }
                    else pts[i] = p1; // Can't offset!
                }
            }
            return new PolyLine(pts, Closed);
        }

        /// <summary>
        /// Convert this polycurve into an equivalent list
        /// of Line objects
        /// </summary>
        /// <returns></returns>
        public IList<Line> ToLines()
        {
            var result = new List<Line>(SegmentCount);
            for (int i = 0; i < SegmentCount; i++)
            {
                result.Add(new Geometry.Line(SegmentStart(i).Position, SegmentEnd(i).Position, Attributes));
            }
            return result;
        }

        /// <summary>
        /// Convert this polyline into a polycurve containing
        /// line objects representing the same geometry
        /// </summary>
        /// <returns></returns>
        public PolyCurve ToPolyCurve()
        {
            return new PolyCurve(ToLines(), Attributes);
        }

        /// <summary>
        /// 'Explode' this curve into a collection of its constituent
        /// segments as individual curves.
        /// </summary>
        /// <param name="recursive">If true (default), any sub-curves of
        /// this curve which themselves have sub-curves will also be exploded
        /// and added to the collection.</param>
        /// <returns></returns>
        public override CurveCollection Explode(bool recursive = true)
        {
            var result = new CurveCollection();
            result.AddRange(ToLines());
            return result;
        }

        
        /// <summary>
        /// Extract a portion of this curve as a new curve
        /// </summary>
        /// <param name="subDomain">The subdomain of this curve to
        /// be extracted as a new curve</param>
        /// <returns></returns>
        public override Curve Extract(Interval subDomain)
        {
            var pts = new List<Vector>();
            pts.Add(PointAt(subDomain.Start));
            double offset = 1;
            if (Closed) offset = 0;

            if (!subDomain.IsDecreasing)
            {
                for (int i = 0; i < VertexCount; i++)
                {
                    double t = ((double)i) / (VertexCount - offset);
                    if (t > subDomain.Start && t < subDomain.End)
                        pts.Add(Vertices[i].Position);
                }
            }
            else
            {
                //Loops!
                for (int i = 0; i < VertexCount; i++)
                {
                    if (((double)i) / (VertexCount - offset) > subDomain.Start)
                        pts.Add(Vertices[i].Position);
                }
                if (Closed) // Should this treat the polyline as closed even if it isn't?
                {
                    for (int i = 0; i < VertexCount; i++)
                    {
                        if (((double)i) / (VertexCount - offset) < subDomain.End)
                            pts.Add(Vertices[i].Position);
                    }
                }
            }
            pts.Add(PointAt(subDomain.End));
            return new PolyLine(pts, Attributes);
        }
        

        public override string ToString()
        {
            return "Polyline";
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Static factory method to create a polyline representing a trapezoid centred on the
        /// origin of the XY plane.  If depth or both widths are 0 or lower, null will be returned instead.
        /// </summary>
        /// <param name="depth">The depth of the trapezoid</param>
        /// <param name="topWidth">The width of the top of the trapezoid</param>
        /// <param name="baseWidth">The width of the base of the trapezoid</param>
        /// <returns></returns>
        public static PolyLine Trapezoid(double depth, double topWidth, double baseWidth)
        {
            if (depth > 0 && (topWidth > 0 || baseWidth > 0))
            {
                topWidth = Math.Max(0, topWidth);
                baseWidth = Math.Max(0, baseWidth);
                return new PolyLine(new Vector[]
                {
                    new Vector(topWidth/2, depth/2),
                    new Vector(-topWidth/2, depth/2),
                    new Vector(-baseWidth/2, -depth/2),
                    new Vector(baseWidth/2, -depth/2)
                }, true);
            }
            else return null;
        }

        /// <summary>
        /// Static factory method to create a polyline representing a rectangle centred on the
        /// origin on the XY plane.  If the depth or width are 0 or lower null will be returned instead.
        /// </summary>
        /// <param name="depth">The depth of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <returns>A polyline representing the specified rectangle with vertices arranged in an
        /// anti-clockwise manner, or null is the input depth and width are invalid.</returns>
        public static PolyLine Rectangle(double depth, double width)
        {
            if (depth > 0 && width > 0)
            {
                return new PolyLine(new Vector[]
                {
                new Vector(width/2, depth/2),
                new Vector(-width/2, depth/2),
                new Vector(-width/2, -depth/2),
                new Vector(width/2, -depth/2)
                }, true);
            }
            else return null;
        }

        /// <summary>
        /// Static factory method to create a polyline representing a rectangle with the specified
        /// corner point coordinates.
        /// </summary>
        /// <returns>A polyline representing the specified rectangle with vertices arranged in an
        /// anti-clockwise manner, or null is the input depth and width are invalid.</returns>
        public static PolyLine Rectangle(double x0, double y0, double x1, double y1)
        {
            return new PolyLine(new Vector[]
            {
                new Vector(x0, y0),
                new Vector(x1, y0),
                new Vector(x1, y1),
                new Vector(x0, y1),
            }, true);
        }

        /// <summary>
        /// Static factory method to create a polyline representing a rectangle on the XY plane between the two specified
        /// corner points.
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static PolyLine Rectangle(Vector v0, Vector v1)
        {
            return Rectangle(Math.Min(v0.X, v1.X), Math.Min(v0.Y, v1.Y), Math.Max(v0.X, v1.X), Math.Max(v0.Y, v1.Y));
        }

        #endregion
    }
}

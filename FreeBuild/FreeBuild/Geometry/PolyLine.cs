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

        public override string ToString()
        {
            return "Polyline";
        }

        #endregion

        #region Static Methods

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

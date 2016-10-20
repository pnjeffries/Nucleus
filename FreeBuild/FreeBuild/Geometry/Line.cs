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

using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A straight line between two points.
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    public class Line : Curve
    {

        #region Properties

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Lines cannot be closed, so this will always return false.
        /// </summary>
        public override bool Closed { get { return false; } protected set { } }

        public override bool IsValid
        {
            get
            {
                return Vertices.Count == 2;
            }
        }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The line will be defined as a straight line in between the first and last vertices
        /// in this collection.
        /// </summary>
        public override VertexCollection Vertices { get; }

        /// <summary>
        /// Get the number of segments that this curve posesses.
        /// Segments are stretches of the curve that can be evaluated independantly 
        /// of the rest of the curve.
        /// Valid lines have one segment.
        /// </summary>
        public override int SegmentCount
        {
            get
            {
                if (IsValid) return 1;
                else return 0;
            }
        }

        /// <summary>
        /// Get the mid-point of the line
        /// </summary>
        public Vector MidPoint { get { return StartPoint.Interpolate(EndPoint, 0.5); } }

        #endregion

        #region Construtors

        /// <summary>
        /// Default constructor.  Initialises an empty line with
        /// no geometry.  The line will not be valid until its vertices
        /// are populated.
        /// </summary>
        public Line()
        {
            Vertices = new VertexCollection(this);
        }

        /// <summary>
        /// Constructor to create a new line between two points
        /// </summary>
        /// <param name="startPoint">The start point of the line</param>
        /// <param name="endPoint">The end point of the line</param>
        public Line(Vector startPoint, Vector endPoint): this()
        {
            Vertices.Add(new Vertex(startPoint));
            Vertices.Add(new Vertex(endPoint));
        }

        /// <summary>
        /// Constructor to create a new line between two vertices
        /// </summary>
        /// <param name="startVertex">The start vertex of the line.  This should not be shared with any other geometry.</param>
        /// <param name="endVertex">The end vertex of the line.  This should not be shared with any other geometry.Thi</param>
        public Line(Vertex startVertex, Vertex endVertex): this()
        {
            Vertices.Add(startVertex);
            Vertices.Add(endVertex);
        }

        /// <summary>
        /// Initialises a new line between two nodes.
        /// </summary>
        /// <param name="startNode">The node at the beginning of the line</param>
        /// <param name="endNode">The node at the end of the line</param>
        public Line(Node startNode, Node endNode) : this()
        {
            Vertices.Add(new Vertex(startNode));
            Vertices.Add(new Vertex(endNode));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the vertex (if any) which defines the end of the specified segment.
        /// </summary>
        /// <param name="index">The segment index.  Valid range 0 to SegmentCount</param>
        /// <returns>The end vertex of the given segment, if it exists.  Else null.</returns>
        public override Vertex SegmentEnd(int index)
        {
            if (index >= 0 && index < SegmentCount) return Vertices.Last();
            else return null;
        }

        /// <summary>
        /// Calculate the length of the line
        /// </summary>
        /// <returns></returns>
        public override double CalculateLength()
        {
            return Start.Position.DistanceTo(End.Position);
        }

        /// <summary>
        /// Calculate the area enclosed by this line.
        /// This is an easy calculation because it's zero.
        /// </summary>
        /// <param name="centroid"></param>
        /// <param name="onPlane"></param>
        /// <returns></returns>
        public override double CalculateEnclosedArea(out Vector centroid, Plane onPlane = null)
        {
            centroid = MidPoint;
            return 0;
        }

        /// <summary>
        /// Set this line to run between the specified start and end points.
        /// Will modify the positions of the start and end vertices of this line.
        /// </summary>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        public void Set(Vector startPt, Vector endPt)
        {
            Start.Position = startPt;
            End.Position = endPt;
        }

        #endregion

    }
}

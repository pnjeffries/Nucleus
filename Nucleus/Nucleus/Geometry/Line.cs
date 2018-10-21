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

using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Maths;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A straight line between two points.
    /// </summary>
    [Serializable]
    public class Line : Curve, ISimpleCurve
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
        /// Private backing field for Vertices property
        /// </summary>
        private VertexCollection _Vertices;

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The line will be defined as a straight line in between the first and last vertices
        /// in this collection.
        /// </summary>
        public override VertexCollection Vertices
        {
            get { return _Vertices; }
        }

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
        
        /// <summary>
        /// Get a unit vector in the direction of the line from start to end
        /// </summary>
        public Vector Direction { get { return (EndPoint - StartPoint).Unitize(); } }

        #endregion

        #region Construtors

        /// <summary>
        /// Default constructor.  Initialises an empty line with
        /// no geometry.  The line will not be valid until its vertices
        /// are populated.
        /// </summary>
        public Line()
        {
            _Vertices = new VertexCollection(this);
        }

        /// <summary>
        /// Constructor to create a new line between two points
        /// </summary>
        /// <param name="startPoint">The start point of the line</param>
        /// <param name="endPoint">The end point of the line</param>
        public Line(Vector startPoint, Vector endPoint, GeometryAttributes attributes = null) : this()
        {
            Vertices.Add(new Vertex(startPoint));
            Vertices.Add(new Vertex(endPoint));
            Attributes = attributes;
        }

        /// <summary>
        /// Constructor to create a new line between two sets of coordinates
        /// </summary>
        /// <param name="x0">The x coordinate of the line start</param>
        /// <param name="y0">The y coordinate of the line start</param>
        /// <param name="z0">The z coordinate of the line start</param>
        /// <param name="x1">The x coordinate of the line end</param>
        /// <param name="y1">The y coordinate of the line end</param>
        /// <param name="z1">The z coordinate of the line end</param>
        public Line(double x0, double y0, double z0, double x1, double y1, double z1, GeometryAttributes attributes = null)
            : this(new Vector(x0, y0, z0), new Vector(x1, y1, z1), attributes) { }

        /// <summary>
        /// Constructor to create a new line between two sets of coordinates
        /// on the XY plane.
        /// </summary>
        /// <param name="x0">The x coordinate of the line start</param>
        /// <param name="y0">The y coordinate of the line start</param>
        /// <param name="x1">The x coordinate of the line end</param>
        /// <param name="y1">The y coordinate of the line end</param>
        public Line(double x0, double y0, double x1, double y1, GeometryAttributes attributes = null) : this(x0, y0, 0, x1, y1, 0, attributes) { }

        /// <summary>
        /// Constructor to create a new line between two vertices
        /// </summary>
        /// <param name="startVertex">The start vertex of the line.  This should not be shared with any other geometry.</param>
        /// <param name="endVertex">The end vertex of the line.  This should not be shared with any other geometry.Thi</param>
        public Line(Vertex startVertex, Vertex endVertex) : this()
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

        /// <summary>
        /// Evaluate a point on this curve a specified distance from the start or end.
        /// </summary>
        /// <param name="length">The length along the curve</param>
        /// <param name="fromEnd">If true, the length will be measured from the end
        /// of the curve.  If false (default) it will be measured from the start.</param>
        /// <returns></returns>
        public override Vector PointAtLength(double length, bool fromEnd = false)
        {
            double lineLength = Length;
            if (length <= lineLength)
            {
                if (!fromEnd) return StartPoint.Interpolate(EndPoint, length / lineLength);
                else return EndPoint.Interpolate(StartPoint, length / lineLength);
            }
            else return Vector.Unset;
        }

        /// <summary>
        /// Get the curve parameter at the specified length along this curve.
        /// If the returned parameter falls outside the range 0-1, the specified
        /// length does not fall within the domain of the curve.
        /// </summary>
        /// <param name="length">The distance along the curve from the start of the curve to the point in question</param>
        /// <returns>A curve parameter</returns>
        public override double ParameterAt(double length)
        {
            return length / Length;
        }

        /// <summary>
        /// Generate a set of evenly-spaced points along this line
        /// by dividing it into the specified number of segments.
        /// The resulting number of points will be divisions + 1
        /// </summary>
        /// <param name="divisions"></param>
        /// <returns></returns>
        public override Vector[] Divide(int divisions)
        {
            Vector[] result = new Vector[divisions + 1];
            for (int i = 0; i <= divisions; i++)
            {
                result[i] = StartPoint.Interpolate(EndPoint, (double)i * 1.0 / divisions);
            }
            return result;
        }

        public override string ToString()
        {
            return "Line{" + Start.ToString() + " to " + End.ToString() + "}";
        }

        /// <summary>
        /// Find the closest point on this line to a test point, expressed as
        /// a parameter from 0-1 (Start-End)
        /// </summary>
        /// <param name="toPoint">The test point</param>
        /// <returns></returns>
        public override double ClosestParameter(Vector toPoint)
        {
            return ClosestParameter(StartPoint, EndPoint, toPoint);
        }

        /// <summary>
        /// Find the closest point on this line to a test point
        /// </summary>
        /// <param name="toPoint">The test point</param>
        /// <returns></returns>
        public override Vector ClosestPoint(Vector toPoint)
        {
            return ClosestPoint(StartPoint, EndPoint, toPoint);
        }

        /// <summary>
        /// Offset this curve on the XY plane
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <returns></returns>
        public override Curve Offset(double distance, bool tidy = true, bool copyAttributes = true)
        {
            Vector dir = Direction;
            dir = dir.PerpendicularXY() * distance;
            return new Line(StartPoint + dir, EndPoint + dir, copyAttributes ? Attributes : null);
        }

        /// <summary>
        /// Offset this curve on the XY plane by varying distances for
        /// each span.
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <param name="tidy">If true (default) collapsed segments will be removed.</param>
        /// <returns></returns>
        public override Curve Offset(IList<double> distances, bool tidy = true, bool copyAttributes = true)
        {
            if (distances != null && distances.Count > 0)
                return Offset(distances[0], tidy, copyAttributes);
            else return Offset(0, tidy, copyAttributes);
        }

        /// <summary>
        /// Extend this line to meet (as closely as possible) another.
        /// The start or end vertex of this line (whichever will result in the smallest
        /// overall movement) will be moved to meet the other at the closest point.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool ExtendToLine(Line other)
        {
            Vector pt = Axis.ClosestPoint(StartPoint, Direction, other.StartPoint, other.Direction);
            if (pt.IsValid())
            {
                double t = ClosestParameter(pt);
                if (t < 0.5) Start.Position = pt;
                else End.Position = pt;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Check whether the specified point lies within the area enclosed by this curve
        /// on the XY plane
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool EnclosesXY(Vector point)
        {
            return false;
        }

        /// <summary>
        /// Extract a portion of this curve as a new curve
        /// </summary>
        /// <param name="subDomain">The subdomain of this curve to
        /// be extracted as a new curve</param>
        /// <returns></returns>
        public override Curve Extract(Interval subDomain)
        {
            return new Line(PointAt(subDomain.Start), PointAt(subDomain.End), Attributes);
        }

        /// <summary>
        /// Decompose this curve down to simple primitive curve types such
        /// as line and arc segments.  Lines are already 'simple' and so this
        /// will just return a list containing this line.
        /// </summary>
        /// <returns></returns>
        public override IList<ISimpleCurve> ToSimpleCurves()
        {
            var result = new List<ISimpleCurve>();
            result.Add(this);
            return result;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Find the closest point on a line segment specified by start and end points to
        /// a test point.
        /// </summary>
        /// <param name="startPt">The start point of the line</param>
        /// <param name="endPt">The end point of the line</param>
        /// <param name="testPt">The point from which the distance is to be checked</param>
        /// <returns></returns>
        public static Vector ClosestPoint(Vector startPt, Vector endPt, Vector testPt)
        {
            Vector direction = endPt - startPt;
            double t = Axis.ClosestParameter(startPt, direction, testPt);
            if (t < 0) return startPt;
            else if (t > 1.0) return endPt;
            else return startPt + direction * t;
        }

        /// <summary>
        /// Find the closest point on a line segment specified by start and end points to
        /// a test point.
        /// </summary>
        /// <param name="startPt">The start point of the line</param>
        /// <param name="endPt">The end point of the line</param>
        /// <param name="testPt">The point from which the distance is to be checked</param>
        /// <returns></returns>
        public static double ClosestParameter(Vector startPt, Vector endPt, Vector testPt)
        {
            Vector direction = endPt - startPt;
            double t = Axis.ClosestParameter(startPt, direction, testPt);
            if (t < 0) return 0;
            else if (t > 1.0) return 1;
            else return t;
        }


        #endregion
    }
}

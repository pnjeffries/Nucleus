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
        /// Constructor to create a new line between two points
        /// </summary>
        /// <param name="startPoint">The start point of the line</param>
        /// <param name="endPoint">The end point of the line</param>
        public Line(Vector startPoint, Vector endPoint)
        {
            Vertices = new VertexCollection(this);
            Vertices.Add(new PointVertex(startPoint));
            Vertices.Add(new PointVertex(endPoint));
        }

        /// <summary>
        /// Constructor to create a new line between two vertices
        /// </summary>
        /// <param name="startVertex">The start vertex of the line.  This should not be shared with any other geometry.</param>
        /// <param name="endVertex">The end vertex of the line.  This should not be shared with any other geometry.Thi</param>
        public Line(Vertex startVertex, Vertex endVertex)
        {
            Vertices = new VertexCollection(this);
            Vertices.Add(startVertex);
            Vertices.Add(endVertex);
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

        public override double CalculateEnclosedArea(out Vector centroid, Plane onPlane = null)
        {
            centroid = MidPoint;
            return 0;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A class representing a mesh edge which stores references to
    /// the faces and vertices to which it is connected.
    /// Note that this is rather different in implementation to
    /// the temporary mesh edges
    /// </summary>
    public class LinkedMeshEdge
    {
        #region Properties

        /// <summary>
        /// The vertex at the start of this edge
        /// </summary>
        public Vertex Start { get; }

        /// <summary>
        /// The vertex at the end of this edge
        /// </summary>
        public Vertex End { get; }

        /// <summary>
        /// Get the start point of this edge
        /// </summary>
        public Vector StartPoint { get { return Start.Position; } }

        /// <summary>
        /// Get the end point of this edge
        /// </summary>
        public Vector EndPoint { get { return End.Position; } }

        /// <summary>
        /// Get the length of this edge
        /// </summary>
        public double Length
        {
            get { return StartPoint.DistanceTo(EndPoint); }
        }

        /// <summary>
        /// Get the squared length of this edge
        /// </summary>
        public double LengthSquared
        {
            get { return StartPoint.DistanceToSquared(EndPoint); }
        }

        #endregion
    }
}

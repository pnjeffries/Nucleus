using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Meshing
{
    /// <summary>
    /// A half-edge data structure used as a means of
    /// representing edges in a manifold mesh in an easily-traversable
    /// manner.
    /// </summary>
    [Serializable]
    public class HalfEdge
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Vertex property
        /// </summary>
        private HalfEdgeVertex _Vertex;

        /// <summary>
        /// The vertex at the end of the half-edge
        /// </summary>
        public HalfEdgeVertex Vertex
        {
            get { return _Vertex; }
            set { _Vertex = value; }
        }

        /// <summary>
        /// Private backing member variable for the Pair property
        /// </summary>
        private HalfEdge _Pair;

        /// <summary>
        /// The oppositely-orientated adjacent half-edge
        /// </summary>
        public HalfEdge Pair
        {
            get { return _Pair; }
            set { _Pair = value; }
        }

        /// <summary>
        /// Private backing member variable for the Face property
        /// </summary>
        private HalfEdgeFace _Face;

        /// <summary>
        /// The mesh face that the half-edge borders
        /// </summary>
        public HalfEdgeFace Face
        {
            get { return _Face; }
            set { _Face = value; }
        }

        /// <summary>
        /// Private backing member variable for the Next property
        /// </summary>
        private HalfEdge _Next;

        /// <summary>
        /// The mesh face that the half-edge borders
        /// </summary>
        public HalfEdge Next
        {
            get { return _Next; }
            set { _Next = value; }
        }



        #endregion
    }
}

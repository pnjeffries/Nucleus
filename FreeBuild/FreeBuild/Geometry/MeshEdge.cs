using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Class that represents an edge between two vertices in a mesh.
    /// A temporary construct used during certain mesh operations - does not
    /// form part of the core definition of a mesh geometry
    /// </summary>
    public struct MeshEdge
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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor, initialising an edge between the two specified vertices
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public MeshEdge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            return Start.GetHashCode() * End.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this == (MeshEdge)obj;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Tests whether the two edges are equal.
        /// Direction does not matter for two edges to be equal.
        /// </summary>
        /// <param name="edge1"></param>
        /// <param name="edge2"></param>
        /// <returns></returns>
        public static bool operator == (MeshEdge edge1, MeshEdge edge2)
        {
            //if (ReferenceEquals(edge1, edge2)) return true;
            //else if ((object)edge1 == null || (object)edge2 == null) return false;
            //else 
            return ((edge1.Start == edge2.Start && edge1.End == edge2.End) || (edge1.Start == edge2.End && edge1.End == edge2.Start));
        }

        public static bool operator != (MeshEdge edge1, MeshEdge edge2)
        {
            return !(edge1 == edge2);
        }

        #endregion
    }
}

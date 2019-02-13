using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Meshing
{
    /// <summary>
    /// A temporary data structure to hold information about
    /// a mesh edge during a subdivision operation
    /// </summary>
    [Serializable]
    public class MeshDivisionEdge
    {
        #region Properties

        private VertexCollection _Vertices = new VertexCollection();

        /// <summary>
        /// The vertices along this edge
        /// </summary>
        public VertexCollection Vertices
        {
            get { return _Vertices; }
        }

        private Vertex _Start;

        /// <summary>
        /// Get the start vertex of the edge
        /// </summary>
        public Vertex Start
        {
            get { return _Start; }
        }

        private Vertex _End;

        /// <summary>
        /// Get the end vertex of the edge
        /// </summary>
        public Vertex End
        {
            get { return _End; }
        }

        /// <summary>
        /// The length of the edge
        /// </summary>
        public double Length
        {
            get { return Start.DistanceTo(End); }
        }

        private string _ID;

        /// <summary>
        /// The ID of the edge, used to uniquely
        /// identify and combine them
        /// </summary>
        public string ID
        {
            get { return _ID; }
        }

        /// <summary>
        /// The number of segments this edge has been divided
        /// into.
        /// </summary>
        public int Divisions
        {
            get { return _Vertices.Count - 1; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise a new MeshDivisionEdge between the specified vertices
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public MeshDivisionEdge(Vertex start, Vertex end)
        {
            _Start = start;
            _End = end;
            _ID = IDFor(start, end);
        }

        /// <summary>
        /// Initialise a new MeshDivisionEdge for the specified edge of the specified face
        /// </summary>
        /// <param name="face"></param>
        /// <param name="edgeIndex"></param>
        public MeshDivisionEdge(MeshFace face, int edgeIndex)
            : this(face[edgeIndex], face.GetWrapped(edgeIndex + 1))
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Subdivide this edge by adding new vertices between the start and
        /// end vertices
        /// </summary>
        /// <param name="divisions"></param>
        public void SubDivide(int divisions)
        {
            _Vertices.Clear();
            Vector startPt = Start.Position;
            Vector endPt = End.Position;
            Vector trans = endPt - startPt;
            double step = 1.0 / divisions;
            _Vertices.Add(Start);
            for (int i = 1; i < divisions; i++)
            {
                _Vertices.Add(new Vertex(startPt + trans * (step * i)));
            }
            _Vertices.Add(End);
        }

        /// <summary>
        /// Subdivide this edge into segments with no greater than the specified
        /// maximum length by adding new vertices between the start and end
        /// vertices.
        /// </summary>
        /// <param name="maxLength"></param>
        public void SubDivideByLength(double maxLength)
        {
            double edgeLength = Length;
            int divisions = (int)Math.Ceiling(edgeLength / maxLength);
            SubDivide(divisions);
        }

        /// <summary>
        /// Get a copy of this edge with the direction reversed
        /// </summary>
        /// <returns></returns>
        public MeshDivisionEdge Reversed()
        {
            var result = new MeshDivisionEdge(End, Start);
            for (int i = _Vertices.Count - 1; i >= 0; i--)
            {
                result.Vertices.Add(_Vertices[i]);
            }
            return result;
        }

        /// <summary>
        /// Split this division edge into two pieces, being shorter
        /// MeshDivisionEdges which together contain the division
        /// vertices of this edge.
        /// </summary>
        /// <param name="atIndex">The index of the vertex in this edge's
        /// Vertices collection at which the split will occur.  This
        /// will be the last vertex of the first sub-edge and the first
        /// vertex of the second.</param>
        /// <returns>An array containing two new MeshDivisionEdges, or
        /// null if the split failed.</returns>
        public MeshDivisionEdge[] Split(int atIndex)
        {
            if (atIndex > 0 && atIndex < Vertices.Count - 1)
            {
                Vertex splitVert = Vertices[atIndex];

                // Create sub-edges
                MeshDivisionEdge edge1 = new MeshDivisionEdge(Start, splitVert);
                MeshDivisionEdge edge2 = new MeshDivisionEdge(splitVert, End);

                // Populate sub-edge vertices
                for (int i = 0; i < Vertices.Count; i++)
                {
                    Vertex v = Vertices[i];
                    if (i <= atIndex) edge1.Vertices.Add(v);
                    if (i >= atIndex) edge2.Vertices.Add(v);
                }

                return new MeshDivisionEdge[] { edge1, edge2 };
            }
            else return null;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Get the ID string for an edge between the specified pair of vertices
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string IDFor(Vertex start, Vertex end)
        {
            int sID = start.Number;
            int eID = end.Number;
            if (eID < sID) return eID + "_" + sID;
            else return sID + "_" + eID;
        }

        /// <summary>
        /// Get the ID string for the specified edge of the face
        /// </summary>
        /// <param name="face"></param>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public static string IDFor(MeshFace face, int edgeIndex)
        {
            return IDFor(face[edgeIndex], face.GetWrapped(edgeIndex + 1));
        }

        #endregion
    }
}

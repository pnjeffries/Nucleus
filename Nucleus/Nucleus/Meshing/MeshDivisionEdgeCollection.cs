using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Meshing
{
    /// <summary>
    /// A collection of mesh division edges
    /// </summary>
    [Serializable]
    public class MeshDivisionEdgeCollection : KeyedCollection<string, MeshDivisionEdge>
    {
        #region Constructors

        /// <summary>
        /// Initialise a new empty collection
        /// </summary>
        public MeshDivisionEdgeCollection() { }

        /// <summary>
        /// Initialise a collection of MeshDivisionEdges generated from the specified
        /// face collection.
        /// </summary>
        /// <param name="generateFrom"></param>
        public MeshDivisionEdgeCollection(MeshFaceCollection generateFrom)
        {
            GenerateForFaces(generateFrom);
        }

        #endregion

        #region Methods

        protected override string GetKeyForItem(MeshDivisionEdge item)
        {
            return item.ID;
        }

        /// <summary>
        /// Generate edge division structures for the specified collection
        /// of mesh faces
        /// </summary>
        /// <param name="faces"></param>
        public void GenerateForFaces(MeshFaceCollection faces)
        {
            foreach (MeshFace face in faces)
            {
                for (int i = 0; i < face.Count; i++)
                {
                    string edgeID = MeshDivisionEdge.IDFor(face, i);
                    if (!Contains(edgeID))
                    {
                        Add(new MeshDivisionEdge(face, i));
                    }
                }
            }
        }

        /// <summary>
        /// Subdivide all edges in this collection by adding intermediate
        /// vertices such that no segment between them exceeds the specified
        /// length
        /// </summary>
        /// <param name="maxLength"></param>
        public void SubDivideAll(double maxLength)
        {
            foreach (MeshDivisionEdge edge in this)
                edge.SubDivideByLength(maxLength);
        }

        /// <summary>
        /// Get all of the previously generated MeshDivisionEdges for the
        /// specified face.
        /// Edges will automatically be reversed in order to match the winding
        /// order of the face if necessary.
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public IList<MeshDivisionEdge> GetEdgesForFace(MeshFace face)
        {
            var result = new List<MeshDivisionEdge>(face.Count);
            for (int i = 0; i < face.Count; i++)
            {
                string edgeID = MeshDivisionEdge.IDFor(face, i);
                if (Contains(edgeID))
                {
                    MeshDivisionEdge edge = this[edgeID];
                    if (edge.Start != face[i]) result.Add(edge.Reversed());
                    else result.Add(edge);
                }
            }
            return result;
        }

        #endregion
    }
}

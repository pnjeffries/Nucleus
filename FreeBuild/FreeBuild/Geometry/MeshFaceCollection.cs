using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A collection of mesh face objects
    /// </summary>
    [Serializable]
    public class MeshFaceCollection : UniquesCollection<MeshFace>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MeshFaceCollection() { }

        /// <summary>
        /// Initialise this collection with the specified set of faces
        /// </summary>
        /// <param name="faces"></param>
        public MeshFaceCollection(IEnumerable<MeshFace> faces) : base()
        {
            foreach(MeshFace face in faces)
            {
                Add(face);
            }
        }

        /// <summary>
        /// Initialise this collection containing the specified face
        /// </summary>
        /// <param name="face"></param>
        public MeshFaceCollection(MeshFace face) : base()
        {
            Add(face);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove from this collection all faces which contain any vertices
        /// shared with the specified vertex collection
        /// </summary>
        /// <param name="vertices"></param>
        public void RemoveAllWithVertices(VertexCollection vertices)
        {
            for (int i = Count -1; i >= 0; i--)
            {
                if (this[i].ContainsAnyVertex(vertices)) RemoveAt(i);
            }
        }

        /// <summary>
        /// Extract the boundary curves of each mesh face in this collection
        /// </summary>
        /// <returns></returns>
        public ShapeCollection ExtractFaceBoundaries()
        {
            ShapeCollection result = new Geometry.ShapeCollection();
            foreach (MeshFace face in this)
            {
                result.Add(face.GetBoundary());
            }
            return result;
        }

        /// <summary>
        /// Trim this collection of faces to only those which fit within the specified boundary polygon
        /// on the XY plane
        /// </summary>
        /// <param name="boundary"></param>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public MeshFaceCollection TrimToPolygonXY(IList<Vertex> boundary, IList<Vertex> vertices = null)
        {
            MeshFaceCollection result = new MeshFaceCollection();
            foreach (MeshFace face in this)
            {
                IList<MeshFace> splitFaces = Intersect.PolygonOverlapXY<MeshFace>(face, boundary, vertices);
                if (splitFaces != null) foreach (MeshFace sFace in splitFaces) result.Add(sFace);
            }
            return result;
        }

        #endregion
    }
}

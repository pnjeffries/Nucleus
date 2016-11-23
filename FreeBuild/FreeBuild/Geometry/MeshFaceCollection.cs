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

        #endregion
    }
}

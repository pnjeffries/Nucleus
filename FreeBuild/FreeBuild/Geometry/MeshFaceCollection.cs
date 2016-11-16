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

        #endregion
    }
}

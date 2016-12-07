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
        public VertexGeometryCollection ExtractFaceBoundaries()
        {
            VertexGeometryCollection result = new Geometry.VertexGeometryCollection();
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

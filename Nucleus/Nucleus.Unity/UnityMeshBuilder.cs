using Nucleus.Geometry;
using Nucleus.Meshing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Mesh builder class to create Unity meshes
    /// </summary>
    public class UnityMeshBuilder : MeshBuilderBase<UnityEngine.Mesh>
    {
        #region Properties

        /// <summary>
        /// Toggle to produce double-sided faces.
        /// Set to true to have each added face also create a second face
        /// with inverted normal direction.
        /// </summary>
        public bool DoubleSided { get; set; } = false;

        /// <summary>
        /// Temporary vertex collection
        /// </summary>
        private List<Vector3> _Vertices = new List<Vector3>();

        /// <summary>
        /// Temporary triangle indices collection
        /// </summary>
        private List<int> _Triangles = new List<int>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new UnityMeshBuilder
        /// </summary>
        public UnityMeshBuilder()
        {
            Mesh = new UnityEngine.Mesh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new tri face to the mesh.
        /// By default, the winding order is counter-clockwise.
        /// </summary>
        /// <param name="v1">The first vertex index</param>
        /// <param name="v2">The second vertex index</param>
        /// <param name="v3">The third vertex index</param>
        /// <returns>The new face index</returns>
        /// <remarks>
        /// As the Unity convention for face normals is 
        /// clockwise rather than counter-clockwise winding, 
        /// the order of the vertices is changed to produce
        /// the same face normal.
        /// If the DoubleSided option is set to true, this will
        /// automatically create a second triangle with the opposite
        /// orientation, also duplicating vertices to accomplish this
        /// for each face.  As this will result in faces sharing
        /// vertices on the 'outside' face but not the 'inside', this
        /// may produce some odd effects.
        /// </remarks>
        public override int AddFace(int v1, int v2, int v3)
        {
            _Triangles.Add(v2);
            _Triangles.Add(v1);
            _Triangles.Add(v3);

            if (DoubleSided)
            {
                _Triangles.Add(AddVertex(_Vertices[v1].ToNucleusVector()));
                _Triangles.Add(AddVertex(_Vertices[v2].ToNucleusVector()));
                _Triangles.Add(AddVertex(_Vertices[v3].ToNucleusVector()));
            }

            return _Triangles.Count - 1;
        }

        /// <summary>
        /// Add a new quad face to the mesh.
        /// By default, the winding order is counter-clockwise.
        /// Unity meshes do not support quads so instead this will
        /// be resolved as two triangles.
        /// </summary>
        /// <param name="v1">The first vertex index</param>
        /// <param name="v2">The second vertex index</param>
        /// <param name="v3">The third vertex index</param>
        /// <param name="v4">The fourth vertex index</param>
        /// <returns>The new face index</returns>
        public override int AddFace(int v1, int v2, int v3, int v4)
        {
            AddFace(v1, v2, v3);
            return AddFace(v1, v3, v4);
        }

        /// <summary>
        /// Add a new vertex to the mesh
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>The new vertex index</returns>
        /// <remarks>The returned vertex indices should be sequential</remarks>
        public override int AddVertex(Vector pt)
        {
            _Vertices.Add(ToUnity.Convert(pt));
            return _Vertices.Count - 1;
        }

        /// <summary>
        /// Add a new vertex to the mesh.
        /// This operation will set the VertexIndex property of the vertex.
        /// </summary>
        /// <param name="v"></param>
        /// <returns>The new vertex index</returns>
        /// <remarks>The returned indices should be sequential and the 
        /// VertexIndex property of the input vertex should be set.</remarks> 
        public override int AddVertex(Vertex v)
        {
            v.Number = AddVertex(v.Position);
            return v.Number;
        }

        /// <summary>
        /// Finalize the mesh building.
        /// Will apply any necessary last steps to the mesh generation.
        /// </summary>
        /// <returns></returns>
        public override bool Finalize()
        {
            Mesh.SetVertices(_Vertices);
            Mesh.SetTriangles(_Triangles, 0);
            Mesh.RecalculateNormals();
            return base.Finalize();
        }

        #endregion
    }
}

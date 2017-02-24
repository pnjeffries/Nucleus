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

using FreeBuild.Extensions;
using FreeBuild.Meshing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A type of surface represented by a mesh of vertices and triangular or quadrangular faces
    /// </summary>
    [Serializable]
    public class Mesh : Surface
    {
        #region Properties

        /// <summary>
        /// Is this mesh valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                //TODO!
                return true;
            }
        }

        /// <summary>
        /// Private backing field for Vertices collection
        /// </summary>
        private VertexCollection _Vertices;

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The mesh may contain as many vertices as you like, with the connecting topology
        /// described by the Faces collection.
        /// </summary>
        public override VertexCollection Vertices { get { return _Vertices; } }

        /// <summary>
        /// Private backing field for Faces property
        /// </summary>
        private MeshFaceCollection _Faces;

        /// <summary>
        /// The collection of faces which describe the topology of the mesh.
        /// </summary>
        public MeshFaceCollection Faces
        {
            get
            {
                if (_Faces == null) _Faces = new MeshFaceCollection();
                return _Faces;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Mesh()
        {
            _Vertices = new VertexCollection(this);
        }

        /// <summary>
        /// Initialise a mesh with the specified set of vertex locations
        /// </summary>
        /// <param name="points"></param>
        public Mesh(IList<Vector> points) : this()
        {
            foreach (Vector pt in points)
            {
                Vertices.Add(new Vertex(pt));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new vertex to this mesh
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The index of the new vertex</returns>
        public int AddVertex(Vector position)
        {
            Vertices.Add(new Vertex(position));
            return Vertices.Count - 1;
        }

        /// <summary>
        /// Add a new triangular face to this mesh.
        /// The vertex indices provided should reference valid vertices
        /// already added to this mesh.
        /// </summary>
        /// <param name="v0">The first vertex index</param>
        /// <param name="v1">The second vertex index</param>
        /// <param name="v2">The third vertex index</param>
        /// <returns>The index of the new face</returns>
        public int AddFace(int v0, int v1, int v2)
        {
            Faces.Add(new MeshFace(Vertices[v0], Vertices[v1], Vertices[v2]));
            return Faces.Count - 1;
        }

        /// <summary>
        /// Add a new quadrangular face to this mesh.
        /// The vertex indices provided should reference valid vertices
        /// already added to this mesh.
        /// </summary>
        /// <param name="v0">The first vertex index</param>
        /// <param name="v1">The second vertex index</param>
        /// <param name="v2">The third vertex index</param>
        /// <param name="v3">The fourth vertex index</param>
        /// <returns>The index of the new face</returns>
        public int AddFace(int v0, int v1, int v2, int v3)
        {
            Faces.Add(new MeshFace(Vertices[v0], Vertices[v1], Vertices[v2], Vertices[v3]));
            return Faces.Count - 1;
        }

        /// <summary>
        /// Calculate the surface area of this mesh's faces
        /// </summary>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public override double CalculateArea(out Vector centroid)
        {
            throw new NotImplementedException();
            foreach (MeshFace face in Faces)
            {
                //TODO!
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Generate a set of mesh faces that represent a delaunay triangulation in the XY plane of the
        /// specified set of vertices.
        /// Based on the algorithm described here: http://paulbourke.net/papers/triangulate/
        /// </summary>
        /// <param name="vertices">The vertices to mesh between</param>
        /// <param name="faces">Optional.  The face collection to which to add the triangles.
        /// If null or ommitted, a new MeshFaceCollection will be created.</param>
        /// <param name="bounds">The bounding box that contains the triangulation.  Should encompass all vertices
        /// and, for later voronoi generation, any bounding geometry.  If null, the bounding box of the vertices
        /// themselves will be used.</param>
        /// <param name="clean">Clean up construction Super Triangle.  If true, any remaining fragments of the
        /// initial generating supertriangle will be removed from the output face list.  Set this to false
        /// if planning on using this mesh for subsequent voronoi generation so that the edge cells can be extended.</param>
        /// <param name="outerVerts">Optional.  If input and non-null, this collection will be populated with the exterior vertices - those connected
        /// to the original supertriangle</param>
        public static MeshFaceCollection DelaunayTriangulationXY(VertexCollection vertices, 
            MeshFaceCollection faces = null, BoundingBox bounds = null, bool clean = true, VertexCollection outerVerts = null)
        {
            List<Vertex> vertexList = vertices.ToList();
            vertexList.Sort();

            if (faces == null) faces = new MeshFaceCollection();

            if (bounds == null) bounds = new BoundingBox(vertexList);
            else bounds = new BoundingBox(bounds);

            bounds.Scale(5); //Provide a little wriggle room!

            // Meshing starts with one 'super triangle' that encloses all vertices.
            // This will be removed at the end
            MeshFace superTriangle = DelaunayTriangle.GenerateSuperTriangleXY(bounds);
            faces.Add(superTriangle);

            // Include each vertex in the meshing one at a time
            foreach (Vertex v in vertexList)
            {
                IList<MeshEdge> edges = new List<MeshEdge>(); //The edges of replaced triangles

                for (int i = faces.Count - 1; i >= 0; i--)
                {
                    MeshFace face = faces[i];
                    if (face.XYCircumcircleContainmentQuickCheck(v)) //The vertex lies within the circumcircle of this face
                    {
                        //The edges of the triangle are added to the current edge set...
                        for (int j = 0; j < face.Count; j++)
                        {
                            edges.Add(face.GetEdge(j));
                        }
                        //...and the triangle is removed.
                        faces.RemoveAt(i);
                    }
                }

                //Remove duplicate edges to retain only the convex hull of edges.
                //edges.RemoveDuplicates();

                //Replaced with bespoke version 
                for (int i = edges.Count - 2; i >= 0; i--)
                {
                    MeshEdge itemA = edges[i];
                    for (int j = edges.Count - 1; j > i; j--)
                    {
                        if (itemA.Equals(edges[j]))
                        {
                            edges.RemoveAt(j);
                            edges.RemoveAt(i);
                            j--;
                            continue;
                        }
                    }
                }

                // Add triangle fan between all remaining edges and the new vertex
                foreach (MeshEdge edge in edges)
                {
                    faces.Add(new DelaunayTriangle(edge, v));
                }
            }

            // Extract outer vertices
            if (outerVerts != null)
            {
                var outerFaces = faces.AllWithVertices(new VertexCollection(superTriangle));
                foreach (Vertex v in outerFaces.ExtractVertices()) outerVerts.Add(v);
            }

            // Remove the super triangle and any triangles still attached to it
            if (clean) faces.RemoveAllWithVertices(new VertexCollection(superTriangle));

            return faces;
        }

        /// <summary>
        /// Convert a set of mesh faces generated via delaunay triangulation into its dual -
        /// the voronoi diagram of the vertices.
        /// </summary>
        /// <param name="triangles"></param>
        /// <returns></returns>
        public static Dictionary<Vertex, MeshFace> VoronoiFromDelaunay(VertexCollection vertices, MeshFaceCollection faces, bool weighted = false)
        {
            var cells = new Dictionary<Vertex, MeshFace>(); //The generated voronoi cells

            foreach (Vertex v in vertices)
            {
                cells.Add(v, new MeshFace()); //Create empty cells
            }

            foreach (MeshFace face in faces)
            {
                Vertex newVert;
                if (weighted)
                {
                    face.WeightedVoronoiPoints(cells);
                }
                else
                {
                    newVert = new Vertex(face.XYCircumcentre);

                    foreach (Vertex v in face)
                    {
                        if (cells.ContainsKey(v))
                        {
                            MeshFace cell = cells[v];
                            cell.Add(newVert);
                        }
                    }
                }
            }

            //TODO: Deal with cells on the edge

            //Sort cell vertices anticlockwise around vertex
            foreach (KeyValuePair<Vertex, MeshFace> kvp in cells)
            {
                kvp.Value.SortVerticesAntiClockwise(kvp.Key.Position);
            }

            return cells;
        }

        #endregion
    }
}

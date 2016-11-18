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
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The mesh may contain as many vertices as you like, with the connecting topology
        /// described by the Faces collection.
        /// </summary>
        public override VertexCollection Vertices { get; }

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
            Vertices = new VertexCollection(this);
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
        public static MeshFaceCollection DelaunayTriangulationXY(VertexCollection vertices, MeshFaceCollection faces = null)
        {
            List<Vertex> vertexList = vertices.ToList();
            vertexList.Sort();

            if (faces == null) faces = new MeshFaceCollection();

            // Meshing starts with one 'super triangle' that encloses all vertices.
            // This will be removed at the end
            MeshFace superTriangle = DelaunayTriangle.GenerateSuperTriangleXY(new BoundingBox(vertexList));
                //vertices.BoundingBox());
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
        

                //Add triangle fan between all remaining edges and the new vertex
                foreach (MeshEdge edge in edges)
                {
                    faces.Add(new DelaunayTriangle(edge, v));
                }
            }

            //Remove the super triangle and any triangles still attached to it
            faces.RemoveAllWithVertices(new VertexCollection(superTriangle));

            return faces;
        }

        /// <summary>
        /// Convert a set of mesh faces generated via 
        /// </summary>
        /// <param name="triangles"></param>
        /// <returns></returns>
        public static Dictionary<Vertex, MeshFace> VoronoiFromDelaunay(VertexCollection vertices, MeshFaceCollection faces)
        {
            var cells = new Dictionary<Vertex, MeshFace>(); //The generated voronoi cells

            foreach (Vertex v in vertices)
            {
                cells.Add(v, new MeshFace()); //Create empty cells
            }

            foreach (MeshFace face in faces)
            {
                Vector centre = face.XYCircumcentre;
                Vertex newVert = new Vertex(centre);
                foreach (Vertex v in face)
                {
                    MeshFace cell = cells[v];
                    cell.Add(newVert);
                }
            }

            //TODO: Sort cell vertices anticlockwise around centroid

            return cells;
        }

        #endregion
    }
}

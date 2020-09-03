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

using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Meshing;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
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
        [CollectionCopy(CopyBehaviour.DUPLICATE, CopyBehaviour.DUPLICATE)]
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

        /// <summary>
        /// Get the total number of faces in the mesh
        /// </summary>
        public override int FaceCount
        {
            get
            {
                return Faces.Count;
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

        /// <summary>
        /// Initialise a mesh with the specified set of nodes.
        /// </summary>
        /// <param name="nodes">The collection of nodes.  Vertices will
        /// be created at the node locations and bound to the nodes.</param>
        /// <param name="addFace">If true, a face will be added to the mesh joining all of the
        /// vertices.</param>
        public Mesh(IList<Node> nodes, bool addFace = false) : this()
        {
            foreach (Node n in nodes)
            {
                Vertices.Add(new Vertex(n));
            }
            if (addFace)
            {
                Faces.Add(new MeshFace(Vertices));
            }
        }

        /// <summary>
        /// Initialise a mesh with the specified vertices and faces.
        /// The vertices used should not already form part of any other geometry definition.
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="faces"></param>
        public Mesh(VertexCollection verts, MeshFaceCollection faces) : this()
        {
            foreach (Vertex v in verts) Vertices.Add(v);
            foreach (MeshFace f in faces) Faces.Add(f);
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
        /// Add a new face to this mesh.
        /// The vertex indices provided should reference valid vertices
        /// already added to this mesh.
        /// </summary>
        /// <param name="vertexIndices">The vertex indices which the 
        /// face should join</param>
        /// <returns>The index of the new face</returns>
        public int AddFace(IEnumerable<int> vertexIndices)
        {
            var face = new MeshFace();
            foreach (int i in vertexIndices)
            {
                face.Add(Vertices[i]);
            }
            Faces.Add(face);
            return Faces.Count - 1;
        }

        /// <summary>
        /// Calculate the surface area of this mesh's faces.
        /// </summary>
        /// <returns></returns>
        public override double CalculateArea()
        {
            double result = 0;
            foreach (MeshFace face in Faces)
            {
                double faceArea = face.CalculateArea();
                result += faceArea;
            }
            return result;
        }

        /// <summary>
        /// Calculate the surface area of this mesh's faces.
        /// </summary>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public override double CalculateArea(out Vector centroid)
        {
            double result = 0;
            centroid = new Vector();
            foreach (MeshFace face in Faces)
            {
                Vector faceCentroid;
                double faceArea = face.CalculateArea(out faceCentroid);
                result += faceArea;
                centroid += faceCentroid * faceArea;
            }
            centroid /= result;
            return result;
        }

        /// <summary>
        /// Calculate the tributary area of each vertex in this mesh,
        /// calculated by summing an even distribution of the area of
        /// each face to which each vertex belongs.
        /// Returns an array of area values, one for each vertex in this
        /// mesh, in order.
        /// </summary>
        /// <returns></returns>
        public double[] VertexTributaryAreas()
        {
            double[] result = new double[Vertices.Count];
            Vertices.AssignVertexIndices(0);
            foreach (MeshFace face in Faces)
            {
                double area = face.CalculateArea()/face.Count;
                foreach (Vertex v in face)
                {
                    if (v.Number >= 0 && v.Number < result.Length)
                    {
                        result[v.Number] += area;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Invert the direction of all the face normals in this mesh by reversing
        /// the order of vertices in all faces.
        /// </summary>
        public void FlipNormals()
        {
            foreach (var face in Faces)
                face.FlipNormal();
        }

        /// <summary>
        /// Re-order the vertices in the faces of this mesh if necessary so that the
        /// face normals are aligned as closely as possible with the specified
        /// vector.
        /// </summary>
        /// <param name="alignTo">The vector to align the normals with</param>
        /// <returns>True if any face was flipped, false if all remained as-was.</returns>
        public bool AlignNormals(Vector alignTo)
        {
            bool result = false;
            foreach (var face in Faces)
                if (face.AlignNormal(alignTo))
                    result = true;
            return result;
        }

        public override string ToString()
        {
            return "Mesh";
        }

        /// <summary>
        /// Assign numbers to the vertices in this mesh based on their position in the collection.
        /// The vertex Number property will be written to and may be overwritten if a previous value
        /// has been stored there.
        /// </summary>
        public void AssignVertexNumbers(int firstNumber = 0)
        {
            for (int i = 0; i < _Vertices.Count; i++)
            {
                _Vertices[i].Number = i + firstNumber;
            }
        }

        /// <summary>
        /// Generate a list of the links between edges in this mesh
        /// </summary>
        /// <returns></returns>
        public IList<MeshEdgeLink> GenerateEdgeLinks()
        {
            var linkDictionary = new Dictionary<string, MeshEdgeLink>();
            foreach (MeshFace face in Faces)
            {
                face.EnsureEdgeLinkGeneration(linkDictionary);
            }
            return linkDictionary.Values.ToList();
        }

        /// <summary>
        /// Generate a set of division edges for use in face subdivision
        /// </summary>
        /// <returns></returns>
        public MeshDivisionEdgeCollection GenerateDivisionEdges()
        {
            return new MeshDivisionEdgeCollection(Faces);
        }

        /// <summary>
        /// Returns a refinement of this mesh, formed by
        /// subdividing mesh faces to produce a quad-dominant
        /// mesh with edge lengths of approximately the target
        /// length or lower.  Utilises the 'Jeffries More-Trouble-
        /// Than-It's-Worth Mesh Subdivision Algorithm.'
        /// Note that unlike typical mesh subdivision routines 
        /// (Cutmull-Clark, etc.) the edges of each face do not
        /// have to each have the same number of subdivisions -
        /// the algorithm is capable of transitioning across faces
        /// to allow for more consistent mesh density.
        /// </summary>
        /// <param name="targetEdgeLength"></param>
        /// <returns></returns>
        public Mesh Refined(double targetEdgeLength)
        {
            var dupFaces = Faces.FastDuplicate();
            dupFaces.FreshVertices();
            var newFaces = dupFaces.Refine(targetEdgeLength);
            return new Mesh(newFaces.ExtractVertices(), newFaces);
        }

        /// <summary>
        /// Get the local coordinate system of a point on the mesh
        /// </summary>
        /// <param name="i">The face index</param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="orientation"></param>
        /// <param name="xLimit"></param>
        /// <returns></returns>
        public override CartesianCoordinateSystem LocalCoordinateSystem(int i, double u, double v, Angle orientation, Angle xLimit)
        {
            if (Faces != null && Faces.Count > i)
            {
                MeshFace face = Faces[i];
                return face.GetPlane();
            }
            return null;
        }

        /// <summary>
        /// Project the specified point onto this mesh along the global
        /// z-axis, returning all z-coordinates 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public IList<double> ProjectPoint(Vector point)
        {
            var result = new List<double>();
            foreach (MeshFace face in Faces)
            {
                double z = face.ProjectPoint(point);
                if (!double.IsNaN(z)) result.Add(z);
            }
            return result;
        }

        /// <summary>
        /// Write this mesh to a string in OBJ format
        /// </summary>
        /// <returns></returns>
        public string ToOBJ()
        {
            string result = null;
            using (var tW = new StringWriter())
            {
                ToOBJ(tW);
                result = tW.ToString();
            }
            return result;
        }

        /// <summary>
        /// Write this mesh to a file in OBJ format
        /// </summary>
        /// <param name="path"></param>
        public void ToOBJ(FilePath filePath)
        {
            using (Stream stream = new FileStream(filePath,
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None))
            {
                ToOBJ(stream);
                stream.Flush();
                stream.Close();
            }
        }

        /// <summary>
        /// Write this mesh to a stream in OBJ format
        /// </summary>
        /// <returns></returns>
        public void ToOBJ(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                ToOBJ(writer);
            }
        }

        /// <summary>
        /// Write this mesh to a stream in OBJ format
        /// </summary>
        /// <returns></returns>
        public void ToOBJ(TextWriter writer)
        {
            writer.WriteLine("# Vertices");
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertex v = Vertices[i];
                writer.Write("v ");
                writer.Write(v.X);
                writer.Write(" ");
                writer.Write(v.Y);
                writer.Write(" ");
                writer.Write(v.Z);
                writer.WriteLine();
                v.Number = i;
            }
            writer.WriteLine();
            writer.WriteLine("# Faces");
            foreach(MeshFace face in Faces)
            {
                writer.Write("f");
                foreach (Vertex v in face)
                {
                    writer.Write(" ");
                    writer.Write(v.Number);
                }
            }
            writer.Flush();
        }

        /// <summary>
        /// Load mesh geometry in Wavefront OBJ format from a string.
        /// The new geometry will be added to any existing already in this mesh.
        /// </summary>
        /// <param name="obj"></param>
        public void FromOBJ(string obj)
        {
            using (StringReader reader = new StringReader(obj))
            {
                FromOBJ(reader);
            }
        }

        /// <summary>
        /// Load mesh geometry in Wavefront OBJ format from a stream.
        /// The new geometry will be added to any existing already in this mesh.
        /// </summary>
        /// <param name="stream"></param>
        public void FromOBJ(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                FromOBJ(reader);
            }
        }

        /// <summary>
        /// Load mesh geometry in Wavefront OBJ format.
        /// The new geometry will be added to any existing already in this mesh.
        /// </summary>
        /// <param name="reader"></param>
        public void FromOBJ(TextReader reader)
        {
            int offset = Vertices.Count;
            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine().Trim().Before('#');
                string[] tokens = line.Split(' ');
                if (tokens.Length > 0)
                {
                    string key = tokens[0];
                    if (key.EqualsIgnoreCase("v"))
                    {
                        // Vertex:
                        Vertex v = new Vertex(Vector.FromTokensList(tokens, 1));
                        Vertices.Add(v);
                    }
                    else if (key.EqualsIgnoreCase("f"))
                    {
                        // Face:
                        MeshFace face = new MeshFace();
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            string[] subTokens = tokens[i].Split('/');
                            if (subTokens.Length > 0)
                            {
                                int vi = subTokens[0].ToInteger(-1);
                                if (vi >= 0 && vi + offset < Vertices.Count)
                                {
                                    face.Add(Vertices[vi]);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Find the set of curves which represent the intersections between
        /// the faces in this mesh and a plane aligned with the global XY plane
        /// at the specified z-level
        /// </summary>
        /// <param name="zLevel">The z-coordinate of the flat plane</param>
        /// <param name="join">If true, the resultant curves will be automatically
        /// joined together into as few polycurves as possible.</param>
        /// <returns></returns>
        public CurveCollection IntersectPlane(double zLevel, bool join = false)
        {
            var result = new CurveCollection();
            foreach (var face in Faces)
            {
                var line = face.IntersectPlane(zLevel);
                if (line != null) result.Add(line);
            }
            if (join) result = result.JoinCurves(true);
            return result;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Generate a set of triangular mesh faces that fill a simple polygon via an 'Ear Clipping' process.
        /// Based loosely on https://www.geometrictools.com/Documentation/TriangulationByEarClipping.pdf
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static MeshFaceCollection EarClippingXY(IList<Vertex> polygon, MeshFaceCollection faces = null)
        {
            if (faces == null) faces = new MeshFaceCollection();

            VertexCollection verts = new VertexCollection(polygon);
            verts.RemoveSequentialCoincident();
            int angleSign = verts.ClockwiseTestSum().Sign();
            int maxTries = verts.Count * 10;

            for (int i = 0; i < maxTries && verts.Count > 2; i++)
            {
                Vertex v0 = verts.GetWrapped(i);
                Vertex v1 = verts.GetWrapped(i + 1);
                Vertex v2 = verts.GetWrapped(i + 2);
                Vector dir0 = v1.Position - v0.Position;
                Vector dir1 = v2.Position - v1.Position;
                Angle a1 = dir0.RotationAngleXY(dir1);

                if (a1 * angleSign >= 0) continue; //Reflex

                bool isEar = true;

                // TODO: Cache reflex as a pre-step and and only check those?
                for (int j = i + 3; j < i + verts.Count; j++)
                {
                    Vertex vT = verts.GetWrapped(j);
                    if (Triangle.XYContainment(vT,v0,v1,v2))
                    {
                        isEar = false;
                        break;
                    }
                }

                if (isEar)
                {
                    // Is an ear:
                    var face = new MeshFace(v0, v1, v2);
                    faces.Add(face);
                    verts.Remove(v1.GUID);
                    i--; // Step back and try next
                }
            }

            return faces;
        }

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
        /// to the original supertriangle.  If 'clean' is false, this will instead return the supertriangle vertices.</param>
        public static MeshFaceCollection DelaunayTriangulationXY(VertexCollection vertices, 
            MeshFaceCollection faces = null, BoundingBox bounds = null, bool clean = true, VertexCollection outerVerts = null)
        {
            List<Vertex> vertexList = vertices.ToList();
            // Sort by X-coordinate:
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
                if (clean)
                {
                    var outerFaces = faces.AllWithVertices(new VertexCollection(superTriangle));
                    foreach (Vertex v in outerFaces.ExtractVertices()) outerVerts.Add(v);
                }
                else foreach (Vertex v in superTriangle) outerVerts.Add(v);
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

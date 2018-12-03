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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
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
        /// Get all mesh faces in this collection that contain any vertices
        /// shared with the specified vertex collection
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public MeshFaceCollection AllWithVertices(VertexCollection vertices)
        {
            var result = new MeshFaceCollection();
            foreach(MeshFace face in this)
            { 
                if (face.ContainsAnyVertex(vertices)) result.Add(face);
            }
            return result;
        }

        /// <summary>
        /// Extract the boundary curves of each mesh face in this collection
        /// </summary>
        /// <returns></returns>
        public CurveCollection ExtractFaceBoundaries()
        {
            CurveCollection result = new CurveCollection();
            foreach (MeshFace face in this)
            {
                result.Add(face.GetBoundary());
            }
            return result;
        }

        /// <summary>
        /// Extract all vertices belonging to the faces in this collection
        /// </summary>
        /// <returns></returns>
        public VertexCollection ExtractVertices()
        {
            var result = new VertexCollection();
            foreach(MeshFace face in this)
            {
                foreach(Vertex v in face)
                {
                    if (!result.Contains(v.GUID)) result.Add(v);
                }
            }
            return result;
        }

        /// <summary>
        /// Trim this collection of faces to only those which fit within the specified boundary polygon
        /// on the XY plane
        /// </summary>
        /// <param name="boundary"></param>
        /// <param name="vertices">Optional.  The collection of vertices to which new vertices created during
        /// this process should be added.</param>
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

        /// <summary>
        /// Remove from this collection any faces whose circumcentre falls outside the specified boundary on the XY plane
        /// </summary>
        /// <param name="boundary"></param>
        public void CullOutsideXY(IList<Vector> boundary)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (!boundary.PolygonContainmentXY(this[i].AveragePoint())) RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove from this collection any faces whose circumcentre falls outside the specified boundary on the XY plane
        /// </summary>
        /// <param name="boundary"></param>
        public void CullOutsideXY(Curve boundary)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (!boundary.EnclosesXY(this[i].AveragePoint())) RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove from this collection any faces whose circumcentre falls inside the specified boundary on the XY plane
        /// </summary>
        /// <param name="boundary"></param>
        public void CullInsideXY(IList<Vector> boundary)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (boundary.PolygonContainmentXY(this[i].AveragePoint())) RemoveAt(i);
            }
        }

        /// <summary>
        /// Find and return the first face in this collection that encloses the specified
        /// point in the XY plane.
        /// </summary>
        /// <param name="point">The point to test for</param>
        /// <returns></returns>
        public MeshFace FaceContainingXY(Vector point)
        {
            foreach (MeshFace face in this)
            {
                if (face.PolygonContainmentXY(point)) return face;
            }
            return null;
        }

        /// <summary>
        /// Quadrangulate this mesh by merging adjacent tris into quads.
        /// The algorithm will prioritise merging the longest edges first
        /// </summary>
        public void Quadrangulate()
        {
            var sortedPairs = new SortedList<double, Pair<MeshFace, MeshFace>>(Count);

            // Find adjacent pairs of tris and sort by edge length
            for (int i = 0; i < Count - 1; i++)
            {
                MeshFace faceA = this[i];
                if (faceA.IsTri)
                {
                    for (int j = i + 1; j < Count; j++)
                    {
                        MeshFace faceB = this[j];
                        if (faceB.IsTri)
                        {
                            double length = faceA.SharedEdgeLengthSquared(faceB);
                            if (length > 0)
                                sortedPairs.AddSafe(length, Pair.Create(faceA, faceB));
                        }
                    }
                }
            }

            // Reverse through pairs and join:
            for (int i = sortedPairs.Count - 1; i >= 0; i--)
            {
                var pair = sortedPairs.Values[i];
                if (Contains(pair.First.GUID) && Contains(pair.Second.GUID))
                {
                    Remove(pair.First);
                    Remove(pair.Second);
                    Add(pair.First.MergeWith(pair.Second));
                }
            }

            /*
            // Populate lists:
            var sortedLists = new SortedList<double, IList<MeshFace>>(Count / 2);

            foreach (MeshFace face in this)
            {
                if (face.IsTri)
                {
                    double longEdge = face.LongestEdgeLengthSquared();

                    if (!sortedLists.ContainsKey(longEdge))
                        sortedLists.Add(longEdge, new MeshFaceCollection());

                    sortedLists[longEdge].Add(face);
                }
            }

            foreach (IList<MeshFace> faceSet in sortedLists.Values)
            {
                for (int i = 0; i < faceSet.Count - 1; i++)
                {
                    MeshFace faceA = faceSet[i];
                    for (int j = i + 1; j < faceSet.Count; j++)
                    {
                        MeshFace faceB = faceSet[j];
                        if (faceA.SharedVertexCount(faceB) == 2) // Has a shared edge
                        {
                            // Merge faces and replace:
                            Remove(faceA);
                            Remove(faceB);
                            Add(faceA.MergeWith(faceB));

                            faceSet.RemoveAt(j);
                            j = faceSet.Count;
                        }
                    }
                }
            }
            */
        }

        /// <summary>
        /// Extract all edges from the faces in this collection.
        /// This may include duplicates where edges are shared between
        /// faces.
        /// </summary>
        /// <returns></returns>
        public IList<MeshEdge> ExtractAllEdges()
        {
            var result = new List<MeshEdge>(Count * 3);
            foreach (MeshFace face in this)
            {
                result.AddRange(face.GetEdges());
            }
            return result;
        }

        /// <summary>
        /// Extract all naked edges from the faces in this collection.
        /// Shared edges will be automatically
        /// removed.
        /// </summary>
        /// <returns></returns>
        public IList<MeshEdge> ExtractNakedEdges()
        {
            var result = ExtractAllEdges();
            result.RemoveAllDuplicates();
            return result;
        }

        /// <summary>
        /// Extract all naked edges from the faces in this collection.
        /// Shared edges will be automatically
        /// removed.
        /// </summary>
        /// <returns></returns>
        public IList<MeshEdge> ExtractUniqueEdges()
        {
            var result = ExtractAllEdges();
            result.RemoveDuplicates();
            return result;
        }

        /// <summary>
        /// Refine and subdivide the faces in this collection to
        /// reduce the maximum edge length below the specified 
        /// </summary>
        /// <param name="maxEdgeLength"></param>
        /// <returns></returns>
        public MeshFaceCollection Refine(double maxEdgeLength)
        {
            MeshFaceCollection result = new MeshFaceCollection();
            var edges = new MeshDivisionEdgeCollection(this);
            edges.SubDivideAll(maxEdgeLength);
            foreach (var face in this)
            {
                var faceEdges = edges.GetEdgesForFace(face);
                face.Refine(faceEdges, result);
            }
            return result;
        }

        #endregion
    }
}

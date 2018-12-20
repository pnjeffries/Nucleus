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
using Nucleus.Exceptions;
using Nucleus.Extensions;
using Nucleus.Maths;
using Nucleus.Meshing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Mesh face representation.  Describes a face which connects three or more
    /// vertices in a mesh.  The face is defined as a series of vertex indices in the
    /// parent mesh.
    /// </summary>
    [Serializable]
    [CollectionCopy(CopyBehaviour.DUPLICATE, CopyBehaviour.MAP_OR_COPY)]
    public class MeshFace : 
        List<Vertex>, IUnique, IDuplicatable
        //VertexCollection, IUnique //Slower!
    {
        #region Properties

        /// <summary>
        /// Internal backing member for GUID property
        /// </summary>
        [Copy(CopyBehaviour.DO_NOT_COPY)]
        private Guid _GUID = Guid.NewGuid();

        /// <summary>
        /// The GUID of this object, which can be used to uniquely identify it. 
        /// </summary>
        public Guid GUID { get { return _GUID; } }

        /// <summary>
        /// Is this mesh face definition valid?
        /// </summary>
        public bool IsValid { get { return Count >= 3 && Count <= 4; } }

        /// <summary>
        /// Is this mesh face triangular?
        /// i.e. does it have three vertices?
        /// </summary>
        public bool IsTri { get { return Count == 3; } }

        /// <summary>
        /// Is this mesh face quadrangular?
        /// i.e. does it have four vertices?
        /// </summary>
        public bool IsQuad { get { return Count == 4; } }

        /// <summary>
        /// Get the circumcentre of the first three vertices of this face on the XY plane
        /// </summary>
        /// <returns></returns>
        internal virtual Vector XYCircumcentre
        {
            get { return CalculateXYCircumcentre(); }
        }

        /// <summary>
        /// Get the normal vector of this face
        /// </summary>
        public Vector Normal
        {
            get
            {
                return CalculateNormal();
                //TODO: Possibly cache this?
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MeshFace() : base(4)
        {

        }

        /// <summary>
        /// Initialise a triangular mesh face between the three specified vertices
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public MeshFace(Vertex v1, Vertex v2, Vertex v3) : base(3)
        {
            Add(v1);
            Add(v2);
            Add(v3);
        }

        /// <summary>
        /// Initialise a quadrangular mesh face between the four specified vertices
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        public MeshFace(Vertex v1, Vertex v2, Vertex v3, Vertex v4) : base(4)
        {
            Add(v1);
            Add(v2);
            Add(v3);
            Add(v4);
        }

        /// <summary>
        /// Initialise a triangular mesh face between an edge and a vertex
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="v"></param>
        public MeshFace(MeshEdge edge, Vertex v) : base(3)
        {
            Add(edge.Start);
            Add(edge.End);
            Add(v);
        }

        /// <summary>
        /// Initialise a MeshFace containing the specified set of vertices
        /// </summary>
        /// <param name="vertices">A set of vertices to use to build this face.</param>
        public MeshFace(IList<Vertex> vertices) : base(vertices.Count)
        {
            foreach (Vertex v in vertices)
            {
                Add(v);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the vertices of this face
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public virtual void Set(Vertex v1, Vertex v2, Vertex v3)
        {
            this[0] = v1;
            this[1] = v2;
            this[2] = v3;
            while (Count > 3) RemoveAt(3);
        }

        /// <summary>
        /// Set the vertices of this face
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        public virtual void Set(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
        {
            this[0] = v1;
            this[1] = v2;
            this[2] = v3;
            this[3] = v4;
            while (Count > 4) RemoveAt(4);
        }

        /// <summary>
        /// Set the vertices of this face
        /// </summary>
        /// <param name="vertices"></param>
        public virtual void Set(IList<Vertex> vertices)
        {
            Clear();
            foreach (Vertex v in vertices)
            {
                Add(v);
            }
        }

        /// <summary>
        /// Get the vector describing the path along the edge
        /// starting with the vertex at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector GetEdgeVector(int index)
        {
            return this.GetWrapped(index + 1).Position - this[index].Position;
        }

        /// <summary>
        /// Get the edge of this face at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MeshEdge GetEdge(int index)
        {
            return new MeshEdge(this[index], this.GetWrapped(index + 1));
        }

        /// <summary>
        /// Get all edges of this face
        /// </summary>
        /// <returns></returns>
        public MeshEdge[] GetEdges()
        {
            MeshEdge[] result = new MeshEdge[Count];
            for (int i = 0; i < Count; i++)
            {
                result[i] = GetEdge(i);
            }
            return result;
        }

        /// <summary>
        /// Get the edge link of this face at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual MeshEdgeLink GetEdgeLink(int index)
        {
            return new MeshEdgeLink(this[index], this.GetWrapped(index + 1), this);
        }

        /// <summary>
        /// Generate a short string description of this edge in the format
        /// "[Lowest Vertex Number]:[Highest Vertex Number]".  This can be used
        /// as a key to check for shared edges easily.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetEdgeNumberKey(int index)
        {
            Vertex v0 = this[index];
            Vertex v1 = this.GetWrapped(index + 1);
            if (v0.Number < v1.Number) return v0.Number.ToString() + ":" + v1.Number.ToString();
            else return v1.Number.ToString() + ":" + v0.Number.ToString();
        }

        /// <summary>
        /// Ensure that the edge link at the specified index has been included in the specified dictionary
        /// </summary>
        /// <param name="index">The edge index</param>
        /// <param name="linkDictionary"></param>
        public virtual MeshEdgeLink EnsureEdgeLinkGeneration(int index, Dictionary<string, MeshEdgeLink> linkDictionary)
        {
            string key = GetEdgeNumberKey(index);
            if (linkDictionary.ContainsKey(key))
            {
                MeshEdgeLink link = linkDictionary[key];
                if (!link.Faces.Contains(this)) link.Faces.Add(this);
                return link;
            }
            else
            {
                MeshEdgeLink link = GetEdgeLink(index);
                linkDictionary.Add(key, link);
                return link;
            }
        }

        /// <summary>
        /// Ensure that edge links for this face have been included in the specified dictionary
        /// </summary>
        /// <param name="linkDictionary"></param>
        public void EnsureEdgeLinkGeneration(Dictionary<string, MeshEdgeLink> linkDictionary)
        {
            for (int i = 0; i < Count; i++)
            {
                EnsureEdgeLinkGeneration(i, linkDictionary);
            }
        }

        /// <summary>
        /// Find the closest point on this mesh face to a test point, expressed as a
        /// vector in 3d space.  This may be a position on the face or it may
        /// be an edge of the face depending on the relative location
        /// of the test point.
        /// </summary>
        /// <param name="toPoint">The test point to find the closest point to</param>
        /// <returns></returns>
        public Vector ClosestPoint(Vector toPoint)
        {
            if (Count < 3) return Vector.Unset;
            else if (Count == 3) return Vector.TriangleClosestPoint(this[0].Position, this[1].Position, this[2].Position, toPoint);
            else
            {
                // Treat as triangle fan around start vertex and find closest point on each sub-tri
                Vector result = Vector.Unset;
                double minDistSqd = 0;
                Vector vA = this[0].Position;
                for (int i = 0; i < Count - 2; i++)
                {
                    Vector vB = this[i + 1].Position;
                    Vector vC = this[i + 2].Position;
                    Vector v = Vector.TriangleClosestPoint(vA, vB, vC, toPoint);
                    double distSqd = v.DistanceToSquared(toPoint);
                    if (i == 0 || distSqd < minDistSqd)
                    {
                        result = v;
                        minDistSqd = distSqd;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Find the index of the closest edge in this mesh face to the specified point
        /// </summary>
        /// <param name="toPoint">The point to check against</param>
        /// <returns></returns>
        public int ClosestEdge(Vector toPoint)
        {
            int minIndex = -1;
            double minDist = double.MaxValue;
            for (int i = 0; i < Count; i++)
            {
                double dist = DistanceToEdgeSquared(i, toPoint);
                if (minIndex < 0 || dist < minDist)
                {
                    minIndex = i;
                    minDist = dist;
                }
            }
            return minIndex;
        }

        /// <summary>
        /// Calculate the square of the distance from the specified position to the edge at the specified index
        /// </summary>
        /// <param name="index">The edge index to test against</param>
        /// <param name="point">The point to test</param>
        /// <returns></returns>
        public double DistanceToEdgeSquared(int index, Vector point)
        {
            Vector cPt = Line.ClosestPoint(this[index].Position, this.GetWrapped(index + 1).Position, point);
            return point.DistanceToSquared(cPt);
        }

        /// <summary>
        /// Get the squared length of the edge at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double EdgeLengthSquared(int index)
        {
            return this[index].DistanceToSquared(this.GetWrapped(index + 1));
        }

        /// <summary>
        /// Get the length of the edge at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double EdgeLength(int index)
        {
            return this[index].DistanceTo(this.GetWrapped(index + 1));
        }

        /// <summary>
        /// Get the index of the longest edge of this face
        /// </summary>
        /// <returns></returns>
        public int LongestEdge()
        {
            int iMax = 0;
            double lMax = 0;
            for (int i = 0; i < Count; i++)
            {
                double l = EdgeLengthSquared(i);
                if (l > lMax)
                {
                    lMax = l;
                    iMax = i;
                }
            }
            return iMax;
        }

        /// <summary>
        /// Get the squared length of the longest edge of this face
        /// </summary>
        /// <returns></returns>
        public double LongestEdgeLengthSquared()
        {
            double lMax = 0;
            for (int i = 0; i < Count; i++)
            {
                double l = EdgeLengthSquared(i);
                if (l > lMax)
                {
                    lMax = l;
                }
            }
            return lMax;
        }

        /// <summary>
        /// Get the length of the longest edge of this face
        /// </summary>
        /// <returns></returns>
        public double LongestEdgeLength()
        {
            return LongestEdgeLengthSquared().Root();
        }

        /// <summary>
        /// Is the edge at the specified index shared with the specified other face?
        /// </summary>
        /// <param name="index">The edge index</param>
        /// <param name="withFace">The face to check against</param>
        /// <param name="otherEdgeIndex">The index of the shared edge on the other face</param>
        /// <returns></returns>
        public bool IsSharedEdge(int index, MeshFace withFace, out int otherEdgeIndex)
        {
            int i0 = withFace.IndexOf(this[index]);
            if (i0 >= 0)
            {
                int i1 = withFace.IndexOf(this.GetWrapped(index + 1));
                if (i1 >= 0)
                {
                    int dist = (i0 - i1).Abs();
                    otherEdgeIndex = Math.Min(i0, i1);
                    return dist == 1 || dist == withFace.Count;
                }
            }
            otherEdgeIndex = -1;
            return false;
        }

        /// <summary>
        /// Is the edge at the specified index shared with the specified other face?
        /// </summary>
        /// <param name="index">The edge index</param>
        /// <param name="withFace">The face to check against</param>
        /// <returns></returns>
        public bool IsSharedEdge(int index, MeshFace withFace)
        {
            int dump;
            return IsSharedEdge(index, withFace, out dump);

        }

        /// <summary>
        /// Find the index of the edge of this face which is shared with.
        /// If the result is -1, no shared edge could be found.
        /// </summary>
        /// <param name="withFace">The face to check against</param>
        /// <param name="otherEdgeIndex">The index of the shared edge 
        /// on the other face</param>
        /// <returns></returns>
        public int SharedEdgeIndex(MeshFace withFace, out int otherEdgeIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                if (IsSharedEdge(i, withFace, out otherEdgeIndex)) return i;
            }
            otherEdgeIndex = -1;
            return -1;
        }

        /// <summary>
        /// Extract the position vectors of all vertices in this face to
        /// an array.
        /// </summary>
        /// <returns></returns>
        public Vector[] ExtractPoints()
        {
            Vector[] result = new Vector[Count];
            for (int i = 0; i < Count; i++)
            {
                result[i] = this[i].Position;
            }
            return result;
        }

        /// <summary>
        /// Get the curve that describes the boundary of this face
        /// </summary>
        /// <returns></returns>
        public virtual Curve GetBoundary()
        {
            return new PolyLine(ExtractPoints(), true);
        }

        /// <summary>
        /// Find the circumcentre of the first three vertices of this face on the XY plane
        /// </summary>
        /// <returns></returns>
        protected Vector CalculateXYCircumcentre()
        {
            Vector A = this[0].Position;
            Vector B = this[1].Position;
            Vector C = this[2].Position;

            Vector AB = B - A;
            Vector BC = C - B;

            Vector midAB = A + AB * 0.5;
            Vector midBC = B + BC * 0.5;

            Vector centre = Intersect.LineLineXY(midAB, AB.PerpendicularXY(), midBC, BC.PerpendicularXY());
            return centre;
        }

        /// <summary>
        /// DOES NOT WORK!  DO NOT USE!
        /// </summary>
        /// <param name="cells"></param>
        internal void WeightedVoronoiPoints(Dictionary<Vertex, MeshFace> cells)
        {
            Vertex vA = this[0];
            Vertex vB = this[1];
            Vertex vC = this[2];  

            double wA = 1.0;
            double wB = 1.0;
            double wC = 1.0;

            if (vA is WeightedVertex)
                wA = ((WeightedVertex)vA).Weighting;
            if (vB is WeightedVertex)
                wB = ((WeightedVertex)vB).Weighting;
            if (vC is WeightedVertex)
                wC = ((WeightedVertex)vC).Weighting;

            Vector A = vA.Position;
            Vector B = vB.Position;
            Vector C = vC.Position;

            Vector AB = B - A;
            Vector BC = C - B;
            Vector CA = A - C;

            Vector ptAB = A + AB * wA / (wA + wB);
            Vector ptBC = B + BC * wB / (wB + wC);
            Vector ptCA = C + CA * wC / (wC + wA);

            Vector cAB = Intersect.LineLineXY(ptAB, AB.PerpendicularXY(), ptBC, BC.PerpendicularXY());
            Vector cBC = Intersect.LineLineXY(ptBC, BC.PerpendicularXY(), ptCA, CA.PerpendicularXY());
            Vector cCA = Intersect.LineLineXY(ptCA, CA.PerpendicularXY(), ptAB, AB.PerpendicularXY());

            Vector centre = (cAB + cBC + cCA) / 3;

            //Vector midAB = A + AB * 0.5;
            //Vector midBC = B + BC * 0.5;

            //Vector centre = Intersect.LineLineXY(midAB, AB.PerpendicularXY(), midBC, BC.PerpendicularXY());

            //Vector ACen = (centre - A) * wA;
            //Vector BCen = (centre - B) * wB;
            //Vector CCen = (centre - C) * wC;

            //centre += (ACen + BCen + CCen) / 3;// (wA + wB + wC);
            //Vector centre = (ptAB + ptBC + ptCA) / 3;

            Vertex vAB = new Vertex(ptAB);
            Vertex vBC = new Vertex(ptBC);
            Vertex vCA = new Vertex(ptCA);
            Vertex vCentre = new Vertex(centre);

            if (cells.ContainsKey(vA))
            {
                MeshFace cell = cells[vA];
                cell.Add(vAB);
                cell.Add(vCentre);
                cell.Add(vCA);
            }

            if (cells.ContainsKey(vB))
            {
                MeshFace cell = cells[vB];
                cell.Add(vAB);
                cell.Add(vCentre);
                cell.Add(vBC);
            }

            if (cells.ContainsKey(vC))
            {
                MeshFace cell = cells[vC];
                cell.Add(vBC);
                cell.Add(vCentre);
                cell.Add(vCA);
            }

        }

        /// <summary>
        /// Tests whether a point lies in the circumcircle of this face.  Used during delaunay
        /// triangulation on the XY plane.
        /// Currently works only for triangle faces which are counterclockwise on XY plane.
        /// Hence declared internal until it can be made more robust.
        /// </summary>
        /// <param name="point">The position vector to test</param>
        /// <returns></returns>
        internal virtual bool XYCircumcircleContainmentQuickCheck(Vertex point)
        {
            // See https://en.wikipedia.org/wiki/Delaunay_triangulation#Algorithms for methodology -
            // Calculates the determinant of a matrix containing the vertex and point coordinates
            // If vertices are counterclockwise and point lies inside, determinant will be > 0

            Vertex v0 = this[0];
            Vertex v1 = this[1];
            Vertex v2 = this[2];
            double x0 = v0.X - point.X;
            double y0 = v0.Y - point.Y;
            double x1 = v1.X - point.X;
            double y1 = v1.Y - point.Y;
            double x2 = v2.X - point.X;
            double y2 = v2.Y - point.Y;

            double det_01 = x0 * y1 - x1 * y0;
            double det_12 = x1 * y2 - x2 * y1;
            double det_20 = x2 * y0 - x0 * y2;

            double square0 = x0 * x0 + y0 * y0;
            double square1 = x1 * x1 + y1 * y1;
            double square2 = x2 * x2 + y2 * y2;

            double det = square0 * det_12 + square1 * det_20 + square2 * det_01;
            return (det > 0);

        }

        /// <summary>
        /// Does this face contain any vertex in the specified list of vertices?
        /// </summary>
        /// <param name="other">The other mesh face to test against</param>
        /// <returns></returns>
        public bool ContainsAnyVertex(VertexCollection vertices)
        {
            foreach (Vertex v in this)
            {
                if (vertices.Contains(v.GUID)) return true;
            }
            return false;
        }

        /// <summary>
        /// Count the number of vertices in the specified collection that
        /// also form part of this face
        /// </summary>
        /// <returns></returns>
        public int SharedVertexCount(IEnumerable<Vertex> vertices)
        {
            int count = 0;
            foreach (Vertex v in vertices)
            {
                if (Contains(v)) count++;
            }
            return count;
        }

        /// <summary>
        /// Find the length of the first found edge shared with the specified other face.
        /// If no shared edge exists, will return 0.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double SharedEdgeLengthSquared(MeshFace other)
        {
            for (int i = 0; i < Count; i++)
            {
                Vertex v = this[i];
                if (other.Contains(v))
                {
                    Vertex v2 = this.GetWrapped(i + 1);
                    if (other.Contains(v2))
                    {
                        return v.DistanceToSquared(v2);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Calculate the 'squareness' of the resultant face if this
        /// face were to be joined with another one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double SharedEdgeSquareness(MeshFace other)
        {
            int iA = SharedEdgeIndex(other, out int iB);
            if (iA < 0) return double.NaN; //No shared edge

            // Note: Currently only considering the dot product between the two(+) non-shared
            // edges and not those between the edges which are to be joined.
            return SumOfDotProductsOfEdgesNotIn(other) + other.SumOfDotProductsOfEdgesNotIn(this);
        }

        /// <summary>
        /// Calculate the sum of dot products of adjacent edges of this face
        /// which are not shared with the specified connected face.
        /// Used in determining face 'squareness' for face quadrangulation.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private double SumOfDotProductsOfEdgesNotIn(MeshFace other)
        {
            double result = 0;
            for (int i = 0; i < Count; i++)
            {
                Vertex v1 = this[i];
                if (!other.Contains(v1))
                {
                    Vertex v0 = this.GetWrapped(i - 1);
                    Vertex v2 = this.GetWrapped(i + 1);
                    Vector d1 = (v1.Position - v0.Position).Unitize();
                    Vector d2 = (v2.Position - v1.Position).Unitize();
                    //TODO: Unitize?
                    result += d1.Dot(d2);
                }
            }
            return result;
        }

        /// <summary>
        /// Sort the vertices of this face counter-clockwise around the specified point
        /// in plan (i.e. in the XY plane).  This will essentially align this face 'upwards'.
        /// Note that this will only work in the case of convex polygons where the ordering
        /// of vertices is not important.
        /// </summary>
        /// <param name="aroundPt"></param>
        public void SortVerticesAntiClockwise(Vector aroundPt)
        {
            this.Sort(
                delegate (Vertex v1, Vertex v2)
                {
                    return aroundPt.AngleTo(v1.Position).CompareTo(aroundPt.AngleTo(v2.Position));
                });
        }

        /// <summary>
        /// Sort the vertices of this face counter-clockwise about the circumcentre
        /// of the first three vertices of the face in the XY plane.  This will essentially
        /// align this face 'upwards'.
        /// </summary>
        public void SortVerticesAntiClockwise()
        {
            SortVerticesAntiClockwise(XYCircumcentre);
        }

        /// <summary>
        /// Is this face clockwise in the XY plane?
        /// </summary>
        /// <returns></returns>
        public bool IsClockwiseXY()
        {
            double sum = 0;
            for (int i = 0; i < Count; i++)
            {
                Vertex v1 = this[i];
                Vertex v2 = this.GetWrapped(i + 1);
                sum += (v2.X - v1.X) * (v2.Y + v1.Y);
            }
            return sum > 0;
        }

        /// <summary>
        /// Merge this mesh face together with another, adjacent one
        /// to form a single larger face
        /// </summary>
        /// <param name="other">The mesh face to merge with this one.
        /// Must share two vertices and one edge with this face and be in the same orientation.</param>
        /// <returns></returns>
        public MeshFace MergeWith(MeshFace other)
        {
            var result = new MeshFace();
            bool switched = false;
            for (int i = 0; i < Count; i++)
            {
                // Add the vertices from this face to the result
                Vertex v = this[i];
                result.Add(v);
                if (!switched)
                {
                    
                    int iOther = other.IndexOf(v);
                    if (iOther >= 0) // Found first shared vertex
                    {
                        Vertex nextV = this.GetWrapped(i + 1);
                        if (other.Contains(nextV))
                        {
                            switched = true;
                            // Add the bridging vertices from the other face to the result:
                            for (int j = 1; j < other.Count - 1; j++)
                            {
                                Vertex v2 = other.GetWrapped(j + iOther);
                                result.Add(v2);
                                // Currently assuming that the faces share two vertices and a single edge...
                                //TODO: Check
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Calculate the surface area of this face using the half cross-product formula.
        /// For quad and n-gon faces, the face will be treated as a triangle fan around the first vertex
        /// (i.e. it is assumed to be non-reentrant).
        /// </summary>
        /// <returns></returns>
        public double CalculateArea()
        {
            double result = 0;
            Vertex vA = this[0];
            for (int i = 0; i < Count - 2; i++)
            {
                Vertex vB = this[i + 1];
                Vertex vC = this[i + 2];
                Vector AB = vB.Position - vA.Position;
                Vector AC = vC.Position - vA.Position;
                result += (AB.Cross(AC)).Magnitude() / 2;
            }
            return result;
        }

        /// <summary>
        /// Calculate the normal vector for this face
        /// </summary>
        /// <returns></returns>
        public Vector CalculateNormal()
        {
            Vector result = new Vector();
            Vertex vA = this[0];
            for (int i = 0; i < Count - 2; i++)
            {
                Vertex vB = this[i + 1];
                Vertex vC = this[i + 2];
                Vector AB = vB.Position - vA.Position;
                Vector AC = vC.Position - vA.Position;
                result += AB.Cross(AC);
            }
            if (Count > 3) result /= Count - 2; //If quad (or n-gon), average the result
            return result;
        }

        /// <summary>
        /// Re-order the vertices in this face so that the face normal
        /// faces in the opposite direction.
        /// </summary>
        /// <returns></returns>
        public void FlipNormal()
        {
            Reverse();
            // If caching normal - flip it here.
        }

        /// <summary>
        /// Re-order the vertices in this face if necessary so that the
        /// face normal is aligned as closely as possible with the specified
        /// vector.
        /// </summary>
        /// <param name="alignTo">The vector to align the normal with</param>
        /// <returns>True if the face was flipped, false if it remained as-was.</returns>
        public bool AlignNormal(Vector alignTo)
        {
            if (Normal.Dot(alignTo) < 0)
            {
                FlipNormal();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Calculate and return the plane defined by the first three vertices of this
        /// face.
        /// </summary>
        /// <returns></returns>
        public Plane GetPlane()
        {
            //TODO: This could fail if the three points are co-linear.  Fix!
            if (Count > 2)
                return Plane.From3Points(this[0].Position, this[1].Position, this[2].Position);
            else return null;
        }

        /// <summary>
        /// Project a point along the global Z-axis onto this mesh face.
        /// Returns the Z-coordinate of the projected point or double.NaN
        /// if the point does not lie within the triangle on the XY plane.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double ProjectPoint(Vector point)
        {
            Vertex vA = this[0];
            for (int i = 0; i < Count - 2; i++)
            {
                Vertex vB = this[i + 1];
                Vertex vC = this[i + 2];
                double z = Triangle.ZCoordinateOfPoint(point.X, point.Y, 
                    vA.Position, vB.Position, vC.Position, true);
                if (!double.IsNaN(z)) return z;
            }
            return double.NaN;
        }

        /// <summary>
        /// Convert this mesh face into an array of integers
        /// representing the indices of the vertices in this face.
        /// The 'Number' property of the face vertices should
        /// have been populated prior to calling this function (for
        /// e.g. via the AssingVertexIndices function on VertexCollection)
        /// </summary>
        /// <returns></returns>
        public int[] ToIndexArray()
        {
            var result = new int[Count];
            for (int i = 0; i < Count; i++)
            {
                result[i] = this[i].Number;
            }
            return result;
        }

        /// <summary>
        /// Refine and subdivide this face based on the specified set of pre-divided edges
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="addTo"></param>
        public void Refine(IList<MeshDivisionEdge> edges, MeshFaceCollection addFaceTo)
        {
            //Vector midPt = this.AveragePoint();
            Vector[] midPts = MedialAxisPoints();

            int minDivisions = edges.MinDelegateValue(edge => edge.Divisions);

            for (int offset = 1; offset < (minDivisions+1)/2; offset++)
            {
                var cornerVerts = new List<Vertex>();
                for (int i = 0; i < Count; i++)
                {
                    // Generate corner vertices:
                    var edge1 = edges.GetWrapped(i - 1);
                    var edge2 = edges[i];
                    var newVert = RefinementVertex(edge1, edge2, midPts[i], 1);
                    cornerVerts.Add(newVert);
                }

                var newEdges = new List<MeshDivisionEdge>();
                for (int i = 0; i < Count; i++)
                {
                    // Generate new edge vertices:
                    var vert1 = cornerVerts[i];
                    var vert2 = cornerVerts.GetWrapped(i + 1);
                    var newEdge = new MeshDivisionEdge(vert1, vert2);
                    var oldEdge = edges[i];
                    newEdge.SubDivide(oldEdge.Divisions - 2);
                    newEdges.Add(newEdge);
                }

                for (int i = 0; i < Count; i++)
                {
                    // Generate new faces:
                    var outEdge0 = edges.GetWrapped(i - 1);
                    var outEdge1 = edges[i];
                    var inEdge1 = newEdges[i];
                    // Corner face:
                    var firstFace = new MeshFace(
                        outEdge1.Start, outEdge1.Vertices[1],
                        inEdge1.Start, outEdge0.Vertices.FromEnd(1));
                    addFaceTo.Add(firstFace);
                    for (int j = 0; j < inEdge1.Vertices.Count - 1; j++)
                    {
                        // Edge faces:
                        var edgeFace = new MeshFace(
                            outEdge1.Vertices[j + 1], outEdge1.Vertices[j + 2],
                            inEdge1.Vertices[j + 1], inEdge1.Vertices[j]);
                        addFaceTo.Add(edgeFace);
                    }
                }

                edges = newEdges;
            }

            //TODO: 
            // - Infill central gap somehow

            
        }

        /// <summary>
        /// Generate a new vertex 
        /// </summary>
        /// <param name="edge1"></param>
        /// <param name="edge2"></param>
        /// <param name="midPt"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Vertex RefinementVertex(MeshDivisionEdge edge1, MeshDivisionEdge edge2, Vector midPt, int offset)
        {
            Vertex startPt = edge1.End;
            Vertex original1 = edge1.Vertices.FromEnd(offset);
            Vertex original2 = edge2.Vertices[offset];
            double t1 = offset / ((edge1.Vertices.Count - 1) * 0.5);
            double t2 = offset / ((edge2.Vertices.Count - 1)* 0.5);
            double t = (t1 + t2) / 2;
            Vector newPt = startPt.Position.Interpolate(midPt, t);
            return new Vertex(newPt);
        }

        /// <summary>
        /// Calculate the branch points of the medial axis skeleton within
        /// this face, returned as an array of points
        /// </summary>
        /// <returns></returns>
        private Vector[] MedialAxisPoints()
        {
            if (IsQuad)
            {
                // Calculate edge lengths:
                double lU0 = EdgeLength(0);
                double lV0 = EdgeLength(1);
                double lU1 = EdgeLength(2);
                double lV1 = EdgeLength(3);
                // Find length of each mid-axis:
                double lUMid = (lU0 + lU1) * 0.5;
                double lVMid = (lV0 + lV1) * 0.5;
                if (lUMid > lVMid)
                {
                    // Longer in U-axis
                    Vector start = this[0].Position.Interpolate(this[3].Position, 0.5);
                    Vector end = this[1].Position.Interpolate(this[2].Position, 0.5);
                    Vector mA0 = start.Interpolate(end, (lV1 / 2) / lUMid);
                    Vector mA1 = end.Interpolate(start, (lV0 / 2) / lUMid);
                    return new Vector[] { mA0, mA1, mA1, mA0 };
                }
                else
                {
                    // Longer in V-axis
                    Vector start = this[0].Position.Interpolate(this[1].Position, 0.5);
                    Vector end = this[2].Position.Interpolate(this[3].Position, 0.5);
                    Vector mA0 = start.Interpolate(end, (lU0 / 2) / lVMid);
                    Vector mA1 = end.Interpolate(start, (lU1 / 2) / lVMid);
                    return new Vector[] { mA0, mA0, mA1, mA1 };
                }
            }
            else if (IsTri)
            {
                Vector midPt = this.AveragePoint();
                var result = new Vector[3];
                for (int i = 0; i < 3; i++) result[i] = midPt;
                return result;
            }
            else
                throw new NotImplementedException(
                    "Medial Axis generation not implemented for n-gons.");
        }

        #endregion

    }
}

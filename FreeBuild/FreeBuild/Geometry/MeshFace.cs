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
using FreeBuild.Extensions;
using FreeBuild.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Mesh face representation.  Describes a face which connects three or more
    /// vertices in a mesh.  The face is defined as a series of vertex indices in the
    /// parent mesh.
    /// </summary>
    [Serializable]
    public class MeshFace : 
        List<Vertex>, IUnique
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
        /// Get the squared length of the edge at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double EdgeLengthSquared(int index)
        {
            return this[index].DistanceToSquared(this.GetWrapped(index + 1));
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
        /// <returns></returns>
        public bool IsSharedEdge(int index, MeshFace withFace)
        {
            int i0 = withFace.IndexOf(this[index]);
            if (i0 >= 0)
            {
                int i1 = withFace.IndexOf(this.GetWrapped(index + 1));
                if (i1 >= 0)
                {
                    int dist = (i0 - i1).Abs();
                    return dist == 1 || dist == withFace.Count;
                }
            }
            return false;
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

        public void WeightedVoronoiPoints(Dictionary<Vertex, MeshFace> cells)
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

        #endregion

    }
}

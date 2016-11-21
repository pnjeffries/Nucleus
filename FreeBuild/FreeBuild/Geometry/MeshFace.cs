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

        internal virtual void Set(Vertex v1, Vertex v2, Vertex v3)
        {
            this[0] = v1;
            this[1] = v2;
            this[2] = v3;
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

            Vector centre = Axis.IntersectXY(midAB, AB.PerpendicularXY(), midBC, BC.PerpendicularXY());
            return centre;
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
        /// Sort the vertices of this face counter-clockwise around the specified point
        /// in plan (i.e. in the XY plane).  This will essentially align this face 'upwards'
        /// </summary>
        /// <param name="aroundPt"></param>
        public void SortVerticesCounterClockwise(Vector aroundPt)
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
        public void SortVerticesCounterClockwise()
        {
            SortVerticesCounterClockwise(XYCircumcentre);
        }

        #endregion

    }
}

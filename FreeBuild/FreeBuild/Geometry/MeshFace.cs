using FreeBuild.Base;
using FreeBuild.Extensions;
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
    public class MeshFace : VertexCollection, IUnique
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

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MeshFace()
        {

        }

        /// <summary>
        /// Initialise a triangular mesh face between the three specified vertices
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public MeshFace(Vertex v1, Vertex v2, Vertex v3)
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
        public MeshFace(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
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
        public MeshFace(MeshEdge edge, Vertex v)
        {
            Add(edge.Start);
            Add(edge.End);
            Add(v);
        }

        #endregion

        #region Methods

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
        /// Tests whether a point lies in the circumcircle of this face.  Used during delaunay
        /// triangulation on the XY plane.
        /// Currently works only for triangle faces which are counterclockwise on XY plane.
        /// Hence declared internal until it can be made more robust.
        /// </summary>
        /// <param name="point">The position vector to test</param>
        /// <returns></returns>
        internal bool XYCircumcircleContainmentQuickCheck(Vector point)
        {
            // See https://en.wikipedia.org/wiki/Delaunay_triangulation#Algorithms for methodology -
            // Calculates the determinant of a matrix containing the vertex and point coordinates
            // If vertices are counterclockwise and point lies inside, determinant will be > 0

            double x0 = this[0].X - point.X;
            double y0 = this[0].Y - point.Y;
            double x1 = this[1].X - point.X;
            double y1 = this[1].Y - point.Y;
            double x2 = this[2].X - point.X;
            double y2 = this[2].Y - point.Y;

            double det_01 = x0 * y1 - x1 * y0;
            double det_12 = x1 * y2 - x2 * y1;
            double det_20 = x2 * y0 - x0 * y2;

            double square0 = x0 * x0 + y0 * y0;
            double square1 = x1 * x1 + y1 * y1;
            double square2 = x2 * x2 + y2 * y2;

            double det = square0 * det_12 + square1 * det_20 + square2 * det_01;
            return det > 0;
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
                if (vertices.Contains(v)) return true;
            }
            return false;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Generate a triangular mesh face that encompasses the specified bounding box
        /// Used as the initial stage of delaunary triangulation.
        /// The triangle will be created using temporary vertices that will not be added to
        /// the vertices of this mesh.
        /// </summary>
        /// <returns></returns>
        internal static MeshFace GenerateSuperTriangleXY(BoundingBox box)
        {
            box.Expand(1.0); //Provide a little wriggle room!

            Vertex v0 = new Vertex(box.MinX + box.SizeX * 2, box.MinY);
            Vertex v1 = new Vertex(box.MinX + 1, box.MinY + box.SizeY * 2);
            Vertex v2 = new Vertex(box.MinX, box.MinY);

            return new MeshFace(v0, v1, v2);
        }

        #endregion

    }
}

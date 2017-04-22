using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A class representing a mesh edge which stores references to
    /// the faces and vertices to which it is connected.
    /// Note that this is rather different in implementation to
    /// the temporary mesh edge structure
    /// </summary>
    [Serializable]
    public class MeshEdgeLink
    {
        #region Properties

        /// <summary>
        /// Private backing field for Start property
        /// </summary>
        private Vertex _Start;

        /// <summary>
        /// The vertex at the start of this edge
        /// </summary>
        public Vertex Start { get { return _Start; } }

        /// <summary>
        /// Private backing field for End property
        /// </summary>
        private Vertex _End;

        /// <summary>
        /// The vertex at the end of this edge
        /// </summary>
        public Vertex End { get { return _End; } }

        /// <summary>
        /// Get the start point of this edge
        /// </summary>
        public Vector StartPoint { get { return Start.Position; } }

        /// <summary>
        /// Get the end point of this edge
        /// </summary>
        public Vector EndPoint { get { return End.Position; } }

        /// <summary>
        /// Get the length of this edge
        /// </summary>
        public double Length
        {
            get { return StartPoint.DistanceTo(EndPoint); }
        }

        /// <summary>
        /// Get the squared length of this edge
        /// </summary>
        public double LengthSquared
        {
            get { return StartPoint.DistanceToSquared(EndPoint); }
        }

        /// <summary>
        /// Private backing field for Faces property
        /// </summary>
        private MeshFaceCollection _Faces = null;

        /// <summary>
        /// The collection of faces that adjoin this edge
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
        /// Is this a naked edge?  i.e. does it connect to a single face?  (or none)
        /// </summary>
        public bool IsNaked
        {
            get { return _Faces == null || _Faces.Count <= 1; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new empty linked mesh edge
        /// </summary>
        protected MeshEdgeLink()
        {

        }

        /// <summary>
        /// Initialise a new linked mesh edge between the two specified vertices
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public MeshEdgeLink(Vertex start, Vertex end)
        {
            _Start = start;
            _End = end;
        }

        /// <summary>
        /// Initialise a new linked mesh edge 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="face"></param>
        public MeshEdgeLink(Vertex start, Vertex end, MeshFace face) : this(start, end)
        {
            Faces.Add(face);
        }

        #endregion
    }

    /// <summary>
    /// Static extension methods related to the MeshEdgeLink class
    /// </summary>
    public static class MeshEdgeLinkExtensions
    {
        /// <summary>
        /// Extract the subset of this list of edges that are naked - i.e. they
        /// are not shared between two or more faces.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static IList<MeshEdgeLink> Naked(this IList<MeshEdgeLink> edges)
        {
            var result = new List<MeshEdgeLink>();
            foreach (MeshEdgeLink link in edges)
            {
                if (link.IsNaked) result.Add(link);
            }
            return result;
        }
    }
}

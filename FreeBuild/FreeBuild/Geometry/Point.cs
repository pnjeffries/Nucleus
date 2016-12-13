using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A point geometry object, consisting only of a single vertex.
    /// </summary>
    public class Point : VertexGeometry, IPosition
    {
        #region Properties

        /// <summary>
        /// Private backing field for Vertex property
        /// </summary>
        private Vertex _Vertex;

        /// <summary>
        /// The single vertex that represents the geometry of this Point
        /// </summary>
        public Vertex Vertex
        {
            get { return _Vertex; }
            set { _Vertex = value; NotifyGeometryUpdated(); }
        }

        public override bool IsValid
        {
            get
            {
                return _Vertex != null;
            }
        }

        public override VertexCollection Vertices
        {
            get
            {
                return new VertexCollection(Vertex);
            }
        }

        /// <summary>
        /// Get the position vector of this Point
        /// </summary>
        public Vector Position
        {
            get
            {
                if (_Vertex != null) return _Vertex.Position;
                else return Vector.Unset;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Protected parameterless constructor
        /// </summary>
        protected Point() : base() { }

        /// <summary>
        /// Vertex constructor.   Initialises this Point object with the specified vertex.
        /// </summary>
        /// <param name="vertex"></param>
        public Point(Vertex vertex) : this()
        {
            _Vertex = vertex;
        }

        /// <summary>
        /// Position vector constructor.
        /// Creates a Point object at the specified position.
        /// </summary>
        /// <param name="position"></param>
        public Point(Vector position)
        {
            _Vertex = new Vertex(position);
        }

        #endregion
    }
}

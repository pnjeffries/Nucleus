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
    [Serializable]
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
            set
            {
                _Vertex = value;
                _Vertices = null;
                NotifyGeometryUpdated();
            }
        }

        public override bool IsValid
        {
            get
            {
                return _Vertex != null;
            }
        }

        [NonSerialized]
        private VertexCollection _Vertices;

        public override VertexCollection Vertices
        {
            get
            {
                if (_Vertices == null) _Vertices = new VertexCollection(Vertex);
                return _Vertices;
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
        protected Point(GeometryAttributes attributes = null) : base()
        {
            Attributes = attributes;
        }

        /// <summary>
        /// Vertex constructor.   Initialises this Point object with the specified vertex.
        /// </summary>
        /// <param name="vertex"></param>
        public Point(Vertex vertex, GeometryAttributes attributes = null) : this(attributes)
        {
            _Vertex = vertex;
        }

        /// <summary>
        /// Position vector constructor.
        /// Creates a Point object at the specified position.
        /// </summary>
        /// <param name="position"></param>
        public Point(Vector position, GeometryAttributes attributes = null) : this(attributes)
        {
            _Vertex = new Vertex(position);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Point{" + Vertex.ToString() + "}";
        }

        #endregion
    }
}

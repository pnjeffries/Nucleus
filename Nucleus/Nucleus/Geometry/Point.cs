using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
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

        /// <summary>
        /// Is this point geometry valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return _Vertex != null && Position.IsValid();
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
            set
            {
                if (_Vertex == null) _Vertex = new Vertex(value);
                else _Vertex.Position = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default parameterless constructor
        /// </summary>
        protected Point() : base() { }

        /// <summary>
        /// Protected parameterless constructor
        /// </summary>
        protected Point(GeometryAttributes attributes) : base()
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

        /// <summary>
        /// Coordinates constructor.
        /// Creates a Point object at the specified coordinates.
        /// </summary>
        /// <param name="position"></param>
        public Point(double x, double y, double z = 0, GeometryAttributes attributes = null) : this(new Vector(x, y, z), attributes)
        {

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

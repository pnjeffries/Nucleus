using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A point cloud, consisting only of vertices with no connecting geometry.
    /// </summary>
    public class Cloud : Shape
    {
        #region Properties

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Private backing field for Vertices property
        /// </summary>
        private VertexCollection _Vertices;

        /// <summary>
        /// The vertices of this cloud
        /// </summary>
        public override VertexCollection Vertices { get { return _Vertices; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Cloud()
        {
            _Vertices = new VertexCollection(this);
        }

        /// <summary>
        /// Initialise a point cloud using the specified point locations
        /// </summary>
        /// <param name="points"></param>
        public Cloud(IEnumerable<Vector> points) : this()
        {
            foreach (Vector point in points)
            {
                _Vertices.Add(new Vertex(point));
            }
        }

        /// <summary>
        /// Initialise a point cloud containing a single point
        /// </summary>
        /// <param name="point"></param>
        public Cloud(Vector point) : this()
        {
            _Vertices.Add(new Vertex(point));
        }

        #endregion

    }
}

using FreeBuild.Base;
using FreeBuild.Model;
using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Base class for vertices - positions in space that
    /// form part of the definition (or are themselves derived from)
    /// a particular piece of geometry, and that may have additional
    /// attached data defining properties at that position.
    /// </summary>
    [Serializable]
    public class Vertex : Unique, IOwned<Shape>
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Position property
        /// </summary>
        private Vector _Position = Vector.Unset;

        /// <summary>
        /// The current position of this vertex.
        /// </summary>
        public Vector Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                NotifyPropertyChanged("Position");
                NotifyOwnerOfPositionUpdate();
            }
        }

        /// <summary>
        /// Private backing member variable for the Shape property
        /// </summary>
        private Shape _Owner = null;

        /// <summary>
        /// The shape (if any) that this vertex belongs to.
        /// </summary>
        public Shape Owner
        {
            get { return _Owner; }
            internal set { _Owner = value; }
        }

        /// <summary>
        /// Internal backing member for node property
        /// </summary>
        private Node _Node = null;

        /// <summary>
        /// The node, if any, that this vertex is attached to.
        /// This node may be shared with other vertices and represents
        /// a point of connection between them.
        /// By default, this property is null and this vertex is not
        /// connected to any other.
        /// </summary>
        public Node Node
        {
            get { return _Node; }
            set { _Node = value; NotifyPropertyChanged("Node"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Position constructor.
        /// Create a vertex with an explicitly defined position.
        /// </summary>
        /// <param name="position"></param>
        public Vertex(Vector position)
        {
            _Position = position;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transform this vertex by mapping it from local coordinates on the given system to
        /// global coordinates
        /// </summary>
        /// <param name="cSyatem">The coordinate system to use to map the vertex geometric data</param>
        public void MapTo(ICoordinateSystem cSystem)
        {
            Position = cSystem.LocalToGlobal(Position);
        }

        // <summary>
        /// Apply the specified transformation to this vertex, modifying it's geometric data.
        /// </summary>
        /// <param name="transform">THe transformation matrix.</param>
        public void Transform(Transform transform)
        {
            Position = Position.Transform(transform);
        }

        /// <summary>
        /// Notify the owning shape that the geometry of this vertex has been updated
        /// </summary>
        protected void NotifyOwnerOfPositionUpdate()
        {
            if (Owner != null)
                Owner.NotifyGeometryUpdated();
        }

        #endregion

    }
}

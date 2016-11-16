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
    public class Vertex : Unique, IOwned<Shape>, IPosition
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
        /// Shortcut property to get the X coordinate of the position of this vertex
        /// </summary>
        public double X { get { return _Position.X; } }

        /// <summary>
        /// Shortcut property to get the Y coordinate of the position of this vertex
        /// </summary>
        public double Y { get { return _Position.Y; } }

        /// <summary>
        /// Shortcut property to get the Z coordinate of the position of this vertex
        /// </summary>
        public double Z { get { return _Position.Z; } }

        /// <summary>
        /// Private backing member variable for the Shape property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
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

        /// <summary>
        /// Node constructor.
        /// Initialises a new vertex at the specified node position.
        /// </summary>
        /// <param name="node"></param>
        public Vertex(Node node)
        {
            _Position = node.Position;
            _Node = node;
        }

        /// <summary>
        /// Initialise a new vertex at the specified coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vertex(double x, double y, double z = 0)
        {
            _Position = new Vector(x, y, z);
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
        /// Apply the specified transformation to this vertex, modifying its geometric data.
        /// </summary>
        /// <param name="transform">The transformation matrix.</param>
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

        /// <summary>
        /// Calculate the offset of the position of this vertex from
        /// the node that it is connected to.
        /// </summary>
        /// <returns>The vector from this vertex's node position to its actual position</returns>
        public Vector NodalOffset()
        {
            if (_Node == null) return Vector.Unset; //TODO: Review?
            else return _Position - _Node.Position;
        }

        /// <summary>
        /// Generate a node at this vertex, if it does not already posess one.
        /// A new node will only be created if one does not exist and this vertex is part of
        /// an element's geometry definition.
        /// </summary>
        /// <param name="options"></param>
        public void GenerateNode(NodeGenerationParameters options)
        {
            if (Owner != null && Owner.Element != null)
            {
                Model.Model model = Owner.Element.Model;
                if (Node == null)
                {
                    if (model != null)
                    {
                        Node = model.Create.Node(Position, options.ConnectionTolerance, options.ExInfo);
                    }
                    //TODO: Also split nodes if they are too far apart
                }
            }
        }

        /// <summary>
        /// Copy the values of attached data objects (such as the connected node)
        /// from another vertex.  This is typically used when replacing one shape with another.
        /// </summary>
        /// <param name="other"></param>
        public void CopyAttachedDataFrom(Vertex other)
        {
            Node = other.Node;
            //Additional data should be copied here
        }

        #endregion

    }
}

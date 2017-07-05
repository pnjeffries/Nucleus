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

using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Model;
using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Base class for vertices - positions in space that
    /// form part of the definition (or are themselves derived from)
    /// a particular piece of geometry, and that may have additional
    /// attached data defining properties at that position.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Vertex( {X} , {Y} , {Z}) ")]
    public class Vertex : 
        Unique, IOwned<VertexGeometry>, IPosition, IComparable<Vertex>, IDataOwner
    {
        #region Static Fields

        /// <summary>
        /// The value set aside to indicate an unset vertex index
        /// </summary>
        public static int UnsetIndex = -1;

        #endregion

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
        /// Shortcut property to get or set the X coordinate of the position of this vertex
        /// </summary>
        public double X
        {
            get { return _Position.X; }
            set { Position = _Position.WithX(value); }
        }

        /// <summary>
        /// Shortcut property to get or set the Y coordinate of the position of this vertex
        /// </summary>
        public double Y
        {
            get { return _Position.Y; }
            set { Position = _Position.WithY(value); }
        }

        /// <summary>
        /// Shortcut property to get or set the Z coordinate of the position of this vertex
        /// </summary>
        public double Z
        {
            get { return _Position.Z; }
            set { Position = _Position.WithZ(value); }
        }

        /// <summary>
        /// Private backing member variable for the Shape property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private VertexGeometry _Owner = null;

        /// <summary>
        /// The shape (if any) that this vertex belongs to.
        /// Vertices removed from their owner will automatically be detatched from
        /// their node to prevent memory leaks.
        /// </summary>
        public VertexGeometry Owner
        {
            get { return _Owner; }
            internal set
            {
                _Owner = value;
                if (_Owner == null) Node = null;
            }
        }

        /// <summary>
        /// Get the element (if any) that this vertex forms part of the geometric definition for
        /// </summary>
        public Element Element
        {
            get { return Owner?.Element; }
        }

        /// <summary>
        /// Internal backing member for node property
        /// </summary>
        [Copy(CopyBehaviour.DO_NOT_COPY)]
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
            set
            {
                // De-register with old node
                if (_Node != null) _Node.Vertices.Remove(this);
                _Node = value;
                // Register with new node
                if (_Node != null) _Node.Vertices.TryAdd(this);
                NotifyPropertyChanged("Node");
            }
        }

        /// <summary>
        /// Private backing field for VertexIndex property
        /// </summary>
        [NonSerialized]
        private int _Number = UnsetIndex;

        /// <summary>
        /// The index of this vertex.
        /// This is a temporary value which will be populated during certain
        /// operations (such as drawing), but will not generally be populated automatically.
        /// This does *not* necessarily correspond to the position of this vertex in
        /// the containing shape.
        /// </summary>
        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        /// <summary>
        /// Private backing field for Data property
        /// </summary>
        [Copy(CopyBehaviour.DUPLICATE)]
        private VertexDataStore _Data;

        /// <summary>
        /// The store of data objects attached to this model object.
        /// This container can be used to add and retrieve data objects of specific
        /// types.
        /// </summary>
        public VertexDataStore Data
        {
            get
            {
                if (_Data == null) _Data = new VertexDataStore();
                return _Data;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public Vertex()
        {

        }

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

        /// <summary>
        /// Initialise a new vertex copying data from the specified other
        /// vertex
        /// </summary>
        /// <param name="other"></param>
        public Vertex(Vertex other)
        {
            _Position = other.Position;
            Node = other.Node;
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
        /// Notify this owner that a property of a data component has been changed.
        /// This may then be 'bubbled' upwards with a new event.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="propertyName"></param>
        public virtual void NotifyComponentPropertyChanged(object component, string propertyName)
        {
            //TODO: Review
            if (component == null)
            {
                NotifyPropertyChanged(string.Format("Data[{0}]", propertyName));
            }
            else NotifyPropertyChanged(string.Format("Data[{0}].{1}", component.GetType().Name, propertyName));
        }

        /// <summary>
        /// Check whether this object has any attached data components
        /// </summary>
        public bool HasData()
        {
            return _Data != null && _Data.Count > 0;
        }

        /// <summary>
        /// Check whether this object has any attached data components of the specified type
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public bool HasData(Type componentType)
        {
            if (typeof(IVertexDataComponent).IsAssignableFrom(componentType))
                return _Data != null && Data.Contains(componentType);
            return false;
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
        /// Set the position of this vertex by an offset vector from
        /// its node.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetPositionByNodalOffset(Vector offset)
        {
            if (_Node == null || !_Node.Position.IsValid()) return false;

            Position = _Node.Position + offset;
            return true;
        }

        /// <summary>
        /// Check whether this object has any attached data components of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasData<T>() where T : class, IVertexDataComponent
        {
            return _Data != null && Data.HasData<T>();
        }

        /// <summary>
        /// Get the data component of the specified type attached to this object (if one exists)
        /// </summary>
        /// <typeparam name="T">The type of the attached data component to be retrieved</typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : class, IVertexDataComponent
        {
            return _Data?.GetData<T>();
        }

        /// <summary>
        /// Get data of the specified generic type (or the closest available sub-type) attached to
        /// this object.  If no data component of the specified type is found then optionally a
        /// new one will be created.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <param name="create">If true, a new data component of the specified type will
        /// be created and returned should one not already exist.</param>
        /// <returns></returns>
        public T GetData<T>(bool create) where T : class, IVertexDataComponent, new()
        {
            return Data.GetData<T>(create);
        }

        /// <summary>
        /// Get all data within this store that is of the specified generic type or which
        /// is assignable to that type.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public IList<T> GetAllData<T>() where T : IVertexDataComponent
        {
            return Data.GetAllData<T>();
        }

        /// <summary>
        /// Attach a data component to this object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void SetData(IVertexDataComponent data)
        {
            Data.SetData(data);
        }

        /// <summary>
        /// Remove any attached data components of the specified type
        /// </summary>
        /// <param name="ofType"></param>
        public bool CleanData(Type ofType)
        {
            return Data.Remove(ofType);
        }

        public IList AllAttachedDataComponents()
        {
            return Data;
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
                }
                else
                {
                    // Node already exists - check and update it
                    double dist = Node.DistanceToSquared(this);
                    if (dist > options.ConnectionTolerance.Squared())
                    {
                        // Check for other connections that share this node:
                        if (Node.Vertices.Count > 1)
                        {
                            if (model != null)
                            {
                                // Node is too far away - split and create new node
                                Node = model.Create.Node(Position, options.ConnectionTolerance, options.ExInfo);
                            }
                        }
                        else
                        {
                            // Check for other nodes at the end position to connect to:
                            Node node = model.Nodes.ClosestNodeTo(Position, options.ConnectionTolerance);
                            if (node != null && !node.IsDeleted)
                            {
                                if (options.DeleteUnusedNodes) Node.Delete();
                                Node = node;
                            }
                            else
                            {
                                Node.Position = Position;
                            }
                        }
                    }
                }
                if (Node.IsDeleted) Node.Undelete(); //Make sure the node isn't deleted
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
            if (other.HasData())
            {
                _Data = other.Data.Duplicate();
            }
            //Additional data should be copied here
        }

        /// <summary>
        /// Comparison function for sorting.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Vertex other)
        {
            return _Position.X.CompareTo(other.X);
        }

        /// <summary>
        /// Covert this vertex to its string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Position.ToString();
        }

        #endregion

    }

}

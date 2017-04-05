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
using FreeBuild.Geometry;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A singular point which represents a shared connection point
    /// between multiple vertices within different objects.
    /// </summary>
    [Serializable]
    public class Node : DataOwner<NodeDataStore, INodeDataComponent, Node>, IPosition
    {
        #region Properties

        /// <summary>
        /// Internal backing member for Position property
        /// </summary>
        private Vector _Position = Vector.Unset;

        /// <summary>
        /// The spatial position of this node
        /// </summary>
        [AutoUI(300)]
        public Vector Position
        {
            get { return _Position; }
            set { _Position = value;  NotifyPropertyChanged("Position"); }
        }

        /// <summary>
        /// Private backing field for Vertices property.
        /// </summary>
        private VertexCollection _Vertices = null;

        /// <summary>
        /// The collection of vertices to which this node is connected
        /// </summary>
        public VertexCollection Vertices
        {
            get
            {
                if (_Vertices == null)
                {
                    _Vertices = new VertexCollection();
                }
                return _Vertices;
            }
        }

        ///// <summary>
        ///// Private backing field for Fixtity property
        ///// </summary>
        //private Bool6D _Fixity;

        ///// <summary>
        ///// The lateral and rotational directions in which this node is
        ///// fixed for the purpose of structural and physics-based analysis.
        ///// Represented by a set of six booleans, one each for the X, Y, Z, 
        ///// XX,YY and ZZ degrees of freedom.  If true, the node is fixed in
        ///// that direction, if false it is free to move.
        ///// </summary>
        //public Bool6D Fixity
        //{
        //    get { return _Fixity; }
        //    set { _Fixity = value;  NotifyPropertyChanged("Fixity"); }
        //}

        /// <summary>
        /// Get a description of this node.
        /// Will be the node's name if it has one or will return "Node {ID}"
        /// if not.
        /// </summary>
        public override string Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name) && NumericID > 0) return "Node " + NumericID;
                else return Name;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// For use in factory methods only.
        /// </summary>
        internal Node()
        {

        }

        /// <summary>
        /// Position constructor.
        /// Initialises a new node at the specified position.
        /// </summary>
        /// <param name="position"></param>
        public Node(Vector position)
        {
            _Position = position;
        }

        /// <summary>
        /// X, Y, Z position constructor.
        /// Initialises a new node at the specified position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Node(double x, double y, double z)
        {
            _Position = new Vector(x, y, z);
        }

        #endregion

        #region Methods

        protected override NodeDataStore NewDataStore()
        {
            return new NodeDataStore(this);
        }

        /// <summary>
        /// Get a collection of all elements connected to this node
        /// </summary>
        /// <returns></returns>
        public ElementCollection GetConnectedElements()
        {
            var result = new ElementCollection();

            foreach (Vertex v in Vertices)
            {
                if (v.Element != null) result.Add(v.Element);
            }

            return result;
        }

        /// <summary>
        /// Change the position of this node, optionally dragging any
        /// attached vertices through the same transformation.
        /// </summary>
        /// <param name="newPosition"></param>
        /// <param name="dragVertices"></param>
        public void MoveTo(Vector newPosition, bool dragVertices = true, ElementCollection excludeElements = null)
        {
            Vector move = newPosition - Position;
            if (dragVertices)
            {
                foreach (Vertex v in Vertices)
                {
                    if (excludeElements == null || v.Element == null || !excludeElements.Contains(v.Element))
                        v.Position += move;
                }
            }
        }

        #endregion

    }
}

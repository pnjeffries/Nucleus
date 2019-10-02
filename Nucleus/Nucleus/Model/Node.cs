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
using Nucleus.Geometry;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
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
            set { _Position = value; NotifyPropertyChanged("Position"); }
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
        public Node()
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
        /// <param name="undeletedOnly">If true, only elements that are not marked
        /// as deleted will be returned</param>
        /// <returns></returns>
        public ElementCollection GetConnectedElements(bool undeletedOnly = true)
        {
            var result = new ElementCollection();

            GetConnectedElements(result, undeletedOnly);

            return result;
        }

        /// <summary>
        /// Get a collection of all elements connected to this node
        /// </summary>
        /// <param name="addTo">Add the elements to this collection, which will be returned as the result</param>
        /// <param name="undeletedOnly">If true, only elements that are not marked
        /// as deleted will be returned</param>
        /// <returns></returns>
        public ElementCollection GetConnectedElements(ElementCollection addTo, bool undeletedOnly = true, Element ignore = null)
        {
            foreach (Vertex v in Vertices)
            {
                if (v.Element != null && v.Element != ignore && !addTo.Contains(v.Element.GUID) &&
                    (!undeletedOnly || !v.Element.IsDeleted))
                    addTo.Add(v.Element);
            }

            return addTo;
        }

        /// <summary>
        /// Get a collection of all elements connected to this node
        /// </summary>
        /// <param name="addTo">Add the elements to this collection, which will be returned as the result</param>
        /// <param name="undeletedOnly">If true, only elements that are not marked
        /// as deleted will be returned</param>
        /// <returns></returns>
        public TElementCollection GetConnectedElements<TElement, TElementCollection>(TElementCollection addTo, bool undeletedOnly = true, TElement ignore = null)
            where TElement : Element
            where TElementCollection : UniquesCollection<TElement>, new()
        {
            foreach (Vertex v in Vertices)
            {
                if (v.Element != null && v.Element is TElement && v.Element != ignore && !addTo.Contains(v.Element.GUID) &&
                    (!undeletedOnly || !v.Element.IsDeleted))
                    addTo.Add((TElement)v.Element);
            }

            return addTo;
        }

        /// <summary>
        /// Get the first encountered element of the specified type connected to this node
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="undeletedOnly"></param>
        /// <param name="ignore"></param>
        /// <returns></returns>
        public TElement GetFirstConnectedElement<TElement>(bool undeletedOnly = true, Element ignore = null)
            where TElement : Element
        {
            foreach (Vertex v in Vertices)
            {
                if (v.Element != null && v.Element is TElement && v.Element != ignore &&
                    (!undeletedOnly || !v.Element.IsDeleted))
                    return (TElement)v.Element;
            }
            return null;
        }

        /// <summary>
        /// Get the first encountered element approached from the specified side of the specified direction
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="direction"></param>
        /// <param name="side"></param>
        /// <param name="undeletedOnly"></param>
        /// <param name="ignore"></param>
        /// <returns></returns>
        public TElement GetConnectedElementOnSide<TElement>(Vector direction, HandSide side,
            bool undeletedOnly = true, Element ignore = null)
            where TElement : LinearElement
        {
            return GetConnectedElementOnSide<TElement>(direction, side, undeletedOnly, ignore, out Angle bestAngle);
        }

        /// <summary>
        /// Get the first encountered element approached from the specified side of the specified direction
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="direction"></param>
        /// <param name="side"></param>
        /// <param name="undeletedOnly"></param>
        /// <param name="ignore"></param>
        /// <param name="bestAngle">Output.  The angle of the returned element relative to the specified
        /// direction.</param>
        /// <returns></returns>
        public TElement GetConnectedElementOnSide<TElement>(Vector direction, HandSide side,
        bool undeletedOnly, Element ignore, out Angle bestAngle)
        where TElement : LinearElement
        {
            bestAngle = 0;
            TElement best = null;
            foreach (Vertex v in Vertices)
            {
                if (v.Element != null && v.Element is TElement && v.Element != ignore &&
                    (!undeletedOnly || !v.Element.IsDeleted))
                {
                    TElement element = (TElement)v.Element;
                    Vertex v2 = element.Geometry.GetOtherEnd(v);
                    Angle angle = (v.Position.AngleTo(v2.Position) - direction.Angle).Normalize();
                    if (best == null ||
                        (side == HandSide.Left && angle > bestAngle) ||
                        (side == HandSide.Right && angle < bestAngle))
                    {
                        bestAngle = angle;
                        best = element;
                    }
                }
            }
            return best;
        }

        /// <summary>
        /// Get a collection of the geometric objects whose vertices are connected to this node
        /// </summary>
        /// <returns></returns>
        public VertexGeometryCollection GetConnectedGeometry()
        {
            var result = new VertexGeometryCollection();
            foreach (Vertex v in Vertices)
            {
                if (v.Owner != null)
                    result.Add(v.Owner);
            }

            return result;
        }

        /// <summary>
        /// Get the number of elements connected to this node
        /// </summary>
        /// <param name="undeletedOnly">If true, only elements not marked as deleted will
        /// be counted</param>
        /// <returns></returns>
        public int ConnectionCount(bool undeletedOnly = true)
        {
            int result = 0;

            foreach (Vertex v in Vertices)
            {
                if (v.Element != null && !v.Element.IsDeleted) result++;
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
            Position = newPosition;
            if (dragVertices)
            {
                foreach (Vertex v in Vertices)
                {
                    if (excludeElements == null || v.Element == null || !excludeElements.Contains(v.Element))
                        v.Position += move;
                }
            }
        }

        /// <summary>
        /// Calculate the average of the vectors from this node position to the centroid of connected elements.
        /// </summary>
        /// <returns></returns>
        public Vector AverageConnectionDirection()
        {
            Vector result = Vector.Zero;
            int count = 0;
            foreach (Vertex v in Vertices)
            {
                if (v.Owner != null)
                {
                    VertexGeometry vG = v.Owner;
                    if (vG is Curve)
                    {
                        Curve crv = (Curve)vG;
                        Vector midPt = crv.PointAt(0.5);
                        if (midPt.IsValid())
                        {
                            result += (midPt - Position).Unitize();
                            count += 1;
                        }
                    }
                    //TODO: To surface centroid?
                }
            }
            if (count > 1) result /= count;
            return result;
        }

        /// <summary>
        /// Merge the properties of another node with this one.
        /// </summary>
        /// <param name="other">The node to merge into this one</param>
        /// <param name="averagePositions">If true, the node position will
        /// be set to the average of the original value and the position of
        /// the other.</param>
        public void Merge(Node other, bool averagePositions = false)
        {
            if (HasData() || other.HasData())
            {
                Data.Merge(other.Data);
            }
            if (other.Vertices != null)
            {
                // Replace vertex node references
                for (int i = other.Vertices.Count - 1; i >= 0; i--)
                {
                    Vertex v = other.Vertices[i];
                    if (v.Node == other) v.Node = this;
                }
            }
            if (averagePositions)
            {
                Position = (Position + other.Position) / 2;
            }
        }

        #endregion

    }

    /// <summary>
    /// Static extension methods for collections of nodes
    /// </summary>
    public static class NodeExtensions
    {
        /// <summary>
        /// Merge a collection of nodes into one node.
        /// The lowest-numbered node will be retained, the others
        /// will have their data merged into that one and be deleted.
        /// </summary>
        /// <param name="nodes">The collection of nodes to merge.  Note that any
        /// deleted nodes which you do not want to include in this merge should be
        /// removed prior to running this operation.</param>
        /// <param name="averagePositions">If true, the resultant node position will
        /// be set to the average of the node positions.  Otherwise, the position of the
        /// original node will be retained.</param>
        /// <returns></returns>
        public static Node Merge(this IList<Node> nodes, bool averagePositions = false)
        {
            int i = nodes.IndexOfLowestNumericID();
            if (i < 0) return null;
            Node result = nodes[i];
            Vector average = new Vector();
            for (int j = 0; j < nodes.Count; j++)
            {
                Node node = nodes[j];
                average += node.Position;
                if (j != i)
                {
                    result.Merge(node);
                    node.Delete();
                }
            }
            if (averagePositions)
            {
                average /= nodes.Count;
                result.Position = average;
            }
            return result;
        }

        /// <summary>
        /// Find all the nodes in this list that have lower than or equal to the specified
        /// number of connected elements.  Can be used to identify isolated or 'dead-end' nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static NodeCollection WithMaxConnectedElements(this IList<Node> nodes, int maxCount)
        {
            var result = new NodeCollection();
            foreach (var node in nodes)
            {
                int elCount = 0;
                foreach (var v in node.Vertices)
                {
                    if (v.Element != null && !v.Element.IsDeleted)
                    {
                        elCount++;
                    }
                }
                if (elCount <= maxCount) result.Add(node);
            }
            return result;
        }
    }
}

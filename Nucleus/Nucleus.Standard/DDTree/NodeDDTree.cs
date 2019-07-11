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

using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    /// <summary>
    /// A DDTree used for the spatial partitioning of nodes
    /// </summary>
    [Serializable]
    public class NodeDDTree : DDTree<Node>
    {
        /// <summary>
        /// Creates a new DDTree populated with the specified collection of objects
        /// </summary>
        /// <param name="nodes">The nodes to include within the tree.</param>
        /// <param name="maxDivisions">The maximum number of cells into which each 
        /// level in the tree should be divided</param>
        /// <param name="minCellSize">The minimum allowable size of a cell.  Once a node
        /// reaches this size it will no longer subdivide regardless of how many items are
        /// contained within it.</param>
        /// <param name="maxLeafPopulation">The maximum population per leaf node.  If the
        /// number of objects within a cell exceeds this number and the minimum cell size 
        /// has not yet been reached, the node will subdivide</param>
        public NodeDDTree(NodeCollection nodes, int maxDivisions = 10, 
            double minCellSize = 1, int maxLeafPopulation = 4) 
            : base(nodes, maxDivisions, minCellSize) { }

        /// <summary>
        /// Find the minimum bounding X-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double MinXOf(Node entry)
        {
            return entry.Position.X;
        }

        /// <summary>
        /// Find the maximum bounding X-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double MaxXOf(Node entry)
        {
            return entry.Position.X;
        }

        /// <summary>
        /// Find the minimum bounding Y-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double MinYOf(Node entry)
        {
            return entry.Position.Y;
        }

        /// <summary>
        /// Find the maximum bounding Y-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double MaxYOf(Node entry)
        {
            return entry.Position.Y;
        }

        /// <summary>
        /// Find the minimum bounding Z-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double MinZOf(Node entry)
        {
            return entry.Position.Z;
        }

        /// <summary>
        /// Find the maximum bounding Z-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double MaxZOf(Node entry)
        {
            return entry.Position.Z;
        }

        /// <summary>
        /// Returns the minumum squared distance between the specified position in 3D-space and the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double DistanceSquaredBetween(Vector pt, Node entry)
        {
            return pt.DistanceToSquared(entry.Position);
        }

        /*
        /// <summary>
        /// Returns the minimum squared distance between two entries in the tree.
        /// Should be overridden to deal with the specific tree type
        /// </summary>
        /// <param name="entryA"></param>
        /// <param name="entryB"></param>
        /// <returns></returns>
        public override double MinDistanceSquaredBetween(Node entryA, Node entryB)
        {
            return entryA.DistanceToSquared(entryB);
        }
        */

        /// <summary>
        /// Get the nominal position of the specified entry in the tree the specified dimensional axis.
        /// Should be overridden to deal with the specific tree type
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override double PositionInDimension(CoordinateAxis dimension, Node entry)
        {
            switch (dimension)
            {
                case CoordinateAxis.X:
                    return entry.Position.X;
                case CoordinateAxis.Y:
                    return entry.Position.Y;
                default:
                    return entry.Position.Z;
            }
        }

        public IList<Node> CloseTo(Vector pt, double maxDistance)
        {
            IList<Node> nodes = new NodeCollection();
            CloseTo(pt, maxDistance, ref nodes);
            return nodes;
        }

        /// <summary>
        /// Find all nodes coincident to the specified set of nodes within this tree,
        /// within the specified tolerance.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public IList<IList<Node>> CoincidentNodes(NodeCollection nodes, double tolerance)
        {
            IList<IList<Node>> result = new List<IList<Node>>();
            NodeCollection remainingNodes = new NodeCollection();
            foreach (Node node in nodes) remainingNodes.Add(node);
            while (remainingNodes.Count > 0)
            {
                Node node = remainingNodes.Last();
                IList<Node> close = CloseTo(node.Position, tolerance);
                if (!close.Contains(node)) close.Add(node);
                result.Add(close);
                foreach (Node foundNode in close)
                {
                    remainingNodes.Remove(foundNode);
                }
            }
            return result;
        }

        public override bool CanReturn(Node item)
        {
            // Filter out deleted nodes:
            return !item.IsDeleted;
        }

    }
}

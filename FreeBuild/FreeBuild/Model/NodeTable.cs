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

using FreeBuild.DDTree;
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// An extended NodeCollection that also maintains several other temporary
    /// data structures in order to provide additional fast lookup capabilities
    /// </summary>
    [Serializable]
    public class NodeTable : NodeCollection
    {

        #region Properties

        /// <summary>
        /// Private backing field for NextNumericID property
        /// </summary>
        private long _NextNumericID = 1;

        /// <summary>
        /// The numeric ID that will be assigned to the next element to be added to this table
        /// </summary>
        public long NextNumericID
        {
            get { return _NextNumericID; }
        }

        /// <summary>
        /// Private backing member variable for the SpatialTree property
        /// </summary>
        [NonSerialized]
        private NodeDDTree _SpatialTree;

        /// <summary>
        /// The spatial divided-dimension tree of this node collection.
        /// Speeds up spatial-based searches of node position.
        /// Generated automatically when required.
        /// </summary>
        public NodeDDTree SpatialTree
        {
            get
            {
                if (_SpatialTree == null) _SpatialTree = new NodeDDTree(this);
                return _SpatialTree;
            }
            set
            {
                _SpatialTree = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialises a new NodeTable belonging to the specified model
        /// </summary>
        /// <param name="model"></param>
        public NodeTable(Model model) : base(model) { }

        #endregion

        #region Methods

        /// <summary>
        /// Find the closest node in this collection to the
        /// specified point.  
        /// </summary>
        /// <param name="point">The search point</param>
        /// <param name="maxDistance">The maximum distance within which to look for nodes</param>
        /// <param name="toBeIgnored">Optional.  A node within this collection that should be
        /// excluded from the search.  Use when trying to find another node close to the 
        /// specified one.</param>
        /// <returns></returns>
        public Node ClosestNodeTo(Vector point, double maxDistance, Node toBeIgnored = null)
        {
            return SpatialTree.NearestTo(point, maxDistance, toBeIgnored);
        }

        protected override void OnCollectionChanged()
        {
            //Clear cached data:
            //_SpatialTree = null;
        }

        protected override void InsertItem(int index, Node item)
        {
            base.InsertItem(index, item);
            if (_SpatialTree != null) _SpatialTree.Add(item);
        }

        protected override void SetItem(int index, Node item)
        {
            base.SetItem(index, item);
            _SpatialTree = null;
        }

        protected override void SetNumericID(Node item)
        {
            item.NumericID = NextNumericID;
            _NextNumericID++;
        }

        #endregion
    }
}

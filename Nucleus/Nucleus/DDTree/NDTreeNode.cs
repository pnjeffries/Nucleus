using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    /// <summary>
    /// A node in an NDTree
    /// </summary>
    public class NDTreeNode<T>
    {
        #region Properties

        /// <summary>
        /// The left-hand branch
        /// </summary>
        private NDTreeNode<T> _Left = null;

        /// <summary>
        /// The right-hand branch
        /// </summary>
        private NDTreeNode<T> _Right = null;

        private IList<T> _Children;
        /// <summary>
        /// The child objects of this node
        /// </summary>
        public IList<T> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        private DDTree<T> _Tree; //The tree this node belongs to

        /// <summary>
        /// The dimensional axis along which the space in this node is divided.
        /// If this is Undefined, the node is not split.
        /// </summary>
        private CoordinateAxis _SplitDimension = CoordinateAxis.Undefined;

        /// <summary>
        /// Is this node a leaf node (i.e. is it not divided)
        /// </summary>
        public bool IsLeafNode
        {
            get { return _SplitDimension == CoordinateAxis.Undefined; }
        }

        #endregion
    }
}

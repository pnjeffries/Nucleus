using FreeBuild.Base;
using FreeBuild.Geometry;
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
    public class Node : Unique
    {
        #region Properties

        /// <summary>
        /// Internal backing member for Position property
        /// </summary>
        private Vector _Position = Vector.Unset;

        /// <summary>
        /// The spatial position of this node
        /// </summary>
        public Vector Position
        {
            get { return _Position; }
            set { _Position = value;  NotifyPropertyChanged("Position"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Position constructor
        /// </summary>
        /// <param name="position"></param>
        public Node(Vector position)
        {
            _Position = position;
        }

        #endregion

    }
}

using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Support conditions that can be attached to a node to represent
    /// restraint in a finite element analysis or physical simulation
    /// </summary>
    [Serializable]
    public sealed class NodeSupport : Unique, INodeDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Fixity property
        /// </summary>
        private Bool6D _Fixity;

        /// <summary>
        /// The lateral and rotational directions in which this node is
        /// fixed for the purpose of structural and physics-based analysis.
        /// Represented by a set of six booleans, one each for the X, Y, Z, 
        /// XX,YY and ZZ degrees of freedom.  If true, the node is restrained in
        /// that direction, if false it is free to move.
        /// </summary>
        public Bool6D Fixity
        {
            get { return _Fixity; }
            set { _Fixity = value; NotifyPropertyChanged("Fixity"); }
        }

        /// <summary>
        /// Private backing field for Axes property
        /// </summary>
        private CoordinateSystemReference _Axes = CoordinateSystemReference.Global;

        /// <summary>
        /// The coordinate axis system to which the fixity directions refer
        /// </summary>
        public CoordinateSystemReference Axes
        {
            get { return _Axes; }
            set { ChangeProperty(ref _Axes, value, "Axes"); }
        }

        #endregion

        /// <summary>
        /// Initialise a new node support with no data
        /// </summary>
        public NodeSupport() : base()
        {
        }

        /// <summary>
        /// Initialise a new node support with the fixed dimensions specified
        /// </summary>
        /// <param name="fixity"></param>
        public NodeSupport(Bool6D fixity) : base()
        {
            Fixity = fixity;
        }

        /// <summary>
        /// Initialise a new node support with the specified fixed dimensions in the specified
        /// coordinate system.
        /// </summary>
        /// <param name="fixity"></param>
        /// <param name="axes"></param>
        public NodeSupport(Bool6D fixity, CoordinateSystemReference axes) : this(fixity)
        {
            Axes = axes;
        }

        public void Merge(INodeDataComponent other)
        {
            if (other is NodeSupport)
            {
                NodeSupport otherS = (NodeSupport)other;
                Fixity = Fixity.Or(otherS.Fixity);
                //TODO: Axis merging
            }
        }
    }
}

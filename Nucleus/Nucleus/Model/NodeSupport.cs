using Nucleus.Base;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A data component that represents support conditions that can be attached to a node to represent
    /// restraint in a finite element analysis or physical simulation
    /// </summary>
    [Serializable]
    [Copy(CopyBehaviour.DUPLICATE)]
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

        /// <summary>
        /// Private backing field for Stiffness property
        /// </summary>
        private SixVector _Stiffness = new SixVector();

        /// <summary>
        /// The stiffnesses in the translational and rotational degrees of freedom
        /// of this support.  By default this stiffness is 0 in all directions.
        /// This property is only considered when the fixity in the relevent direction
        /// is set to false - otherwise the node is taken to be fully restrained in that
        /// axis and the stiffness is effectively infinite.
        /// Expressed in N/m.
        /// </summary>
        public SixVector Stiffness
        {
            get { return _Stiffness; }
            set { ChangeProperty(ref _Stiffness, value, "Stiffness"); }
        }

        #endregion

        #region Constructors

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
            _Fixity = fixity;
        }

        /// <summary>
        /// Initialise a new node support with the fixed dimensions specified
        /// and the given stiffnesses in the other directions
        /// </summary>
        /// <param name="fixity"></param>
        public NodeSupport(Bool6D fixity, SixVector stiffness) : this(fixity)
        {
            _Stiffness = stiffness;
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

        #endregion

        #region Methods

        public void Merge(INodeDataComponent other)
        {
            if (other is NodeSupport)
            {
                NodeSupport otherS = (NodeSupport)other;
                Fixity = Fixity.Or(otherS.Fixity);
                Stiffness = Stiffness.MergeMax(otherS.Stiffness);
                //TODO: Axis merging
            }
        }

        #endregion
    }
}

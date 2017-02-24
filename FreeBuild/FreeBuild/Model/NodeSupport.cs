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
    public class NodeSupport : Unique, INodeDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Fixtity property
        /// </summary>
        private Bool6D _Fixity;

        /// <summary>
        /// The lateral and rotational directions in which this node is
        /// fixed for the purpose of structural and physics-based analysis.
        /// Represented by a set of six booleans, one each for the X, Y, Z, 
        /// XX,YY and ZZ degrees of freedom.  If true, the node is fixed in
        /// that direction, if false it is free to move.
        /// </summary>
        public Bool6D Fixity
        {
            get { return _Fixity; }
            set { _Fixity = value; NotifyPropertyChanged("Fixity"); }
        }

        #endregion

        /// <summary>
        /// Initialise a new node support with no data
        /// </summary>
        public NodeSupport() : base()
        {
        }

        /// <summary>
        /// Initialise a new node support with the given fixed dimensions
        /// </summary>
        /// <param name="fixity"></param>
        public NodeSupport(Bool6D fixity) : base()
        {
            Fixity = fixity;
        }
    }
}

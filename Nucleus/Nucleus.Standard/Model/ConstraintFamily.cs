using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A family which describes a rigid constraint between nodes
    /// in a structural or physical model
    /// </summary>
    [Serializable]
    public class ConstraintFamily : Family
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


        public override Material GetPrimaryMaterial()
        {
            return null;
        }
    }
}

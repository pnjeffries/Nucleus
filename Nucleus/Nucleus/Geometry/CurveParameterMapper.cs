using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Class which can map from a parameter value on a parent curve to
    /// an equivalent parameter on a child curve
    /// </summary>
    [Serializable]
    public class CurveParameterMapper
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the CurveA property
        /// </summary>
        private Curve _CurveA;

        /// <summary>
        /// The first curve
        /// </summary>
        public Curve CurveA
        {
            get { return _CurveA; }
            set { _CurveA = value; }
        }

        /// <summary>
        /// Private backing member variable for the CurveB property
        /// </summary>
        private Curve _CurveB;

        /// <summary>
        /// The second curve
        /// </summary>
        public Curve CurveB
        {
            get { return _CurveB; }
            set { _CurveB = value; }
        }

        #endregion

        #region Constructors

        #endregion
    }
}

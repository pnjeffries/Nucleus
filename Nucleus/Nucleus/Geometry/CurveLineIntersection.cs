using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A record of an intersection occurrance between a
    /// curve and a straight line
    /// </summary>
    [Serializable]
    public class CurveLineIntersection
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Curve property
        /// </summary>
        private Curve _Curve;

        /// <summary>
        /// The intersecting curve
        /// </summary>
        public Curve Curve
        {
            get { return _Curve; }
        }

        /// <summary>
        /// Private backing member variable for the CurveParameter property
        /// </summary>
        private double _CurveParameter;

        /// <summary>
        /// The parameter along the curve at which the intersection takes place
        /// </summary>
        public double CurveParameter
        {
            get { return _CurveParameter; }
        }

        /// <summary>
        /// Private backing member variable for the LineParameter property
        /// </summary>
        private double _LineParameter;

        /// <summary>
        /// The parameter along the line at which the intersection takes place
        /// </summary>
        public double LineParameter
        {
            get { return _LineParameter; }
        }

        /// <summary>
        /// Private backing field for the Side property
        /// </summary>
        private HandSide _Side = HandSide.Undefined;

        /// <summary>
        /// Tag to determine which side of a cut the intersection lies
        /// on.  Used during certain advanced operations.
        /// </summary>
        public HandSide Side
        {
            get { return _Side; }
            set { _Side = value; }
        }

        /// <summary>
        /// Private backing member variable for the ProcessCounter property
        /// </summary>
        private int _ProcessCounter = 0;

        /// <summary>
        /// A counter tag used during certain advanced operations to store the number of processing operations performed using this intersection.
        /// </summary>
        public int ProcessCounter
        {
            get { return _ProcessCounter; }
            set { _ProcessCounter = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Curve-Line Intersection record
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="tCurve"></param>
        /// <param name="tLine"></param>
        public CurveLineIntersection(Curve curve, double tCurve, double tLine)
        {
            _Curve = curve;
            _CurveParameter = tCurve;
            _LineParameter = tLine;
        }

        #endregion
    }
}

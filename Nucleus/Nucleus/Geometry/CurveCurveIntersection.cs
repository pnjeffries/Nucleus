using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A record of an intersection occurance between two curves
    /// </summary>
    [Serializable]
    public class CurveCurveIntersection
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the CurveA property
        /// </summary>
        private Curve _CurveA;

        /// <summary>
        /// The first intersecting curve
        /// </summary>
        public Curve CurveA
        {
            get { return _CurveA; }
        }

        /// <summary>
        /// Private backing member variable for the CurveA property
        /// </summary>
        private Curve _CurveB;

        /// <summary>
        /// The first intersecting curve
        /// </summary>
        public Curve CurveB
        {
            get { return _CurveB; }
        }

        /// <summary>
        /// Private backing member variable for the ParameterA property
        /// </summary>
        private double _ParameterA;

        /// <summary>
        /// The intersection parameter on the first curve
        /// </summary>
        public double ParameterA
        {
            get { return _ParameterA; }
        }

        /// <summary>
        /// Private backing member variable for the ParameterB property
        /// </summary>
        private double _ParameterB;

        /// <summary>
        /// The intersection parameter on the second curve
        /// </summary>
        public double ParameterB
        {
            get { return _ParameterB; }

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
        /// Creates a new Curve-Curve Intersection
        /// </summary>
        /// <param name="curveA"></param>
        /// <param name="curveB"></param>
        /// <param name="parameterA"></param>
        /// <param name="parameterB"></param>
        public CurveCurveIntersection(Curve curveA, Curve curveB, double parameterA, double parameterB, int processCounter = 0)
        {
            _CurveA = curveA;
            _CurveB = curveB;
            _ParameterA = parameterA;
            _ParameterB = parameterB;
            _ProcessCounter = processCounter;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the intersection parameter on the specified curve
        /// </summary>
        /// <param name="curve">One of CurveA or CurveB</param>
        /// <returns></returns>
        public double ParameterOn(Curve curve)
        {
            if (curve == CurveA) return ParameterA;
            else return ParameterB;
        }

        /// <summary>
        /// Return the alternate curve involved in this
        /// itersection from the one specified.
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public Curve OtherCurve(Curve curve)
        {
            if (curve == CurveA) return CurveB;
            else return CurveA;
        }

        #endregion
    }
}

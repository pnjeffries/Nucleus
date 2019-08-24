using Nucleus.Maths;
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

        /// <summary>
        /// Private backing member variable for the SpanDomains property
        /// </summary>
        private IList<Interval> _SpanDomains = new List<Interval>();

        /// <summary>
        /// The list of subdomains on CurveA that Correspond to each span of CurveB in order
        /// </summary>
        public IList<Interval> SpanDomains
        {
            get { return _SpanDomains; }
            set { _SpanDomains = value; }
        }

        #endregion

        #region Constructors

        public CurveParameterMapper()
        {

        }

        public CurveParameterMapper(Curve curveA, Curve curveB)
        {
            _CurveA = curveA;
            _CurveB = curveB;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Map from a parameter on curve B to an equivalent
        /// parameter on curve A.
        /// </summary>
        /// <param name="tB"></param>
        /// <returns></returns>
        public double MapBtoA(double tB)
        {
            if (_SpanDomains == null) return double.NaN;

            int iSpan = CurveB.SpanAt(tB, out double tSpan);

            if (iSpan < 0 || iSpan >= _SpanDomains.Count) return double.NaN;

            Interval spanDomain = SpanDomains[iSpan];

            if (!spanDomain.IsValid) return double.NaN;

            return spanDomain.ValueAt(tSpan);
        }

        /// <summary>
        /// Map from a parameter on curve A to an equivalent parameter
        /// on curve B.
        /// </summary>
        /// <param name="tA"></param>
        /// <returns></returns>
        public double MapAtoB(double tA)
        {
            if (_SpanDomains == null) return double.NaN;

            for (int iSpan = 0; iSpan < _SpanDomains.Count; iSpan++)
            {
                Interval spanDomain = _SpanDomains[iSpan];
                if (spanDomain.Contains(tA))
                {
                    double tSpan = spanDomain.ParameterOf(tA);
                    return CurveB.ParameterAt(iSpan, tSpan);
                }
            }
            return double.NaN;
        }

        #endregion
    }
}

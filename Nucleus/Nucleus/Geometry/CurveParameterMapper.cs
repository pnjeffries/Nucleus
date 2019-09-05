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
        /// Add a new interval to the SpanDomains collection of this mapper
        /// </summary>
        /// <param name="spanDomain"></param>
        public void AddSpanDomain(Interval spanDomain)
        {
            SpanDomains.Add(spanDomain);
        }

        /// <summary>
        /// Add new intervals to the SpanDomains collection of this mapper
        /// </summary>
        /// <param name="spanDomains"></param>
        public void AddSpanDomains(IList<Interval> spanDomains)
        {
            foreach (var sd in spanDomains) SpanDomains.Add(sd);
        }

        /// <summary>
        /// Add an interval to the SpanDomains collection
        /// of this mapper which
        /// uses an 'unset' value rather than the original value.
        /// </summary>
        /// <param name="spanDomains"></param>
        public void AddNullSpanDomain()
        {
            SpanDomains.Add(Interval.Unset);
        }

        /// <summary>
        /// Add an equivalent number of intervals to the SpanDomains collection
        /// of this mapper as the specifed collection, but where each interval
        /// uses an 'unset' value rather than the original value.
        /// </summary>
        /// <param name="spanDomains"></param>
        public void AddNullSpanDomains(IList<Interval> spanDomains)
        {
            foreach (var sd in spanDomains) SpanDomains.Add(Interval.Unset);
        }

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

        /// <summary>
        /// Map from a parameter interval on curve A to an equivalent set of
        /// parameter subdomains on curve B.
        /// </summary>
        /// <param name="tA"></param>
        /// <returns></returns>
        public IList<Interval> MapAtoB(Interval tA)
        {
            var result = new List<Interval>();

            MapAtoB(tA, result);

            return result;
        }

        /// <summary>
        /// Map from a set of parameter intervals on curve A to an 
        /// equivalent set of parameter subdomains on curve B.
        /// </summary>
        /// <param name="tAs"></param>
        /// <returns></returns>
        public IList<Interval> MapAtoB(IList<Interval> tAs)
        {
            var result = new List<Interval>();

            foreach (var tA in tAs)
            {
                MapAtoB(tA, result);
            }

            return result;
        }

        /// <summary>
        /// Populate the result collection with equivalent subdomains on curve B
        /// </summary>
        /// <param name="tA"></param>
        /// <param name="result"></param>
        private void MapAtoB(Interval tA, IList<Interval> result)
        {
            if (_SpanDomains == null) return;

            for (int iSpan = 0; iSpan < _SpanDomains.Count; iSpan++)
            {
                Interval spanDomain = _SpanDomains[iSpan];
                if (spanDomain.Overlaps(tA))
                {
                    var overlap = spanDomain.Overlap(tA);
                    double tAStart = spanDomain.ParameterOf(overlap.Start);
                    double tBStart = CurveB.ParameterAt(iSpan, tAStart);
                    double tAEnd = spanDomain.ParameterOf(overlap.End);
                    double tBEnd = CurveB.ParameterAt(iSpan, tAEnd);
                    if (result.Count > 0 &&
                        result.Last().End == tBStart)
                    {
                        result[result.Count - 1] = result.Last().WithEnd(tBEnd);
                    }
                    else result.Add(new Interval(tBStart, tBEnd));
                }
            }
        }

        /// <summary>
        /// Get the subdomain regions of curve B which have no stored relationship
        /// to any part of curve A - typically the new parts of the curve that were
        /// created by whatever operation generated the mapper.
        /// </summary>
        /// <returns></returns>
        public IList<Interval> UnrelatedPartsOfB()
        {
            var result = new List<Interval>();

            if (_SpanDomains == null) return result;

            for (int iSpan = 0; iSpan < _SpanDomains.Count; iSpan++)
            {
                if (!_SpanDomains[iSpan].IsValid)
                {
                    double tBStart = CurveB.ParameterAt(iSpan, 0);
                    double tBEnd = CurveB.ParameterAt(iSpan, 1);

                    if (result.Count > 0 &&
                        result.Last().End == tBStart)
                    {
                        result[result.Count - 1] = result.Last().WithEnd(tBEnd);
                    }
                    else result.Add(new Interval(tBStart, tBEnd));
                }
            }

            return result;
        }

        #endregion
    }
}

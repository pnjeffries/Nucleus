using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Class for performing basic structural engineering calculations on 
    /// simply-supported beams under uniformly distributed loading
    /// </summary>
    [Serializable]
    public class SimpleBeamAnalysis : BeamAnalysisBase
    {
        #region Constructors

        /// <summary>
        /// Initialise a beam calculation for the specified length and load
        /// </summary>
        /// <param name="length"></param>
        /// <param name="udl"></param>
        public SimpleBeamAnalysis(double length, double udl)
        {
            Span = length;
            UDL = udl;
        }

        /// <summary>
        /// Initialise a beam calculation for the specified beam variables
        /// </summary>
        /// <param name="length"></param>
        /// <param name="udl"></param>
        /// <param name="e"></param>
        /// <param name="i"></param>
        public SimpleBeamAnalysis(double length, double udl, double e, double i)
            : this(length, udl)
        {
            E = e;
            I = i;
        }

        /// <summary>
        /// Initialise a beam calculation for the specified element under the specified load
        /// </summary>
        /// <param name="element"></param>
        /// <param name="udl"></param>
        public SimpleBeamAnalysis(LinearElement element, double udl = 0) : base(element, udl)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculate the maximum bending moment (about the major axis).
        /// Requires UDL and Length.
        /// </summary>
        /// <returns></returns>
        public double MaxMoment(ICalculationLog log = null)
        {
            // M = wl²/8
            log?.Symbol("Moment").Write(" = ").Symbol("UDL").Symbol("Span").Superscript("2").WriteLine("/8");
            return UDL * Span.Squared() / 8;
            
        }

        /// <summary>
        /// Calculate the bending moment at a position along the beam.
        /// Requires UDL and Length.
        /// </summary>
        /// <param name="x">The position along the beam, as a distance from
        /// the start (in m).</param>
        /// <returns></returns>
        public double MomentAt(double x, ICalculationLog log = null)
        {
            // Mx = (wx/2)(l - x)
            log?.Write("(").Symbol("UDL").Symbol("x").Write("/2)(").Symbol("Span").Write("-").Symbol("x").WriteLine(")");
            return (UDL * x / 2) * (Span - x);
        }

        /// <summary>
        /// Calculate the maximum shear force in the beam.
        /// Requires UDL and Length.
        /// </summary>
        /// <returns></returns>
        public double MaxShear()
        {
            //V = wl/2
            return UDL * Span / 2;
        }

        /// <summary>
        /// Calculate the shear force in the beam at the specified distance
        /// along it.
        /// Requires UDL and Length.
        /// </summary>
        /// <param name="x">The position along the beam, as a distance from
        /// the start (in m).</param>
        /// <returns></returns>
        public double ShearAt(double x)
        {
            // Vx = w(l/2 - x)
            return UDL * (Span / 2 - x);
        }

        /// <summary>
        /// Calculate the maximum deflection of the beam.
        /// Requires UDL, Length, E and I.
        /// </summary>
        /// <returns></returns>
        public double MaxDeflection()
        {
            // Δmax = (5wl^4) / (384EI)
            return (5 * UDL * Span.Power(4)) / (384 * E * I);
        }

        /// <summary>
        /// Calculate the total value of all loading applied to the beam.
        /// Requires UDL and Length.
        /// </summary>
        /// <returns></returns>
        public double TotalLoad()
        {
            // W = wl
            return UDL * Span;
        }

        /// <summary>
        /// Calculate (approximately) the first 5 natural frequencies of the beam.
        /// NOT YET TESTED.
        /// </summary>
        /// <returns></returns>
        public IList<double> NaturalFrequencies()
        {
            double[] result = new double[5];
            // fn = (Kn/2π) * √(EIg/(w*l^4))
            double g = 9.8; //acceleration due to gravity (m/s)
            double fnBase = (1 / (2 * Math.PI)) * Math.Sqrt((E * I * g) / (UDL * Span.Power(4)));
            // Mode 1: Kn = 9.87
            result[0] = 9.87 * fnBase;
            // Mode 2: Kn = 39.5
            result[1] = 39.5 * fnBase;
            // Mode 3: Kn = 88.8
            result[2] = 88.8 * fnBase;
            // Mode 4: Kn = 158
            result[3] = 158 * fnBase;
            // Mode 5: Kn = 247
            result[4] = 246 * fnBase;
            return result;
        }

        #endregion
    }
}

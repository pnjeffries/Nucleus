using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// Class for performing simple structural engineering calculations on beams
    /// </summary>
    public class SimpleBeamAnalysis
    {
        #region Properties

        /// <summary>
        /// The overall length of the member (m)
        /// </summary>
        double Length { get; set; }

        /// <summary>
        /// The uniformly distributed load along the beam (N/m)
        /// </summary>
        double UDL { get; set; } = 0;

        /// <summary>
        /// The Young's Modulus of the beam in N/m²
        /// </summary>
        double E { get; set; } = double.NaN;

        /// <summary>
        /// The second moment of area of the beam in m^4
        /// </summary>
        double I { get; set; } = double.NaN;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a beam calculation for the specified length and load
        /// </summary>
        /// <param name="length"></param>
        /// <param name="udl"></param>
        public SimpleBeamAnalysis(double length, double udl)
        {
            Length = length;
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
        public SimpleBeamAnalysis(LinearElement element, double udl)
        {
            Length = element.Geometry.Length;
            UDL = udl;
            //TODO: Populate E & I from element properties
        }
        
        /// <summary>
        /// Calculate the maximum bending moment (about the major axis)
        /// </summary>
        /// <returns></returns>
        public double MaxMoment()
        {
            // M = wl²/8
            return UDL * Length.Squared() / 8;
        }

        /// <summary>
        /// Calculate the bending moment at a position along the beam
        /// </summary>
        /// <param name="x">The position along the beam, as a distance from
        /// the start (in m).</param>
        /// <returns></returns>
        public double MomentAt(double x)
        {
            // Mx = (wx/2)(l - x)
            return (UDL * x / 2) * (Length - x);
        }

        /// <summary>
        /// Calculate the maximum shear force in the beam
        /// </summary>
        /// <returns></returns>
        public double MaxShear()
        {
            //V = wl/2
            return UDL * Length / 2;
        }

        /// <summary>
        /// Calculate the shear force in the beam at the specified distance
        /// along it.
        /// </summary>
        /// <param name="x">The position along the beam, as a distance from
        /// the start (in m).</param>
        /// <returns></returns>
        public double ShearAt(double x)
        {
            // Vx = w(l/2 - x)
            return UDL * (Length / 2 - x);
        }

        /// <summary>
        /// Calculate the maximum deflection of the beam
        /// </summary>
        /// <returns></returns>
        public double MaxDeflection()
        {
            // Δmax = (5wl^4) / (384EI)
            return (5 * UDL * Length.Power(4)) / (384 * E * I);
        }

        /// <summary>
        /// Calculate the total value of all loading applied to the beam
        /// </summary>
        /// <returns></returns>
        public double TotalLoad()
        {
            // W = wl
            return UDL * Length;
        }

        #endregion
    }
}

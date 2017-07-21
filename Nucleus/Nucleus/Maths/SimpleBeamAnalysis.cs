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
        /// Initialise a beam calculation for the specified 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="udl"></param>
        public SimpleBeamAnalysis(LinearElement element, double udl)
        {
            Length = element.Geometry.Length;
            UDL = udl;
        }
        
        /// <summary>
        /// Calculate the maximum bending moment (about the major axis)
        /// </summary>
        /// <returns></returns>
        public double MaxMoment()
        {
            //TODO: Consider ends + intermediate supports
            return UDL * Length.Squared() / 8;
        }

        /// <summary>
        /// Calculate the total value of all loading applied to the beam
        /// </summary>
        /// <returns></returns>
        public double TotalLoad()
        {
            return UDL * Length;
        }

        #endregion
    }
}

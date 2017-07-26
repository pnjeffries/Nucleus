using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Abstract base class for beam analysis
    /// </summary>
    public abstract class BeamAnalysisBase
    {
        #region Properties

        /// <summary>
        /// The overall length of the member (m)
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// The uniformly distributed load along the beam (N/m)
        /// </summary>
        public double UDL { get; set; } = 0;

        /// <summary>
        /// The Young's Modulus of the beam in N/m²
        /// </summary>
        public double E { get; set; } = double.NaN;

        /// <summary>
        /// The second moment of area of the beam about the major axis in m^4
        /// </summary>
        public double I { get; set; } = double.NaN;

        #endregion
    }
}

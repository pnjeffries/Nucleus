using Nucleus.Base;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    public class FlatConcreteSlabChecks : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the CylinderStrength property
        /// </summary>
        private double _CylinderStrength = 28;

        /// <summary>
        /// The concrete cylinder strength, in MPa
        /// </summary>
        public double CylinderStrength
        {
            get { return _CylinderStrength; }
            set
            {
                _CylinderStrength = value;
                NotifyPropertyChanged("CylinderStrength");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculate value of K
        /// </summary>
        /// <param name="M">The design moments, in Nm</param>
        /// <param name="b">The bredth of section, in m</param>
        /// <param name="d">The depth of section, in m</param>
        /// <param name="fck">The cylinder strength of the concrete, in MPa</param>
        /// <returns></returns>
        public static double CalculateK(double M, double b, double d, double fck, ICalculationLog log = null)
        {
            // K = M/(bd²fck)
            log.Write("{K} = {M}/({b}{d}²{fck}");
            return M / (b * d * d * fck);
        }

        #endregion

    }
}

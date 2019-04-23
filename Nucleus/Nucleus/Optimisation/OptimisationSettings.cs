using Nucleus.Base;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Base class for settings which provide control parameters for an optimisation solver
    /// </summary>
    [Serializable]
    public class OptimisationSettings : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Objective property
        /// </summary>
        private OptimisationObjective _Objective = OptimisationObjective.Maximise;

        /// <summary>
        /// Get or set whether the optimisation should be seeking to maximise or minimise the output value of the fitness function
        /// </summary>
        [AutoUI(100)]
        public OptimisationObjective Objective
        {
            get { return _Objective; }
            set { ChangeProperty(ref _Objective, value); }
        }

        #endregion
    }
}

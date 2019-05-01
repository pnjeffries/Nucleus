using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Class to contain parameters used in simulated annealing optimisation methods
    /// </summary>
    [Serializable]
    public class SimulatedAnnealingSettings : OptimisationSettings
    {
        #region Properties

        /// <summary>
        /// Private backing field for RNG property
        /// </summary>
        private Random _RNG = new Random();

        /// <summary>
        /// The random number generator used during the solution to generate
        /// and mutate new options (where required)
        /// </summary>
        public Random RNG { get { return _RNG; } }

        /// <summary>
        /// Private backing field for MaxGenerations property
        /// </summary>
        private int _MaxIterations = 1000;

        /// <summary>
        /// The maximum number of iterations which may be performed before termination.
        /// This will control the rate of cooling of the simulation.
        /// </summary>
        [AutoUI(2000)]
        public int MaxIterations
        {
            get { return _MaxIterations; }
            set { ChangeProperty(ref _MaxIterations, value); }
        }

        /// <summary>
        /// Private backing field for MaxGenerations property
        /// </summary>
        private int _FailsBeforeReset = 10;

        /// <summary>
        /// The maximum number of iterations which may be performed without encountering 
        /// a new optimum before resetting back to the current best option.
        /// Set this to 0 to not reset the simulation at all.
        /// </summary>
        [AutoUI(2100)]
        public int FailsBeforeReset
        {
            get { return _FailsBeforeReset; }
            set { ChangeProperty(ref _FailsBeforeReset, value); }
        }

        #endregion
    }
}

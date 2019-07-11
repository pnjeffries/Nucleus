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
        /// Private backing member variable for the StartingTemperature property
        /// </summary>
        private double _StartingTemperature = 1.0;

        /// <summary>
        /// The value of the temperature at the start of the process. 
        /// </summary>
        [AutoUI(1900)]
        public double StartingTemperature
        {
            get { return _StartingTemperature; }
            set { ChangeProperty(ref _StartingTemperature, value); }
        }



        /// <summary>
        /// The maximum number of iterations which may be performed before termination.
        /// </summary>
        public int MaxIterations
        {
            get { return _CoolingCycles * _IterationsPerCoolingCycle; }
        }

        /// <summary>
        /// Private backing member variable for the CoolingCycles property
        /// </summary>
        private int _CoolingCycles = 5;

        /// <summary>
        /// The number of cooling cycles to be run through
        /// </summary>
        [AutoUI(2000)]
        public int CoolingCycles
        {
            get { return _CoolingCycles; }
            set { ChangeProperty(ref _CoolingCycles, value); }
        }


        /// <summary>
        /// Private backing member variable for the IterationsPerCoolingCycle property
        /// </summary>
        private int _IterationsPerCoolingCycle = 200;

        /// <summary>
        /// The number of iterations per cooling cycle.  The temperature will reduce to zero over this number of iterations before resetting to the initial temperature and starting again
        /// </summary>
        [AutoUI(2050)]
        public int IterationsPerCoolingCycle
        {
            get { return _IterationsPerCoolingCycle; }
            set { ChangeProperty(ref _IterationsPerCoolingCycle, value); }
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

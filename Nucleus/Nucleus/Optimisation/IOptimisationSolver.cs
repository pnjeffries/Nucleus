using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Interface for optimisation problem solvers
    /// </summary>
    public interface IOptimisationSolver<TPhenotype>
    {
        /// <summary>
        /// Get a boolean value indicating whether this solver has finished
        /// </summary>
        bool Finished { get; }

        /// <summary>
        /// Run the optimisation solver from start to finish and return the identified
        /// 'best' outcome.
        /// </summary>
        /// <returns></returns>
        TPhenotype Run();

        /// <summary>
        /// Set up the solver to begin a new optimisation run.
        /// This should be called before any iterations are performed.
        /// </summary>
        void Initialise();

        /// <summary>
        /// Perform the next iteration of the solution
        /// </summary>
        /// <returns>The new state generated and tested during this iteration</returns>
        TPhenotype Iterate();
    }
}

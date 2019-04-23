using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Abstract base class for simulated annealing solvers
    /// </summary>
    /// <typeparam name="TPhenotype"></typeparam>
    [Serializable]
    public abstract class AnnealingSolverBase<TPhenotype> 
        : OptimisationSolverBase<TPhenotype, SimulatedAnnealingSettings>
    {
        #region Constructors

        /// <summary>
        /// Initialise the solver
        /// </summary>
        public AnnealingSolverBase() : base(new SimulatedAnnealingSettings()) { }

        /// <summary>
        /// Initialise the solver
        /// </summary>
        public AnnealingSolverBase(SimulatedAnnealingSettings settings) : base(settings) { }

        #endregion

        #region Methods

        /// <summary>
        /// Run the annealing solver
        /// </summary>
        /// <returns></returns>
        public virtual TPhenotype Run()
        {
            double temperature = 1;
            double coolingRate = 1.0 / Settings.MaxIterations;

            // Generate the initial state:
            TPhenotype current = GeneratePhenotype();
            double currentFitness = OverallFitness(current);

            // Store the best state (to allow returning)
            TPhenotype best = current;
            double bestFitness = currentFitness;
            int sinceLastBest = 0;

            while (temperature > 0)
            {
                sinceLastBest++;
                TPhenotype next = GenerateNext(current, temperature);
                double nextFitness = OverallFitness(next);

                if (IsFirstBetter(nextFitness, currentFitness))
                {
                    // New state is better - move state
                    current = next;
                    currentFitness = nextFitness;
                    if (IsFirstBetter(currentFitness, bestFitness))
                    {
                        // New state is best so far - store
                        best = current;
                        bestFitness = currentFitness;
                        sinceLastBest = 0;
                    }
                }
                else if (Settings.RNG.NextDouble() < temperature)
                {
                    // New state is worse - but see where it goes anyway 
                    current = next;
                    currentFitness = nextFitness;

                    // Reset to best value if stuck
                    if (Settings.FailsBeforeReset > 0 && 
                        sinceLastBest > Settings.FailsBeforeReset)
                    {
                        // Restore best option as current
                        current = best;
                        currentFitness = bestFitness;
                        sinceLastBest = 0;
                    }
                }

                temperature -= coolingRate;
            }

            return best;
        }

        /// <summary>
        /// Choose or create a neighbouring state from the current one
        /// </summary>
        /// <param name="current">The current state</param>
        /// <param name="temperature">The current temperature</param>
        /// <returns></returns>
        public abstract TPhenotype GenerateNext(TPhenotype current, double temperature);

        #endregion
    }
}
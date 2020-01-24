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
        : OptimisationSolverBase<TPhenotype, SimulatedAnnealingSettings>,
        IOptimisationSolver<TPhenotype>
        where TPhenotype : class
    {
        #region Fields

        /// <summary>
        /// The number of iterations since the last best state was found
        /// </summary>
        private int _SinceLastBest = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Private backing member variable for the Current property
        /// </summary>
        private TPhenotype _Current = default(TPhenotype);

        /// <summary>
        /// The current state
        /// </summary>
        public TPhenotype Current
        {
            get { return _Current; }
            set { ChangeProperty(ref _Current, value); }
        }

        /// <summary>
        /// Backing member variable for the Best property
        /// </summary>
        private TPhenotype _Best;

        /// <summary>
        /// The current best state found so far
        /// </summary>
        public TPhenotype Best
        {
            get { return _Best; }
            set { ChangeProperty(ref _Best, value); }
        }

        /// <summary>
        /// Private backing member variable for the Iteration property
        /// </summary>
        private int _Iteration = 0;

        /// <summary>
        /// The number of the latest iteration performed
        /// </summary>
        public int Iteration
        {
            get { return _Iteration; }
            set { ChangeProperty(ref _Iteration, value); }
        }

        /// <summary>
        /// Get a boolean value indicating whether this solver has finished
        /// </summary>
        public bool Finished
        {
            get { return _Iteration >= Settings.MaxIterations; }
        }

        #endregion

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
            Initialise();

            while (!Finished)
            { 
                Iterate();
            }

            return Best;
        }

        /// <summary>
        /// Set up the solver to begin a new optimisation run.
        /// This should be called before any iterations are performed.
        /// </summary>
        public virtual void Initialise()
        {
            // Generate the initial state:
            if (Current == null) Current = GeneratePhenotype();
            StoreNewBest(Current);
        }

        /// <summary>
        /// Set up the solver to begin a new optimisation run.
        /// This should be called before any iterations are performed.
        /// </summary>
        /// <param name="startState">The state to be used as the starting
        /// point of the optimisation.</param>
        public virtual void Initialise(TPhenotype startState)
        {
            Current = startState;
            StoreNewBest(Current);
        }

        /// <summary>
        /// Perform the next iteration of the solution
        /// </summary>
        /// <returns>The new state generated and tested during this iteration</returns>
        public virtual TPhenotype Iterate()
        {
            // Calculate new temperature
            int cycleIts = Math.Min(Settings.IterationsPerCoolingCycle, Settings.MaxIterations);
            double temperature = Settings.StartingTemperature * ((double)(Iteration % cycleIts)) / cycleIts;

            // Advance iteration counters
            Iteration++;
            _SinceLastBest++;

            // Extract fitness function scores
            double currentFitness = OverallFitness(Current);
            double bestFitness = OverallFitness(Best);

            TPhenotype next;
            if (temperature > 0.8)
            {
                next = GeneratePhenotype();
            }
            else next = GenerateNext(Current, temperature);

            double nextFitness = OverallFitness(next);

            if (IsFirstBetter(nextFitness, currentFitness, temperature * Settings.RNG.NextDouble()))
            {
                // New state is better - move state
                Current = next;
                currentFitness = nextFitness;
                if (IsFirstBetter(currentFitness, bestFitness))
                {
                    // New state is best so far - store
                    StoreNewBest(Current);
                }
            }
            else
            { 
                // Reset to best value if stuck
                if (Settings.FailsBeforeReset > 0 &&
                    _SinceLastBest > Settings.FailsBeforeReset)
                {
                    // Restore best option as current
                    ResetToBest();
                }
            }

            return next;
        }

        /// <summary>
        /// Update the best state with a newly discovered variable.
        /// May be overridden to report on this.
        /// </summary>
        /// <param name="newBest"></param>
        public virtual void StoreNewBest(TPhenotype newBest)
        {
            Best = newBest;
            _SinceLastBest = 0;
        }

        /// <summary>
        /// Reset the current state to the last best found.
        /// May be overridden to report on this.
        /// </summary>
        public virtual void ResetToBest()
        {
            Current = Best;
            _SinceLastBest = 0;
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
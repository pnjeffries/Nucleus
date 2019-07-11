using Nucleus.Base;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Abstract base class for optimisation solvers
    /// </summary>
    [Serializable]
    public abstract class OptimisationSolverBase<TPhenotype, TSettings> : NotifyPropertyChangedBase
        where TSettings : OptimisationSettings
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Settings property
        /// </summary>
        private TSettings _Settings;

        /// <summary>
        /// The customisable settings used to determine the control parameters of the optimisation
        /// </summary>
        public TSettings Settings
        {
            get { return _Settings; }
            set { _Settings = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise the solver
        /// </summary>
        /// <param name="settings"></param>
        public OptimisationSolverBase(TSettings settings)
        {
            _Settings = settings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate a new Phenotype with randomised genes
        /// </summary>
        /// <returns></returns>
        public abstract TPhenotype GeneratePhenotype();

        /// <summary>
        /// Calculate or retrieve the overall fitness heuristic of a phenotype
        /// </summary>
        /// <param name="phenotype"></param>
        public abstract double OverallFitness(TPhenotype phenotype);

        /// <summary>
        /// Retrieve the single phenotype with the highest overall fitness from the specified population
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public virtual TPhenotype Fittest(IList<TPhenotype> population)
        {
            if (Settings.Objective == OptimisationObjective.Maximise)
            {
                // Find maximum
                return population.ItemWithMax(i => OverallFitness(i));
            }
            else
            {
                //Find minimum
                return population.ItemWithMin(i => OverallFitness(i));
            }
        }

        /// <summary>
        /// Evaluate the population and select the specified proportion with the best
        /// fitness scores.
        /// </summary>
        /// <param name="population">The population to select from.</param>
        /// <param name="proportion">The proportion of the population to select.</param>
        /// <returns></returns>
        public virtual IList<TPhenotype> Select(IList<TPhenotype> population,
            double proportion)
        {
            var sorted = new SortedList<double, TPhenotype>();
            foreach (var option in population)
            {
                double fitness = OverallFitness(option);
                if (fitness != double.NaN)
                {
                    sorted.AddSafe(fitness, option);
                }
            }
            int target = (int)Math.Ceiling(population.Count * proportion);
            target = Math.Min(target, sorted.Count);
            var result = new List<TPhenotype>(target);
            for (int i = 0; i < target; i++)
            {
                if (Settings.Objective == OptimisationObjective.Maximise)
                {
                    // Get maximum fitness:
                    result.Add(sorted.Values[sorted.Count - 1 - i]);
                }
                else
                {
                    // Get minimum fitness:
                    result.Add(sorted.Values[0]);
                }
            }
            return result;
        }

        /// <summary>
        /// Is the first fitness score preferable to the second, according to the
        /// current objective setting?
        /// </summary>
        /// <param name="firstFitness">The fitness of the first phenotype</param>
        /// <param name="secondFitness">The fitness of the second phenotype</param>
        /// <returns></returns>
        public bool IsFirstBetter(double firstFitness, double secondFitness)
        {
            if (Settings.Objective == OptimisationObjective.Maximise)
                return firstFitness > secondFitness;
            else
                return firstFitness < secondFitness;
        }

        #endregion
    }
}

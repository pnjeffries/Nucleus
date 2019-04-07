using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Abstract base class for genetic algorithm solvers
    /// </summary>
    public abstract class GeneticSolverBase : IGeneticSolver
    {
        /// <summary>
        /// Initialise the genetic algorithm by generating (or sampling)
        /// the initial solution set.
        /// </summary>
        /// <param name="settings"></param>
        public abstract IList<IGeneticAlgorithmPhenotype> Initialise(GeneticAlgorithmSettings settings);

        /// <summary>
        /// Calculate the overall fitness score of an option
        /// </summary>
        /// <param name="option"></param>
        /// <param name="settings"></param>
        public abstract double CalculateOverallFitness(IGeneticAlgorithmPhenotype option,
            GeneticAlgorithmSettings settings);

        /// <summary>
        /// Select a proportion of the population to carry on to the next generation based on their
        /// overall fitness scores.
        /// </summary>
        /// <param name="population">The population to select from.</param>
        /// <param name="proportion">The proportion of the population to select.</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public virtual IList<IGeneticAlgorithmPhenotype> Select(IList<IGeneticAlgorithmPhenotype> population, 
            double proportion, GeneticAlgorithmSettings settings)
        {
            var sorted = new SortedList<double, IGeneticAlgorithmPhenotype>();
            foreach (var option in population)
            {
                double fitness = CalculateOverallFitness(option, settings);
                if (fitness != double.NaN)
                {
                    sorted.AddSafe(fitness, option);
                }
            }
            int target = (int)Math.Ceiling(population.Count * proportion);
            target = Math.Min(target, sorted.Count);
            var result = new List<IGeneticAlgorithmPhenotype>(target);
            for (int i = 0; i < target; i++)
            {
                // Get maximum fitness:
                result.Add(sorted.Values[sorted.Count - 1 - i]);
                //TODO: Minimum?
            }
            return result;
        }

    }
}

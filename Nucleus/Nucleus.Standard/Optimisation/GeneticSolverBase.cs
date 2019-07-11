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
    [Serializable]
    public abstract class GeneticSolverBase<TPhenotype> 
        : OptimisationSolverBase<TPhenotype, GeneticAlgorithmSettings>
    {

        #region Constructors

        /// <summary>
        /// Initialise the solver
        /// </summary>
        public GeneticSolverBase() : base(new GeneticAlgorithmSettings())
        {
        }

        /// <summary>
        /// Initialise the solver
        /// </summary>
        /// <param name="settings"></param>
        public GeneticSolverBase(GeneticAlgorithmSettings settings) : base(settings)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Run a complete optimisation, returning the single 'best' option from
        /// the final generation.
        /// </summary>
        /// <returns></returns>
        public virtual TPhenotype Run()
        {
            // Create the initial population
            IList<TPhenotype> population = CreateInitialPopulation();

            // Iterate
            for (int i = 0; i < Settings.MaxGenerations; i++)
            {
                if (TerminationConditionsMet(population)) break;
                else population = SpawnNextGeneration(population);
            }

            // Return the best
            return Fittest(population);
        }

        /// <summary>
        /// Initialise the genetic algorithm by generating (or sampling)
        /// the initial solution set.
        /// </summary>
        public virtual IList<TPhenotype> CreateInitialPopulation()
        {
            var result = new List<TPhenotype>();
            int initialSize = (int)Math.Ceiling(Settings.PopulationSize * Settings.InitialBoost);
            for (int i = 0; i < initialSize; i++)
            {
                var phenotype = GeneratePhenotype();
                if (phenotype != null) result.Add(phenotype);
            }
            return result;
        }


        /// <summary>
        /// Generate the next generation of phenotypes
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public virtual IList<TPhenotype> SpawnNextGeneration
            (IList<TPhenotype> population)
        {
            // Select fittest
            var selected = Select(population, Settings.SurvivalRate);

            // Breed
            var newPopulation = Breed(selected);

            // Add freaks:
            AddWildcards(newPopulation);

            return newPopulation;
        }

        /// <summary>
        /// Crossover and mutate the selected set of phenotypes in order to produce
        /// the next generation.
        /// </summary>
        /// <param name="selected">The subset of the current generation selcted for breeding.</param>
        /// <returns></returns>
        public virtual IList<TPhenotype> Breed(
            IList<TPhenotype> selected)
        {
            var result = new List<TPhenotype>(Settings.PopulationSize);
            int count = (int)Math.Ceiling(Settings.PopulationSize * 
                (1.0 - Settings.WildcardRate - Settings.Elitism));

            // Carry forward elites:
            int elites = Math.Min(selected.Count,(int)Math.Ceiling(Settings.PopulationSize * Settings.Elitism));
            for (int i = 0; i < elites; i++)
            {
                result.Add(selected[i]);
            }

            // Randomly Pair-up:
            for (int i = 0; i < count; i++)
            {
                // Run through each phenotype (possibly several times)
                int jA = i % selected.Count;
                TPhenotype mother = selected[jA];
                // Randomly select a breeding partner
                int jB = Settings.RNG.Next(selected.Count);
                //TODO: Make higher fitnesses more likely?
                TPhenotype father = selected[jB];
                TPhenotype child = Breed(mother, father);
                if (child != null) result.Add(child);
            }
            return result;
        }

        /// <summary>
        /// Add wildcards to the specified population to bring it up to the target population size.
        /// </summary>
        /// <param name="population">The population produced through breeding.  This collection will be
        /// modified.</param>
        /// <returns></returns>
        public virtual void AddWildcards(IList<TPhenotype> population)
        {
            int iStart = population.Count;
            for (int i = iStart; i < Settings.PopulationSize; i++)
            {
                TPhenotype freak = GeneratePhenotype();
                population.Add(freak);
            }
        }

        /// <summary>
        /// Have the termination conditions for this algorithm been met?
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public virtual bool TerminationConditionsMet(IList<TPhenotype> population)
        {
            return false; //TODO: Implement!
        }

        /// <summary>
        /// Breed two phenotypes to produce a child.
        /// </summary>
        /// <param name="mother"></param>
        /// <param name="father"></param>
        /// <returns></returns>
        public abstract TPhenotype Breed(TPhenotype mother, TPhenotype father);

        #endregion
    }
}

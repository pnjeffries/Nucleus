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
    /// Class to contain parameters used in the solution of genetic algorithms
    /// </summary>
    [Serializable]
    public class GeneticAlgorithmSettings : OptimisationSettings
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
        /// Private backing member variable for the PopulationSize property
        /// </summary>
        private int _PopulationSize = 50;

        /// <summary>
        /// The number of Phenotypes to be considered in each generation.
        /// </summary>
        [AutoUI(1000)]
        public int PopulationSize
        {
            get { return _PopulationSize; }
            set { ChangeProperty(ref _PopulationSize, value); }
        }

        /// <summary>
        /// Private backing member variable for the InitialBoost property
        /// </summary>
        private double _InitialBoost = 2;

        /// <summary>
        /// The multiplier by which the typical population size should be scaled 
        /// up for the first generation.  Increasing this increases the coverage 
        /// of the initial sample and decreases the odds of a local maxima being missed.
        /// </summary>
        public double InitialBoost
        {
            get { return _InitialBoost; }
            set { ChangeProperty(ref _InitialBoost, value); }
        }

        /// <summary>
        /// Private backing member variable for the SurvivalRate property
        /// </summary>
        private double _SurvivalRate = 0.1;

        /// <summary>
        /// The proportion of each generation that survives to breed and produce the next
        /// </summary>
        [AutoUI(1010)]
        public double SurvivalRate
        {
            get { return _SurvivalRate; }
            set { ChangeProperty(ref _SurvivalRate, value); }
        }

        /// <summary>
        /// Private backing field for the Elitism property
        /// </summary>
        private double _Elitism = 0.05;

        /// <summary>
        /// The elitist selection proportion.  This proportion
        /// of each generation will be carried forward, unaltered, to
        /// the next generation.  This can be used to guarantee that
        /// the solution fitness does not decrease generation to generation.
        /// This cannot exceed the survival rate.
        /// </summary>
        [AutoUI(1020)]
        public double Elitism
        {
            get { return _Elitism; }
            set { ChangeProperty(ref _Elitism, value); }
        }

        /// <summary>
        /// Private backing member variable for the WildcardRate property
        /// </summary>
        private double _WildcardRate = 0.1;

        /// <summary>
        /// The proportion of each generation that should be made up of randomly-generated 
        /// 'wildcard' options rather than being bred from the survivors of the previous generation.
        /// </summary>
        [AutoUI(1030)]
        public double WildcardRate
        {
            get { return _WildcardRate; }
            set { ChangeProperty(ref _WildcardRate, value); }
        }

        /// <summary>
        /// Private backing field for MaxGenerations property
        /// </summary>
        private int _MaxGenerations = 1000;

        /// <summary>
        /// The maximum number of generations which may be performed before termination
        /// </summary>
        [AutoUI(2000)]
        public int MaxGenerations
        {
            get { return _MaxGenerations; }
            set { ChangeProperty(ref _MaxGenerations, value); }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new GeneticAlgorithmSettings object
        /// </summary>
        public GeneticAlgorithmSettings() { }

        #endregion
    }
}

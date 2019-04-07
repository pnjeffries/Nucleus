using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// An interface for objects which represent a distinct phenotype in a genetic algorithm solution
    /// </summary>
    public interface IGeneticAlgorithmPhenotype
    {
        /// <summary>
        /// Create a new option by breeding this option with another
        /// </summary>
        /// <param name="with">The other parent</param>
        /// <param name="settings">Genetic Algorithm settings</param>
        /// <returns></returns>
        IGeneticAlgorithmPhenotype Breed(IGeneticAlgorithmPhenotype with, GeneticAlgorithmSettings settings);

    }

    /// <summary>
    /// Extension methods for the IGeneticAlgorithmOption interface
    /// </summary>
    public static class IGeneticAlgorithmPhenotypeExtensions
    {
        /// <summary>
        /// Create a new option by breeding this option with another
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="option"></param>
        /// <param name="with">The other parent</param>
        /// <param name="settings">Genetic Algorithm settings</param>
        /// <returns></returns>
        public static TOption Breed<TOption>(this TOption option, TOption with, GeneticAlgorithmSettings settings)
            where TOption: class,IGeneticAlgorithmPhenotype
        {
            return option.Breed(with, settings) as TOption;
        }
    }
}

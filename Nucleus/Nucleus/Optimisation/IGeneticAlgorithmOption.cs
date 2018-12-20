using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// An interface for objects which represent an option in a genetic algorithm solution
    /// </summary>
    public interface IGeneticAlgorithmOption
    {
        /// <summary>
        /// Create a new option by breeding this option with another
        /// </summary>
        /// <param name="with">The other parent</param>
        /// <param name="settings">Genetic Algorithm settings</param>
        /// <returns></returns>
        IGeneticAlgorithmOption Breed(IGeneticAlgorithmOption with, GeneticAlgorithmSettings settings);

        /// <summary>
        /// Get the fitness score for this option
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        double GetFitness(GeneticAlgorithmSettings settings);
    }

    /// <summary>
    /// Extension methods for the IGeneticAlgorithmOption interface
    /// </summary>
    public static class IGeneticAlgorithmOptionExtensions
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
            where TOption: class,IGeneticAlgorithmOption
        {
            return option.Breed(with, settings) as TOption;
        }
    }
}

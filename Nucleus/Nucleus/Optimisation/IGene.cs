using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// An interface for object types which may be utilised as
    /// a 'gene' - a controlling generative parameter which
    /// drives a characteristic of an option to be considered
    /// within a genetic algorithm optimisation.
    /// </summary>
    public interface IGene
    {
        /// <summary>
        /// Crossover this gene with another
        /// </summary>
        /// <param name="other"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        IGene Crossover(IGene other, GeneticAlgorithmSettings settings);
    }

    /// <summary>
    /// Extension methods for the IGene interface
    /// </summary>
    public static class IGeneExtensions
    {
        /// <summary>
        /// Crossover this gene with another of the same type.
        /// </summary>
        /// <typeparam name="TGene"></typeparam>
        /// <param name="gene"></param>
        /// <param name="other"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static TGene Crossover<TGene>(this TGene gene, TGene other, GeneticAlgorithmSettings settings)
            where TGene : IGene
        {
            IGene result = gene.Crossover((IGene)other, settings);
            if (result != null && result is TGene) return (TGene)result;
            else return default(TGene);
        }
    }
}

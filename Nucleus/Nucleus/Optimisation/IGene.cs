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

        /// <summary>
        /// Mutate this gene
        /// </summary>
        /// <param name="settings"></param>
        void Mutate(GeneticAlgorithmSettings settings);
    }
}

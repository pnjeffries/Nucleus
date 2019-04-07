using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// An interface for classes which can be used to optimise
    /// via a genetic algorithm
    /// </summary>
    public interface IGeneticSolver
    {
        /// <summary>
        /// Initialise the genetic algorithm by generating (or sampling)
        /// the initial solution set.
        /// </summary>
        /// <param name="settings"></param>
        IList<IGeneticAlgorithmPhenotype> Initialise(GeneticAlgorithmSettings settings);
    }
}

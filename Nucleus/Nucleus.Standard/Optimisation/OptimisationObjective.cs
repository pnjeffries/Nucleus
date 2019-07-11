using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Enum to represent whether the target of an optimisation is to maximise or 
    /// minimise a given value
    /// </summary>
    public enum OptimisationObjective
    {
        /// <summary>
        /// The objective is to maximise the fitness
        /// </summary>
        Maximise = 0,

        /// <summary>
        /// The objective is to minimise the fitness
        /// </summary>
        Minimise = 1
    }
}

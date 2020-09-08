using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Optimisation
{
    /// <summary>
    /// Interface for transformations made to a phenotype
    /// during an optimisation operation
    /// </summary>
    public interface IOptimisationMove<TPhenotype>
    {
        /// <summary>
        /// Apply the transformation to 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        TPhenotype GenerateNext(TPhenotype current, double factor);
    }
}

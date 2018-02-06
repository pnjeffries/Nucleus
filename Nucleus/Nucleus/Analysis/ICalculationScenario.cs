using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Interface for classes which represent scenarios on which calculations
    /// are to be performed.
    /// </summary>
    public interface ICalculationScenario
    {
        /// <summary>
        /// The log to be used to print
        /// </summary>
        ICalculationLog Log { get; set; }
    }
}

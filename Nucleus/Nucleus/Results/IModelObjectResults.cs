using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Interface for dictionaries of results for a particular model object
    /// </summary>
    public interface IModelObjectResults
    {
        /// <summary>
        /// Get the results for the specified case
        /// </summary>
        /// <param name="rCase"></param>
        /// <returns></returns>
        ICaseResults Get(ResultsCase rCase);

        /// <summary>
        /// Remove any stored results for the specified case
        /// </summary>
        /// <param name="rCase"></param>
        bool Remove(ResultsCase rCase);
    }
}

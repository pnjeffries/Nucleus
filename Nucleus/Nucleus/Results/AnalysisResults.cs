using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Abstract base class for results relating to a specific object and stored on
    /// that object as a data component.  Within this dictionary, results are stored
    /// by case and then by type.
    /// </summary>
    [Serializable]
    public abstract class AnalysisResults<TResults> : ResultsDictionary<ResultsCase, TResults>
    {
    }
}

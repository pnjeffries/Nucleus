using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Results
{
    /// <summary>
    /// Storage dictionary for all the results belonging to a model,
    /// keyed by the analysis or combination case for which they were generated
    /// </summary>
    [Serializable]
    public class ModelResults : ResultsDictionary<ResultsCase, CaseResults>
    {
    }
}

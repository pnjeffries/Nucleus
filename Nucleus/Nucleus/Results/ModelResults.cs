using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// A top-level storage mechanism for all analysis results for a particular
    /// model, keyed by object, then by case, then by type
    /// </summary>
    public class ModelResults : ResultsDictionary<Guid, IModelObjectResult>
    {
    }
}

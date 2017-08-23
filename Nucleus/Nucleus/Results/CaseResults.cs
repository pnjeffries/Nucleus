using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Abstract base class for dictionaries of results for a particular
    /// case, keyed by result type
    /// </summary>
    public abstract class CaseResults<TType, TResults>
        : ResultsDictionary<TType, TResults>, ICaseResults
    {
    }
}

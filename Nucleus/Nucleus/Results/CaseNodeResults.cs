using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Node results for a specific results case, keyed by type
    /// </summary>
    [Serializable]
    public class CaseNodeResults : CaseResults<NodeResultTypes, Interval>
    {
    }
}

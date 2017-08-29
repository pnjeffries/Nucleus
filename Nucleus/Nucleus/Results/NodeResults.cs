using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Results dictionary for a specific node, keyed by case and then by type
    /// </summary>
    [Serializable]
    public class NodeResults : ModelObjectResults<CaseNodeResults>, IModelObjectResults
    {
    }
}

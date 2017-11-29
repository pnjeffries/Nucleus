using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model.Loading
{
    /// <summary>
    /// A force load applied directly to a set of nodes
    /// </summary>
    [Serializable]
    public class NodeLoad : ForceLoad<NodeSet, Node>
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Viscous
{
    /// <summary>
    /// An interface for input or output connectors
    /// </summary>
    public interface IFlowConnector
    {
        /// <summary>
        /// The component which owns this connector
        /// </summary>
        IFlowNode Owner { get; }


    }
}

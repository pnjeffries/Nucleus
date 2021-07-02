using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Viscous
{
    /// <summary>
    /// A link between a FlowNode output and input
    /// </summary>
    public interface IFlowLink
    {
        /// <summary>
        /// The connection point which this link flows from
        /// </summary>
        IFlowOutput From { get; set; }

        /// <summary>
        /// The connection point which this link flows to
        /// </summary>
        IFlowInput To { get; set; }
    }
}

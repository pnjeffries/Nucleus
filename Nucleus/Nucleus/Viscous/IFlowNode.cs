using Nucleus.Actions;
using Nucleus.Alerts;
using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Viscous
{
    /// <summary>
    /// Interface for visual scripting components
    /// </summary>
    public interface IFlowNode : IUnique
    {
        /// <summary>
        /// Get the collection of input parameters for this component
        /// </summary>
        ParameterCollection Inputs { get; }

        /// <summary>
        /// Get the collection of output parameters for this component.
        /// Note that the values of these outputs may not have been populated
        /// before the component has been executed.
        /// </summary>
        ParameterCollection Outputs { get; }

        /// <summary>
        /// Execute the component to consume the input parameters and 
        /// </summary>
        /// <param name="exInfo"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        bool Execute(ExecutionInfo exInfo, AlertLog log);
    }
}

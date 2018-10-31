using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// An interface for Salamander actions which can automatically extract input data from a 'context' object.
    /// Typically, this is used for actions linked to embedded buttons in WPF controls, where the passed in context object
    /// will be the DataContext property of the control.
    /// </summary>
    public interface IContextualAction : IAction
    {
        /// <summary>
        /// This function is to be used to extract any relevant inputs from a 'context' object.
        /// Extracting the data from the context and populating the input properties is down to you.
        /// </summary>
        /// <param name="context">The object which describes the context of the action.
        /// For example, if called from a WPF button this might be the datacontext of that
        /// control.</param>
        /// <returns></returns>
        bool PopulateInputsFromContext(object context);
    }
}

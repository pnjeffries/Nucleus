using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// A base class for actions.  This provides some built-in functionality for the sake of convenience,
    /// however you can go fully custom if you want by implementing the IAction interface yourself.
    /// </summary>
    [Serializable]
    public abstract class ParametricAction : IAction
    {
        #region Methods

        /// <summary>
        /// Execute this action.  Input parameters will be consumed and output parameters will be populated.
        /// </summary>
        /// <param name="exInfo">A set of data communicating the parametric context of execution.
        /// Used to update existing outputs from the same parametric source instead of generating new ones.
        /// Will be null if the source is not part of a parametric process.</param>
        /// <returns>Should return true if the action is successfully completed, false otherwise</returns>
        public abstract bool Execute(ExecutionInfo exInfo = null);

        /// <summary>
        /// Return a list of the PropertyInfo objects for all input parameters of this action.
        /// </summary>
        /// <returns></returns>
        public IList<PropertyInfo> InputParameters()
        {
            return this.ExtractInputParameters();
        }

        /// <summary>
        /// Return a list of the PropertyInfo objects for all output parameters of this action.
        /// </summary>
        /// <returns></returns>
        public IList<PropertyInfo> OutputParameters()
        {
            return this.ExtractOutputParameters();
        }

        /// <summary>
        /// Base PostExecutionOperations implementation.  Override this to add operations to be performed after each execution.
        /// </summary>
        /// <param name="exInfo"></param>
        /// <returns></returns>
        public virtual bool PostExecutionOperations(ExecutionInfo exInfo = null)
        {
            return true;
        }

        /// <summary>
        /// Base PreExecutionOperations implementation.  Override this to add operations to be performed prior to each execution.
        /// </summary>
        /// <param name="exInfo"></param>
        /// <returns></returns>
        public virtual bool PreExecutionOperations(ExecutionInfo exInfo = null)
        {
            return true;
        }

        /// <summary>
        /// A function which will be called on the last action to be executed in a sequence, after any
        /// PostExecutionOperations have been performed..  For example, in a Grasshopper component dealing 
        /// with multiple inputs, this will only be called once all inputs have been dealt with.
        /// Use this to define any tidying up operations that should be performed after the final iteration.
        /// </summary>
        /// <param name="exInfo"></param>
        /// <returns></returns>
        public virtual bool FinalOperations(ExecutionInfo exInfo = null)
        {
            return true;
        }

        #endregion
    }
}

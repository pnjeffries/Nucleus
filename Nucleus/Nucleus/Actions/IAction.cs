using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// An interface which must be implemented by any class which represents an action.
    /// You can implement this interface entirely yourself, but probably it is better simply to inherit from
    /// an appropriate base class (ActionBase, DocumentActionBase etc.), which will provide a lot of the basic functionality
    /// for you.
    /// </summary>
    public interface IAction
    {

        /// <summary>
        /// Execute this action.  Input parameters will be consumed and output parameters will be populated.
        /// </summary>
        /// <param name="exInfo">A set of data communicating the parametric context of execution.
        /// Used to update existing outputs from the same parametric source instead of generating new ones.
        /// Will be null if the source is not part of a parametric process.</param>
        /// <returns>Should return true if the action is successfully completed, false otherwise</returns>
        bool Execute(ExecutionInfo exInfo = null);

        /// <summary>
        /// A function which will be called before the action is executed in normal execution mode.
        /// </summary>
        /// <returns>True if execution can be allowed to proceed</returns>
        bool PreExecutionOperations(ExecutionInfo exInfo = null);

        /// <summary>
        /// A function which will be called after the action has been executed in normal execution mode.
        /// Override to define specific behaviours - typically to consume the outputs of the execution.
        /// </summary>
        /// <returns></returns>
        bool PostExecutionOperations(ExecutionInfo exInfo = null);

        /// <summary>
        /// A function which will be called on the last action to be executed in a sequence, after any
        /// PostExecutionOperations have been performed.  For example, in a Grasshopper component dealing 
        /// with multiple inputs, this will only be called once all inputs have been dealt with.
        /// Use this to define any tidying up operations that should be performed after the final iteration.
        /// </summary>
        /// <param name="exInfo"></param>
        /// <returns></returns>
        bool FinalOperations(ExecutionInfo exInfo = null);

        /// <summary>
        /// Return a list of the PropertyInfo objects for all input parameters of this action.
        /// </summary>
        /// <returns></returns>
        IList<PropertyInfo> InputParameters();

        /// <summary>
        /// Return a list of the PropertyInfo objects for all output parameters of this action.
        /// </summary>
        /// <returns></returns>
        IList<PropertyInfo> OutputParameters();

    }

    /// <summary>
    /// Extension methods for the IAction interface
    /// </summary>
    public static class IActionExtensions
    {
        /// <summary>
        /// Retrieve the action attributes for this action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ActionAttribute GetAttributes(this IAction action)
        {
            return ActionAttribute.ExtractFrom(action);
        }

        /// <summary>
        /// Retrieve the command name for this action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetCommandName(this IAction action)
        {
            Type type = action.GetType();
            string commandName;
            object[] typeAtts = type.GetCustomAttributes(typeof(ActionAttribute), false);
            if (typeAtts.Count() > 0)
            {
                ActionAttribute aAtt = (ActionAttribute)typeAtts[0];
                commandName = aAtt.CommandName;
            }
            else commandName = type.Name;
            return commandName;
        }

        /// <summary>
        /// Copy across persistent input parameter values from another action of the same type
        /// </summary>
        /// <param name="action"></param>
        /// <param name="other"></param>
        public static void CopyPersistentValuesFrom(this IAction action, IAction other)
        {
            if (other != null && action.GetType().IsAssignableFrom(other.GetType()))
            {
                IList<PropertyInfo> inputs = action.InputParameters();
                foreach (PropertyInfo input in inputs)
                {
                    ActionInputAttribute inputAtt = ActionInputAttribute.ExtractFrom(input);
                    if (inputAtt != null && inputAtt.Persistant)
                    {
                        input.SetValue(action, input.GetValue(other, null), null);
                    }
                }
            }
        }

        /// <summary>
        /// Generate a dictionary of the current output property values, keyed by their names
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetOutputsDictionary(this IAction action)
        {
            var result = new Dictionary<string, object>();
            var outputs = action.OutputParameters();
            foreach (PropertyInfo pInfo in outputs)
            {
                result.Add(pInfo.Name, pInfo.GetValue(result));
            }
            return result;
        }

        /// <summary>
        /// Helper function to extract all properties from the given type with the specified type of
        /// ActionParameter annotation.
        /// </summary>
        /// <param name="type">The type from which to extract parameters</param>
        /// <param name="parameterType">The subtype of ActionParameter annotation to be searched for</param>
        /// <returns></returns>
        public static IList<PropertyInfo> ExtractActionParameters(this IAction action, Type parameterType)
        {
            Type type = action.GetType();
            SortedList<double, PropertyInfo> result = new SortedList<double, PropertyInfo>();
            PropertyInfo[] pInfos = type.GetProperties();
            foreach (PropertyInfo pInfo in pInfos)
            {
                object[] attributes = pInfo.GetCustomAttributes(parameterType, true);
                if (attributes.Count() > 0)
                {
                    ActionParameterAttribute aInput = (ActionParameterAttribute)attributes[0];
                    result.AddSafe(aInput.Order, pInfo);
                }
            }
            return result.Values.ToList();
        }

        /// <summary>
        /// Extract a list of all properties annotated as ActionInputs from the specified action
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<PropertyInfo> ExtractInputParameters(this IAction action)
        {
            return action.ExtractActionParameters(typeof(ActionInputAttribute));
        }

        /// <summary>
        /// Extract a list of all properties annotated as ActionOutputs from the specififed action
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<PropertyInfo> ExtractOutputParameters(this IAction action)
        {
            return action.ExtractActionParameters(typeof(ActionOutputAttribute));
        }
    }
}

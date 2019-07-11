using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// Attribute c
    /// </summary>
    public class ActionAttribute : Attribute
    {
        /// <summary>
        /// The command string used to manually call this action
        /// </summary>
        public string CommandName { get; set; } = null;

        /// <summary>
        /// The description of this command
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// The URI string that identifies the image resource to be used as the foreground of the icon for this action 
        /// on buttons and grasshopper components.
        /// </summary>
        public string IconForeground { get; set; } = null;

        /// <summary>
        /// The URI string that identifies the image resource to be used as the background of the icon for this action
        /// on button and grasshopper components.
        /// </summary>
        public string IconBackground { get; set; } = null;

        /// <summary>
        /// The type of the preview layer (if any) to be used to display the results of this operation.
        /// </summary>
        public Type PreviewLayerType { get; set; } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandName"></param>
        public ActionAttribute(string commandName)
        {
            CommandName = commandName;
        }

        /// <summary>
        /// CommandName, Description constructor
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="description"></param>
        public ActionAttribute(string commandName, string description) : this(commandName)
        {
            Description = description;
        }

        /// <summary>
        /// CommandName, Description, Icon constructor
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="description"></param>
        /// <param name="iconBackground"></param>
        public ActionAttribute(string commandName, string description, string iconBackground) : this(commandName, description)
        {
            IconBackground = iconBackground;
        }

        /// <summary>
        /// CommandName, Description, Icon (2-layer) constructor
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="description"></param>
        /// <param name="iconBackground"></param>
        /// <param name="iconForeground"></param>
        public ActionAttribute(string commandName, string description, string iconBackground, string iconForeground) : this(commandName, description, iconBackground)
        {
            IconForeground = iconForeground;
        }

        /// <summary>
        /// Helper function to get the (first) ActionAttribute from the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ActionAttribute ExtractFrom(Type type)
        {
            object[] actionAtts = type.GetCustomAttributes(typeof(ActionAttribute), false);
            if (actionAtts.Count() > 0)
            {
                return (ActionAttribute)actionAtts[0];
            }
            return null;
        }

        /// <summary>
        /// Helper function to get the action attributes from an action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ActionAttribute ExtractFrom(IAction action)
        {
            if (action == null) return null;
            return ExtractFrom(action.GetType());
        }
    }
}

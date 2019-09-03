using Nucleus.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// Attributes of an action output property.
    /// Used to specify information about the input and how it should be obtained/used.
    /// </summary>
    public class ActionOutputAttribute : ActionParameterAttribute
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="order"></param>
        /// <param name="description"></param>
        public ActionOutputAttribute(int order, string description)
        {
            Order = order;
            Description = description;
        }

        #endregion

        /// <summary>
        /// Helper function to get the (first) ActionInput attribute from the specified PropertyInfo
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static ActionOutputAttribute ExtractFrom(PropertyInfo pInfo)
        {
            object[] actionAtts = pInfo.GetCustomAttributes(typeof(ActionOutputAttribute), false);
            if (actionAtts.Count() > 0)
            {
                return (ActionOutputAttribute)actionAtts[0];
            }
            return null;
        }
    }
}

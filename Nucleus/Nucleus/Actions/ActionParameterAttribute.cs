using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// Base class for Action Parameter attributes.  Used to annotate action input and output
    /// properties in order to define more information about how they should be used.
    /// </summary>
    public class ActionParameterAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Weighting value for the display order.  Parameters will be sorted by this value.
        /// Lower values will be displayed first.
        /// </summary>
        public int Order { get; set; } = 1000000;

        /// <summary>
        /// The abbreviated version of this property name to be displayed in limited-space situations
        /// such as grasshopper inputs and outputs.  If not explicitly set, the short name will usually
        /// be generated automatically from the property name itself.
        /// </summary>
        public string ShortName { get; set; } = null;

        /// <summary>
        /// The description of this parameter.  This may be displayed as a tooltip or a prompt.
        /// In the latter case, this string may be prefixed with 'Enter ' - therefore do not capitalise the
        /// first letter of this description unless it should always be capitalised (i.e. it is a noun).
        /// In cases where there is no prefix, the first letter will be automatically capitalised.
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// A version of the Description property with the first character capitalised
        /// </summary>
        public string CapitalisedDescription
        {
            get
            {
                if (Description != null && Description.Length > 0)
                {
                    return Description.Substring(0, 1).ToUpper() + Description.Substring(1);
                }
                return "";
            }
        }

        #endregion
    }
}

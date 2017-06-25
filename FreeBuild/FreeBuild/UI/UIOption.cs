using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UI
{
    /// <summary>
    /// Class representing an option to be displayed within a user interface
    /// </summary>
    public class UIOption
    {
        #region Fields

        /// <summary>
        /// The text to be displayed to describe this option
        /// </summary>
        public readonly string Text;

        /// <summary>
        /// The value to be returned when this option is selected
        /// </summary>
        public readonly object ReturnValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new UIOption with the specified text and return value
        /// </summary>
        /// <param name="text"></param>
        /// <param name="returnValue"></param>
        public UIOption(string text, object returnValue)
        {
            Text = text;
            ReturnValue = returnValue;
        }

        /// <summary>
        /// Initialise a new UIOption with the specified text.
        /// This object will be its own return value.
        /// </summary>
        /// <param name="text"></param>
        public UIOption(string text)
        {
            Text = text;
            ReturnValue = this;
        }

        #endregion


    }
}

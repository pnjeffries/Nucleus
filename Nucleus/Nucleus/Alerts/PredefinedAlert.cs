using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// A type of alert which consists only of a 
    /// </summary>
    [Serializable]
    public class PredefinedAlert : Alert
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Objects property
        /// </summary>
        private IList<object> _Objects = null;

        /// <summary>
        /// The collection of objects to be used to populate formatted string messages in the UI
        /// </summary>
        public IList<object> Objects
        {
            get { return _Objects; }
            set { _Objects = value; }
        }

        #endregion

        public PredefinedAlert(string predefID, object[] objects, AlertLevel level = AlertLevel.Information)
            :base(predefID, level)
        {
            _Objects = objects;
        }

        public PredefinedAlert(string alertID, string predefID, object[] objects, AlertLevel level = AlertLevel.Information)
            :base(alertID, predefID, level)
        {
            _Objects = objects;
        }

        /// <summary>
        /// Get the display message for this alert, adjusting to predefined alerts as necessary.
        /// </summary>
        /// <param name="predefinedMessages">A dictionary of predefined messages.  The set
        /// message of this alert will be used as a key in this dictionary to create the
        /// final message.  The stored objects of this alert will be used to populate
        /// the string in the event that it is a format string.</param>
        /// <returns></returns>
        public override string GetDisplayText(IDictionary<string, string> predefinedMessages)
        {
            string message = Message;
            if (predefinedMessages != null && predefinedMessages.ContainsKey(Message))
            {
                message = predefinedMessages[message];
                if (_Objects != null) message = string.Format(message, _Objects);
            }
            return message;
        }
    }
}

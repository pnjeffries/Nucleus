using Nucleus.Alerts;
using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// Class to store warning messages which are liable to crop
    /// up multiple times over the course of a process and which
    /// may need to be collated and displayed together
    /// </summary>
    [Serializable]
    public class Alert : Unique
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the AlertID property
        /// </summary>
        private string _AlertID = new Guid().ToString();

        /// <summary>
        /// The ID of the alert type - multiple alerts with the same ID will be merged together
        /// </summary>
        public string AlertID
        {
            get { return _AlertID; }
        }

        /// <summary>
        /// Private backing member variable for the Message property
        /// </summary>
        private string _Message;

        /// <summary>
        /// A customisable message which will be shown as part of this alert
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                NotifyPropertyChanged("Message");
                NotifyPropertyChanged("DisplayText");
            }
        }

        /// <summary>
        /// The text which is displayed in the UI to describe this alert
        /// to the user.  By default consists of the set Message, but may
        /// be overridden to allow composite messages to be displayed.
        /// </summary>
        public virtual string DisplayText
        {
            get { return _Message; }
        }

        /// <summary>
        /// Private backing member variable for the Level property
        /// </summary>
        private AlertLevel _Level = AlertLevel.Information;

        /// <summary>
        /// The level of the alert - used to indicate the degree of seriousness of the message.
        /// </summary>
        public AlertLevel Level
        {
            get { return _Level; }
            set
            {
                _Level = value;
                NotifyPropertyChanged("Level");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new generic alert with the specified ID and message
        /// </summary>
        /// <param name="alertID">The ID of the alert.  Multiple alerts raised with the same ID may be merged.</param>
        /// <param name="message">The alert message to display.</param>
        /// <param name="level">The alert level of the message - indicates how serious the alert is.</param>
        public Alert(string alertID, string message, AlertLevel level = AlertLevel.Information)
        {
            _AlertID = alertID;
            _Message = message;
            _Level = level;
        }

        /// <summary>
        /// Initialise a new generic alert with the specified message
        /// </summary>
        /// <param name="message">The alert message to display.</param>
        /// <param name="level">The alert level of the message - indicates how serious the alert is.</param>
        public Alert(string message, AlertLevel level = AlertLevel.Information)
        {
            _Message = message;
            _Level = level;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Merge another alert into this one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual void Merge(Alert other)
        {
            Message = other.Message;
            NotifyPropertiesChanged("DisplayText");
        }


        public override string ToString()
        {
            return DisplayText;
        }

        /// <summary>
        /// Get the display message for this alert, adjusting to predefined alerts as necessary.
        /// </summary>
        /// <param name="predefinedMessages">A dictionary of predefined messages.  The set
        /// message of this alert will be used as a key in this dictionary to create the
        /// final message.  The stored objects of this alert will be used to populate
        /// the string in the event that it is a format string.  Only some alert types will
        /// make use of this; most will use the raw message.</param>
        /// <returns></returns>
        public virtual string GetDisplayText(IDictionary<string, string> predefinedMessages)
        {
            return DisplayText;
        }

        #endregion
    }
}

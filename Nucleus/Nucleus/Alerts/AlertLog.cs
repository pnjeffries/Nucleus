using Nucleus.Base;
using Nucleus.Logs;
using Nucleus.Model;
using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// A log to store alerts raised during a process
    /// </summary>
    [Serializable]
    public class AlertLog : AlertCollection, ILog
    {

        #region Properties

        ///// <summary>
        ///// Private backing member variable for the PredefinedMessages property
        ///// </summary>
        //private IDictionary<string, string> _PredefinedMessages;

        ///// <summary>
        ///// The dictionary of predefined alert messages
        ///// </summary>
        //public IDictionary<string, string> PredefinedMessages
        //{
        //    get { return _PredefinedMessages; }
        //    set { _PredefinedMessages = value; }
        //}

        bool ILog.IsBold { get { return false; }  set { } }
        bool ILog.IsItalicised { get { return false; } set { } }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new AlertLog with default properties
        /// </summary>
        public AlertLog() { }

        /*
        /// <summary>
        /// Create a new AlertLog set up to use the specified dictionary of predefined
        /// message formats
        /// </summary>
        /// <param name="predefinedMessages"></param>
        public AlertLog(IDictionary<string, string> predefinedMessages) : this()
        {
            _PredefinedMessages = predefinedMessages;
        }
        */

        #endregion

        #region Methods

        /// <summary>
        /// Raise an alert
        /// </summary>
        /// <param name="alert">The alert to add to the log</param>
        public virtual void RaiseAlert(Alert alert)
        {
            //Application.Current.Dispatcher.Invoke(new Action(() => AddOrMerge(alert)));
            //Dispatcher.Invoke(new Action(() => AddOrMerge(alert)));
            AddOrMerge(alert);
        }

        /// <summary>
        /// Raise a unique alert based on an exception
        /// </summary>
        /// <param name="ex">The exception to raise an alert based on</param>
        public void RaiseAlert(Exception ex, AlertLevel level = AlertLevel.Error)
        {
            RaiseAlert(ex.Message, level);
        }

        /// <summary>
        /// Raise an alert based on an exception, merging it with any 
        /// previous alerts with the same ID
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  
        /// Multiple alerts with the same ID will be merged.</param>
        /// <param name="ex">The exception to raise an alert based on</param>
        /// <param name="level">The level of the alert</param>
        public void RaiseAlert(string alertID, Exception ex, AlertLevel level = AlertLevel.Error)
        {
            RaiseAlert(alertID, ex.Message, level);
        }

        /// <summary>
        /// Raise a unique alert
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="level">The level of the alert</param>
        public void RaiseAlert(string message, AlertLevel level = AlertLevel.Information)
        {
            RaiseAlert(new Alert(message, level));
        }

        /// <summary>
        /// Raise an alert, merging it with any previous alerts with the same ID
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="message">The message to display</param>
        /// <param name="level">The level of the alert</param>
        public void RaiseAlert(string alertID, string message, AlertLevel level = AlertLevel.Information)
        {
            RaiseAlert(new Alert(alertID, message, level));
        }

        /// <summary>
        /// Raise a progress alert, merging it with any previous alerts with the same ID
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="message">The message to display</param>
        /// <param name="progress">The progress of the operation</param>
        /// <param name="level">The level of the alert</param>
        public void RaiseAlert(string alertID, string message, double progress, AlertLevel level = AlertLevel.Information)
        {
            RaiseAlert(new ProgressAlert(alertID, message, progress, level));
        }

        /// <summary>
        /// Raise an alert regarding an Element, merging it with any previous alerts with the same ID
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="element">The element to which the alert refers.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="level">The level of the alert.</param>
        public void RaiseAlert(string alertID, Element element, string message, AlertLevel level = AlertLevel.Information)
        {
            RaiseAlert(new ElementAlert(alertID, element, message, level));
        }

        /// <summary>
        /// Raise an alert regarding a Node, merging it with any previous alerts with the same ID
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="node">The node to which the alert refers.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="level">The level of the alert.</param>
        public void RaiseAlert(string alertID, Node node, string message, AlertLevel level = AlertLevel.Information)
        {
            RaiseAlert(new NodeAlert(alertID, node, message, level));
        }

        /// <summary>
        /// Raise an alert regarding a Load, merging it with any previous alerts with the same ID
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="load">The element to which the alert refers.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="level">The level of the alert.</param>
        public void RaiseAlert(string alertID, Load load, string message, AlertLevel level = AlertLevel.Information)
        {
            RaiseAlert(new LoadAlert(alertID, load, message, level));
        }

        /*
        /// <summary>
        /// Get a predefined message from the predefined messages dictionary, if one has been predefined.
        /// </summary>
        /// <param name="predefKey"></param>
        /// <returns></returns>
        private string GetPredefinedMessage(string predefKey)
        {
            string message;
            if (PredefinedMessages != null && PredefinedMessages.ContainsKey(predefKey))
                message = PredefinedMessages?[predefKey];
            else
                message = predefKey;
            return message;
        }

        /// <summary>
        /// Get a predefined message from the predefined messages dictionary, if one has been predefined.
        /// </summary>
        /// <param name="predefKey"></param>
        /// <param name="subjects"></param>
        /// <returns></returns>
        private string GetPredefinedMessage(string predefKey, params object[] subjects)
        {
            string message = GetPredefinedMessage(predefKey);
            message = string.Format(message, subjects);
            return message;
        }

        /// <summary>
        /// Raise an alert utilising a predefined message indicated by a key value.  Allows for different languages
        /// to be catered for by alerts.  If using this function the PredefinedMessages dictionary should have been
        /// populated for this alertlog.
        /// </summary>
        /// <param name="predefKey">The key used to identify a predefined message.</param>
        /// <param name="level">The level of the alert.</param>
        /// <param name="subjects">Objects to be included in the message.</param>
        public void RaisePredefinedAlert(string predefKey, AlertLevel level, params object[] subjects)
        {
            string message = GetPredefinedMessage(predefKey, subjects);
            RaiseAlert( message, level);
        }

        /// <summary>
        /// Raise an alert utilising a predefined message indicated by a key value.  Allows for different languages
        /// to be catered for by alerts.  If using this function the PredefinedMessages dictionary should have been
        /// populated for this alertlog.
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="predefKey">The key used to identify a predefined message.</param>
        /// <param name="level">The level of the alert.</param>
        /// <param name="subjects">Objects to be included in the message.</param>
        public void RaisePredefinedAlert(string alertID, string predefKey, AlertLevel level, params object[] subjects)
        {
            string message = GetPredefinedMessage(predefKey, subjects);
            RaiseAlert(alertID, message, level);
        }

        /// <summary>
        /// Raise an alert utilising a predefined message indicated by a key value.  Allows for different languages
        /// to be catered for by alerts.  If using this function the PredefinedMessages dictionary should have been
        /// populated for this alertlog.
        /// </summary>
        /// <param name="predefKey">The key used to identify a predefined message.</param>
        /// <param name="level">The level of the alert.</param>
        public void RaisePredefinedAlert(string predefKey, AlertLevel level = AlertLevel.Information)
        {
            string message = GetPredefinedMessage(predefKey);
            RaiseAlert(message, level);
        }

        /// <summary>
        /// Raise an alert utilising a predefined message indicated by a key value.  Allows for different languages
        /// to be catered for by alerts.  If using this function the PredefinedMessages dictionary should have been
        /// populated for this alertlog.
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="predefKey">The key used to identify a predefined message.</param>
        /// <param name="level">The level of the alert.</param>
        public void RaisePredefinedAlert(string alertID, string predefKey, AlertLevel level = AlertLevel.Information)
        {
            string message = GetPredefinedMessage(predefKey);
            RaiseAlert(alertID, message, level);
        }
        */

        /// <summary>
        /// ILog WriteText implementation
        /// </summary>
        /// <param name="text"></param>
        void ILog.WriteText(string text)
        {
            RaiseAlert(text);
        }

        #endregion
    }
}

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
    public class AlertLog : AlertCollection
    {
        /// <summary>
        /// Raise an alert
        /// </summary>
        /// <param name="alert">The alert to add to the log</param>
        public void RaiseAlert(Alert alert)
        {
            AddOrMerge(alert);
        }

        /// <summary>
        /// Raise a unique alert
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="level">The level of the alert</param>
        public void RaiseAlert(string message, AlertLevel level = AlertLevel.Information)
        {
            AddOrMerge(new Alert(message, level));
        }

        /// <summary>
        /// Raise an alert, merging it with any previous alerts with the same ID
        /// </summary>
        /// <param name="alertID">The identifier for the alert type.  Multiple alerts with the same ID will be merged.</param>
        /// <param name="message">The message to display</param>
        /// <param name="level">The level of the alert</param>
        public void RaiseAlert(string alertID, string message, AlertLevel level = AlertLevel.Information)
        {
            AddOrMerge(new Alert(alertID, message, level));
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
            AddOrMerge(new ElementAlert(alertID, element, message, level));
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
            AddOrMerge(new NodeAlert(alertID, node, message, level));
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
            AddOrMerge(new LoadAlert(alertID, load, message, level));
        }
    }
}

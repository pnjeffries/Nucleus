﻿using Nucleus.Base;
using Nucleus.Logs;
using Nucleus.Model;
using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Nucleus.Alerts
{
    /// <summary>
    /// A log to store alerts raised during a process
    /// </summary>
    public class AlertLog : AlertCollection, ILog
    {
        bool ILog.IsBold { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        bool ILog.IsItalicised { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

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

        /// <summary>
        /// ILog WriteText implementation
        /// </summary>
        /// <param name="text"></param>
        void ILog.WriteText(string text)
        {
            RaiseAlert(text);
        }
    }
}

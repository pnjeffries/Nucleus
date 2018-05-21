using Nucleus.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nucleus.WPF
{
    /// <summary>
    /// An alert log which can raise alerts asynchronously using the UI dispatcher
    /// to enable alerts to be updated during a process.
    /// </summary>
    public class AsyncAlertLog : AlertLog
    {
        #region Properties

        /// <summary>
        /// The window whose dispatcher this alert log uses to invoke alert changes
        /// </summary>
        public Window Window { get; set; }

        #endregion

        #region Constructors

        public AsyncAlertLog():base() { }

        public AsyncAlertLog(Window window):base() { Window = window; }

        #endregion

        #region Methods

        public override void RaiseAlert(Alert alert)
        {
            Window.Dispatcher.Invoke(new Action(() => AddOrMerge(alert)));
        }

        #endregion
    }
}

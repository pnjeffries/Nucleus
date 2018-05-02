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
        public Window Window { get; set; }

        public override void RaiseAlert(Alert alert)
        {
            Window.Dispatcher.Invoke(new Action(() => AddOrMerge(alert)));
        }
    }
}

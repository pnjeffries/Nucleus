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
    /// An alert relating to one of many Loads
    /// </summary>
    public class LoadAlert : ModelObjectAlert<Load, LoadCollection>
    {
        public LoadAlert(string message, IList<Load> items, AlertLevel level = AlertLevel.Information) : base(message, items, level)
        {
        }

        public LoadAlert(string message, Load item, AlertLevel level = AlertLevel.Information) : base(message, item, level)
        {
        }

        public LoadAlert(string alertID, IList<Load> items, string message, AlertLevel level = AlertLevel.Information) : base(alertID, items, message, level)
        {
        }

        public LoadAlert(string alertID, Load item, string message, AlertLevel level = AlertLevel.Information) : base(alertID, item, message, level)
        {
        }
    }
}

using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// An alert relating to one or more elements
    /// </summary>
    public class ElementAlert : ModelObjectAlert<Element, ElementCollection>
    {
        public ElementAlert(string message, IList<Element> items, AlertLevel level = AlertLevel.Information) : base(message, items, level)
        {
        }

        public ElementAlert(string message, Element item, AlertLevel level = AlertLevel.Information) : base(message, item, level)
        {
        }

        public ElementAlert(string alertID, IList<Element> items, string message, AlertLevel level = AlertLevel.Information) : base(alertID, items, message, level)
        {
        }

        public ElementAlert(string alertID, Element item, string message, AlertLevel level = AlertLevel.Information) : base(alertID, item, message, level)
        {
        }
    }
}

using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// An alert relating to one or more Nodes
    /// </summary>
    public class NodeAlert : ModelObjectAlert<Node, NodeCollection>
    {
        public NodeAlert(string message, IList<Node> items, AlertLevel level = AlertLevel.Information) : base(message, items, level)
        {
        }

        public NodeAlert(string message, Node item, AlertLevel level = AlertLevel.Information) : base(message, item, level)
        {
        }

        public NodeAlert(string alertID, IList<Node> items, string message, AlertLevel level = AlertLevel.Information) : base(alertID, items, message, level)
        {
        }

        public NodeAlert(string alertID, Node item, string message, AlertLevel level = AlertLevel.Information) : base(alertID, item, message, level)
        {
        }
    }
}

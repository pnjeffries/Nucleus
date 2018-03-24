using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// A collection of alerts
    /// </summary>
    public class AlertCollection : ObservableKeyedCollection<string, Alert>
    {
        protected override string GetKeyForItem(Alert item)
        {
            return item.AlertID;
        }

        /// <summary>
        /// Add an alert to this collection if it is unique, or merge
        /// it with an existing alert if one exists in this collection
        /// already with the same AlertID
        /// </summary>
        /// <param name="alert"></param>
        public void AddOrMerge(Alert alert)
        {
            if (Contains(alert.AlertID))
            {
                this[alert.AlertID].Merge(alert);
            }
            else
                Add(alert);
        }
    }
}

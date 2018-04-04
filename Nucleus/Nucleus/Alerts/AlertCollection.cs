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

        #region Methods

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

        /// <summary>
        /// Get the highest level of an alert in this collection
        /// </summary>
        /// <returns></returns>
        public AlertLevel HighestLevel()
        {
            AlertLevel result = AlertLevel.Information;
            foreach (var alert in this)
                if (alert.Level > result) result = alert.Level;
            return result;
        }

        #endregion
    }
}

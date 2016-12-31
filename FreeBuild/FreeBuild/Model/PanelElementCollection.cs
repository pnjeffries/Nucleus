using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of PanelElements
    /// </summary>
    [Serializable]
    public class PanelElementCollection : ElementCollection<PanelElement, PanelElementCollection>
    {
        #region Methods

        /// <summary>
        /// Get the subset of items in this collection which has a recorded modification 
        /// after the specified date and time
        /// </summary>
        /// <param name="since">The date/time to filter by</param>
        /// <returns></returns>
        public PanelElementCollection Modified(DateTime since)
        {
            return this.Modified<PanelElementCollection, PanelElement>(since);
        }

        #endregion
    }
}

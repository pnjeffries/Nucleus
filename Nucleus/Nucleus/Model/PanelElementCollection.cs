using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A collection of PanelElements
    /// </summary>
    [Serializable]
    public class PanelElementCollection : ElementCollection<PanelElement, PanelElementCollection>
    {
        #region Constructors

        /// <summary>
        /// Initialise a new blank PanelElementCollection
        /// </summary>
        public PanelElementCollection() : base() { }

        /// <summary>
        /// Initialise a new PanelElementCollection containing the specified single element
        /// </summary>
        /// <param name="element"></param>
        public PanelElementCollection(PanelElement element) : base()
        {
            Add(element);
        }

        #endregion

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

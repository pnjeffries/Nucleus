using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract base class for conditional filters
    /// </summary>
    [Serializable]
    public abstract class SetFilter<TItem> : Unique
    {
        /// <summary>
        /// Does the specified item pass through this filter?
        /// </summary>
        /// <param name="item">The item to be tested</param>
        /// <returns>True if the item passes through the filter, false if not.</returns>
        public abstract bool Pass(TItem item);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Interface for conditional filters suitable to be applied to sets
    /// </summary>
    public interface ISetFilter<TItem> : IUnique
    {
        /// <summary>
        /// Does the specified item pass through this filter?
        /// </summary>
        /// <param name="item">The item to be tested</param>
        /// <returns>True if the item passes through the filter, false if not.</returns>
        bool Pass(TItem item);
    }
}

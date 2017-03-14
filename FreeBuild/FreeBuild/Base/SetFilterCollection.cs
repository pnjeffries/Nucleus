using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Generic base type for collections of Set Filters
    /// </summary>
    [Serializable]
    public class SetFilterCollection<TFilter, TItem> : UniquesCollection<TFilter>
        where TFilter : class, ISetFilter<TItem>
    {
        /// <summary>
        /// Does the specified item pass all filters in this collection?
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Pass(TItem item)
        {
            foreach (TFilter filter in this)
            {
                if (!filter.Pass(item)) return false;
            }
            return true;
        }
    }
}

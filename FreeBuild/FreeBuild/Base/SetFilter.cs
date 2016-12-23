using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract base class for conditional filters which can be applied to collections
    /// in order to 
    /// </summary>
    public abstract class SetFilter<TItem> : Unique
    {
        public abstract bool Pass(TItem item);
    }
}

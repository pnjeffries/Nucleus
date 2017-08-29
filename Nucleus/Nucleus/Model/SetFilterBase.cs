using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract generic base class for set filters
    /// </summary>
    public abstract class SetFilterBase<TItem> : Unique, ISetFilter<TItem>
    {
        #region Properties

        private bool _Invert = false;

        /// <summary>
        /// Inversion toggle.  If set to true, those items which would
        /// normally pass the filter will fail and vise versa.
        /// </summary>
        public bool Invert
        {
            get { return _Invert; }
            set { ChangeProperty(ref _Invert, value, "Invert"); }
        }

        #endregion

        /// <summary>
        /// Does the specified item pass through this filter?
        /// </summary>
        /// <param name="item">The item to be tested</param>
        /// <returns>True if the item passes through the filter, false if not.</returns>
        public abstract bool Pass(TItem item);
    }
}

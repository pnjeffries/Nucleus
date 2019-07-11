using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Base class for set filters which pass or reject objects based on whether
    /// their properties match a specified pre-set value
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public abstract class PropertyMatchFilter<TItem, TValue> : SetFilterBase<TItem>
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Value property
        /// </summary>
        private TValue _Value;

        /// <summary>
        /// The value to match against.
        /// </summary>
        public TValue Value
        {
            get { return _Value; }
            set { ChangeProperty(ref _Value, value, "Value"); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the value of the property to be compared with the preset
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract TValue GetItemPropertyValue(TItem item);

        // <summary>
        /// Does the specified item pass through this filter?
        /// </summary>
        /// <param name="item">The item to be tested</param>
        /// <returns>True if the item passes through the filter, false if not.</returns>
        public override bool Pass(TItem item)
        {
            return GetItemPropertyValue(item).Equals(Value) != Invert;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A collection of ParameterGroups.
    /// Each group within this collection must have a unique name.
    /// </summary>
    [Serializable]
    public class ParameterGroupCollection : ObservableKeyedCollection<string, ParameterGroup>
    {

        #region Methods

        /// <summary>
        /// Get the key for the specified group
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(ParameterGroup item)
        {
            return item.Name;
        }

        #endregion
    }
}

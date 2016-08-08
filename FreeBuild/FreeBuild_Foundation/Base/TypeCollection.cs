using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Extensions;

namespace FreeBuild.Base
{
    /// <summary>
    /// A keyed collection of types.
    /// </summary>
    public class TypeCollection : ObservableKeyedCollection<Guid, Type>
    {
        #region Methods

        protected override Guid GetKeyForItem(Type item)
        {
            return item.GUID;
        }

        /// <summary>
        /// Find the type in this set of types which is the least number of
        /// inheritance levels above the specified type.
        /// </summary>
        /// <param name="forType">The type to seach for</param>
        /// <returns>The type in this collection that is closest in the inheritance
        /// hierarchy to the specified type.  Or, null if the type does not have an
        /// ancestor in the collection.</returns>
        public Type ClosestAncestor(Type forType)
        {
            return TypeExtensions.ClosestAncestor(Items, forType);
        }

        #endregion
    }
}

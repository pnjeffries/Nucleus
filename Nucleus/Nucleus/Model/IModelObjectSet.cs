using Nucleus.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// An interface for sets of Model Objects
    /// </summary>
    public interface IModelObjectSet : INamed, IUnique
    {
        /// <summary>
        /// Get the final set of items contained within this set, consisting of all items in the base collection
        /// and any subsets (or all items in the model if 'All' is true) that pass all filters specified via the
        /// Filters property.
        /// </summary>
        IList GetItems();

        /// <summary>
        /// Add an item to the base collection of this set.
        /// If the specified item is not a valid type for this set, adding it will
        /// fail and this function will return false.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Add(ModelObject item);

        /// <summary>
        /// Does this set contain the specified item?
        /// (or, would it, if they were part of the same model?)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(ModelObject item);
    }
}

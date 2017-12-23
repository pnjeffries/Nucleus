using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A parametrically-defined set of Linear elements.  
    /// Allows linear element collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    [Serializable]
    public class LinearElementSet : ElementSet<LinearElement>
    {
        /// <summary>
        /// Create a new blank element set
        /// </summary>
        public LinearElementSet() : base() { }

        /// <summary>
        /// Create a new 'all elements' set
        /// </summary>
        /// <param name="all"></param>
        public LinearElementSet(bool all) : base(all) { }

        /// <summary>
        /// Initialise this set to contain a single item
        /// </summary>
        /// <param name="item"></param>
        public LinearElementSet(LinearElement item) : base(item) { }

        /// <summary>
        /// Initialise this set to contain the specified base collection of items
        /// </summary>
        /// <param name="collection"></param>
        public LinearElementSet(ElementCollection collection) : base(collection) { }

        /// <summary>
        /// Initialise a set to contain all elements in the model to which it
        /// belongs which are assigned the specified section.
        /// </summary>
        /// <param name="section"></param>
        public LinearElementSet(SectionFamily section) : base(section) { }

        #region Methods

        /// <summary>
        /// Set this set to contain only the specified items.
        /// All existing items, filters etc. in this set will be removed.
        /// </summary>
        /// <param name="items"></param>
        public void Set(LinearElementCollection items)
        {
            Clear();
            Add(items);
        }

        // <summary>
        /// Add a collection of items to the base collection of this set, to be considered for
        /// inclusion.  Note that adding an item to this set does not guarantee its inclusion 
        /// should said item fail to pass any of the specified set filters.
        /// </summary>
        /// <param name="items"></param>
        public void Add(LinearElementCollection items)
        {
            BaseCollection.TryAddRange(items);
        }

        #endregion
    }
}

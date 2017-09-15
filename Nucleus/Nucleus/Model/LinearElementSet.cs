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
    }
}

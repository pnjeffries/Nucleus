using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A load case multiplied by a scalar factor
    /// </summary>
    [Serializable]
    public class FactoredLoadCase : FactoredCase<LoadCase>
    {
        #region Constructors

        /// <summary>
        /// Initialise an empty factored case.
        /// </summary>
        public FactoredLoadCase() : base() { }

        /// <summary>
        /// Initialise a new FactoredCase with a factor of 1 and the
        /// speciifed case.
        /// </summary>
        /// <param name="lCase"></param>
        public FactoredLoadCase(LoadCase lCase) : base(lCase) { }

        /// <summary>
        /// Initialise a new FactoredCase with the specifed factor
        /// and case.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="lCase"></param>
        public FactoredLoadCase(double factor, LoadCase lCase) : base(factor, lCase) { }

        #endregion
    }
}

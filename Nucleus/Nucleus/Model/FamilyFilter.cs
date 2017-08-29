using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Generic base for filters that pass or fail based 
    /// on the assigned family of an element
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TFamily"></typeparam>
    public abstract class FamilyFilter<TElement, TFamily> : SetFilterBase<TElement>
        where TElement : Element
        where TFamily : Family
    {
        #region Properties

        /// <summary>
        /// Private backing field for Family property
        /// </summary>
        private TFamily _Family;

        /// <summary>
        /// The family to filter by
        /// </summary>
        public TFamily Family
        {
            get { return _Family; }
            set { _Family = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Does the specified item pass through this filter?
        /// </summary>
        /// <param name="item">The item to be tested</param>
        /// <returns>True if the item passes through the filter, false if not.</returns>
        public override bool Pass(TElement item)
        {
            return (item.GetFamily() == Family) != Invert;
        }

        #endregion
    }

    /// <summary>
    /// Filter for elements which will pass if the element is assigned
    /// a specific family
    /// </summary>
    public class FamilyFilter : FamilyFilter<Element, Family>
    {
        #region Constructors

        /// <summary>
        /// Initialise a new blank FamilyFilter
        /// </summary>
        public FamilyFilter() { }

        /// <summary>
        /// Initialise a new FamilyFilter to pass elements
        /// with the specified family
        /// </summary>
        /// <param name="family"></param>
        /// <param name="invert"></param>
        public FamilyFilter(Family family, bool invert = false)
        {
            Family = family;
            Invert = invert;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of linear elements
    /// </summary>
    public class LinearElementCollection : ElementCollection<LinearElement, LinearElementCollection>
    {
        #region Properties

        /// <summary>
        /// Set or set the combined value of the section properties of the elsments
        /// within this collection.
        /// </summary>
        public SectionProperty Property
        {
            get { return (SectionProperty)CombinedValue(i => i.Property, null); }
            set
            {
                foreach (LinearElement lEl in this) lEl.Property = value;
            }
        }

        /// <summary>
        /// The set of sections which are available to be assigned to the elements
        /// in this collection.
        /// </summary>
        public SectionPropertyCollection AvailableSections
        {
            get
            {
                if (Count > 0)
                {
                    return this[0].Model?.Properties.Sections;
                }
                else return null;
            }
        }

        #endregion
    }
}

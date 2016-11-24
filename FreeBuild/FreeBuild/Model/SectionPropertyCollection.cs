using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of Section Properties
    /// </summary>
    [Serializable]
    public class SectionPropertyCollection : ModelObjectCollection<SectionProperty>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SectionPropertyCollection() : base() { }

        /// <summary>
        /// Initialise a new SectionPropertyCollection containing the specified set of sections
        /// </summary>
        /// <param name="sections"></param>
        public SectionPropertyCollection(IEnumerable<SectionProperty> sections)
        {
            foreach (SectionProperty section in sections)
            {
                Add(section);
            }
        }

        #endregion
    }
}

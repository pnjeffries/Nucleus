using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// An extended VolumetricPropertyCollection that also contains additional
    /// temporary data structures for fast lookup operations.
    /// </summary>
    [Serializable]
    public class VolumetricPropertyTable : VolumetricPropertyCollection
    {
        #region Properties

        /// <summary>
        /// Private backing field for Sections property
        /// </summary>
        private SectionPropertyCollection _Sections = null;

        /// <summary>
        /// The subset of section properties in this table.
        /// Generated when necessary and cached.  Do not modify - modifications to this
        /// collection will have no effect on the source table.
        /// </summary>
        public SectionPropertyCollection Sections
        {
            get
            {
                if (_Sections == null) _Sections = GetSections();
                return _Sections;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new VolumetricPropertyTable belonging to the specified model
        /// </summary>
        /// <param name="model"></param>
        public VolumetricPropertyTable(Model model) : base(model) { }

        #endregion

        #region Methods

        protected override void OnCollectionChanged()
        {
            //Clear cached data:
            _Sections = null;
        }

        #endregion
    }
}

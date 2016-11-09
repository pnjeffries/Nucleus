using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of VolumetricProperty objects
    /// </summary>
    [Serializable]
    public class VolumetricPropertyCollection : ModelObjectCollection<VolumetricProperty>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public VolumetricPropertyCollection() : base() { }

        /// <summary>
        /// Owner constructor
        /// </summary>
        /// <param name="model"></param>
        protected VolumetricPropertyCollection(Model model) : base(model) { }

        #endregion

        #region Methods

        /// <summary>
        /// Extract all Section Properties from this collection
        /// </summary>
        /// <returns></returns>
        public SectionPropertyCollection GetSections()
        {
            var result = new SectionPropertyCollection();
            foreach (VolumetricProperty vProp in this)
            {
                if (vProp is SectionProperty)
                    result.Add((SectionProperty)vProp);
            }
            return result;
        }

        #endregion
    }
}

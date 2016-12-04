using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.UI
{
    /// <summary>
    /// Attribute for automatic UI generation.  Add this attribute to properties
    /// for which you wish to have UI controls generated automatically.
    /// </summary>
    public class AutoUIAttribute : Attribute
    {
        #region properties

        /// <summary>
        /// The order weighting for this property.  Those with a lower order
        /// weighting will be displayed first.
        /// </summary>
        public double Order { get; set; } = 0;

        /// <summary>
        /// The text that the field should be labelled with.  If not set,
        /// the property name itself will be used.
        /// </summary>
        public string Label { get; set; } = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoUIAttribute() { }

        /// <summary>
        /// Order constructor
        /// </summary>
        /// <param name="order"></param>
        public AutoUIAttribute(double order)
        {
            Order = order;
        }

        #endregion
    }
}

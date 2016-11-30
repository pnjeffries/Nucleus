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
    /// Properties tagged with this attribute will be represented by a ComboBox.
    /// </summary>
    public class AutoUIComboBoxAttribute : AutoUIAttribute
    {
        #region Properties

        /// <summary>
        /// The path of the property to use for the items source of the combobox
        /// </summary>
        public string ItemsSource { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemsSource"></param>
        public AutoUIComboBoxAttribute(string itemsSource):base()
        {
            ItemsSource = itemsSource;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemsSource"></param>
        public AutoUIComboBoxAttribute(double order, string itemsSource):base(order)
        {
            ItemsSource = itemsSource;
        }


        #endregion
    }
}

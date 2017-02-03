using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FreeBuild.WPF
{
    /// <summary>
    /// A DataTemplateSelector which bases the data template to be used on whether
    /// or not an item is null
    /// </summary>
    public class IsNullDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// The DataTemplate to use when the value is not null
        /// </summary>
        public DataTemplate NotNullTemplate{ get; set; }

        /// <summary>
        /// The DataTemplate to use when the value is null
        /// </summary>
        public DataTemplate NullTemplate { get; set; }

        /// <summary>
        /// SelectTemplate override
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return NullTemplate;
            else return NotNullTemplate;
        }
    }
}

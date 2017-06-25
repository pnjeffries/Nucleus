using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nucleus.WPF.Extensions
{
    /// <summary>
    /// Extension methods for AutoUIComboBoxAttribute objects
    /// </summary>
    public static class AutoUIComboBoxAttributeExtensions
    {
        /// <summary>
        /// Get the compositecollection detailing the full set of available source items
        /// specified by this attribute
        /// </summary>
        /// <param name="cBA"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static CompositeCollection GetCombinedSourceCollection(this AutoUIComboBoxAttribute cBA, object source)
        {
            CompositeCollection collection = new CompositeCollection();
            var c1 = new CollectionContainer();
            var b1 = new Binding("DataContext." + cBA.ItemsSource);
            b1.Source = source;
            BindingOperations.SetBinding(c1, CollectionContainer.CollectionProperty, b1);
            collection.Add(c1);

            var c2 = new CollectionContainer();
            var b2 = new Binding("DataContext." + cBA.ExtraItemsSource);
            b2.Source = source;
            BindingOperations.SetBinding(c2, CollectionContainer.CollectionProperty, b2);
            collection.Add(c2);

            return collection;
        }
    }
}

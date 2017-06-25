using Nucleus.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for SelectItemsDialog.xaml
    /// </summary>
    public partial class SelectItemsDialog : Window
    {
        #region Properties

        /// <summary>
        /// ItemsSource dependency property
        /// </summary>
        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(SelectItemsDialog));

        /// <summary>
        /// The source of the items that can be selected from
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(SelectItemsDialog));

        /// <summary>
        /// The selection mode of the dialog
        /// </summary>
        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// The items selected in the dialog
        /// </summary>
        public IList SelectedItems
        {
            get { return ListBox.SelectedItems; }
        }

        /// <summary>
        /// The item selected in the dialog
        /// </summary>
        public object SelectedItem
        {
            get { return ListBox.SelectedItem; }
        }

        #endregion

        #region Constructor

        public SelectItemsDialog(string title, ICollection items, SelectionMode selectionMode = SelectionMode.Single)
        {
            InitializeComponent();

            Title = title;
            LayoutRoot.DataContext = this;
            ItemsSource = items;
            SelectionMode = selectionMode;
        }

        #endregion

        #region Methods

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Use a SelectItemsDialog to choose a single item from the specified collection
        /// </summary>
        /// <typeparam name="TItem">The type of the item to choose</typeparam>
        /// <param name="caption"></param>
        /// <param name="items"></param>
        /// <returns>The selected item if one is chosen and the dialog is not cancelled, else null</returns>
        public static TItem SelectItem<TItem>(string caption, ICollection<TItem> items)
            where TItem : class, INamed
        {
            var dialog = new SelectItemsDialog(caption, (ICollection)items);
            if (dialog.ShowDialog() == true)
            {
                return dialog.SelectedItem as TItem;
            }
            return null;
        }

        /// <summary>
        /// Use a SelectItemsDialog to choose a subset of items from the specified collection
        /// </summary>
        /// <typeparam name="TCollection"></typeparam>
        /// <param name="caption"></param>
        /// <param name="sourceItems"></param>
        /// <returns></returns>
        public static TCollection SelectItems<TCollection>(string caption, TCollection sourceItems)
            where TCollection : class, IList, ICollection, new()
        {
            var dialog = new SelectItemsDialog(caption, sourceItems, SelectionMode.Multiple);
            if (dialog.ShowDialog() == true)
            {
                TCollection result = new TCollection();
                foreach (object item in dialog.SelectedItems)
                {
                    result.Add(item);
                }
                return result;
            }
            return null;
        }

        #endregion
    }
}

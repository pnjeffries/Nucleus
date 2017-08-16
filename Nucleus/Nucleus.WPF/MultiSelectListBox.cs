using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nucleus.WPF
{
    /// <summary>
    /// A list box with (two-way) bindable Selected items
    /// </summary>
    public class MultiSelectListBox : ListBox
    {
        #region Properties

        public static readonly DependencyProperty SelectionProperty =
            DependencyProperty.Register("Selection", typeof(IList), typeof(MultiSelectListBox));

        /// <summary>
        /// The currently selected items.
        /// Essentially replicates SelectedItems, but with two-way binding.
        /// </summary>
        public IList Selection
        {
            get { return (IList)GetValue(SelectionProperty); }
            set { SetValue(SelectionProperty, value); }
        }

        #endregion

        #region Constructors

        public MultiSelectListBox()
        {
            SelectionChanged += MultiSelectListBox_SelectionChanged;
            SelectionMode = SelectionMode.Multiple;
        }

        private void MultiSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == Selection.Count) Selection.Clear();
            else foreach (var item in e.RemovedItems) Selection.Remove(item);

            foreach (var item in e.AddedItems)
                if (!Selection.Contains(item))
                    Selection.Add(item);
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        #region Fields

        /// <summary>
        /// Flag that is used to prevent infinite back-and-forth between SelectedItems
        /// and the bound Selection's modification events
        /// </summary>
        private bool _LockSelectionUpdates = false;

        #endregion

        #region Properties

        private static void OnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiSelectListBox)d).RegisterNewSelectionEvents(e.NewValue, e.OldValue);
        }

        public static readonly DependencyProperty SelectionProperty =
            DependencyProperty.Register("Selection", typeof(IList), typeof(MultiSelectListBox),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnSelectionChanged)));

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

        private void RegisterNewSelectionEvents(object newValue, object oldValue)
        {
            if (oldValue is INotifyCollectionChanged)
            {
                var oldNCC = (INotifyCollectionChanged)oldValue;
                oldNCC.CollectionChanged -= Selection_CollectionChanged;
            }

            if (newValue is INotifyCollectionChanged)
            {
                var newNCC = (INotifyCollectionChanged)newValue;
                newNCC.CollectionChanged += Selection_CollectionChanged;
            }
        }

        private void Selection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_LockSelectionUpdates)
            {
                _LockSelectionUpdates = true;
                if (e.Action == NotifyCollectionChangedAction.Reset) SelectedItems.Clear();
                if (e.OldItems != null)
                {
                    if (e.OldItems.Count == SelectedItems.Count) SelectedItems.Clear();
                    else foreach (var item in e.OldItems) SelectedItems.Remove(item);
                }
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                        if (!SelectedItems.Contains(item))
                            SelectedItems.Add(item);
                }
                _LockSelectionUpdates = false;
            }
        }

        private void MultiSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_LockSelectionUpdates)
            {
                _LockSelectionUpdates = true;
                if (e.RemovedItems.Count == Selection.Count) Selection.Clear();
                else foreach (var item in e.RemovedItems) Selection.Remove(item);

                foreach (var item in e.AddedItems)
                    if (!Selection.Contains(item))
                        Selection.Add(item);
                _LockSelectionUpdates = false;
            }
        }

        #endregion
    }
}

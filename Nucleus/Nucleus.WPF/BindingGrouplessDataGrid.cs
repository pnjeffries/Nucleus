using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nucleus.WPF
{
    /// <summary>
    /// A datagrid that does not use binding groups - source changes will be made immediately rather than when the
    /// user leaves the row
    /// </summary>
    public class BindingGrouplessDataGrid : DataGrid
    {
        /// <summary>
        /// Override to clear binding groups from this grid when something is changed
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = base.MeasureOverride(availableSize);
            ClearBindingGroups();
            return desiredSize;
        }

        /// <summary>
        /// Clear the binding groups from all rows
        /// </summary>
        private void ClearBindingGroups()
        {
            //Clear ItemBindingGroup so it isn't applied to new rows:
            ItemBindingGroup = null;
            //Clear BindingGroup on already created rows
            foreach (var item in Items)
            {
                var row = ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (row != null) row.BindingGroup = null;
            }
        }
    }
}

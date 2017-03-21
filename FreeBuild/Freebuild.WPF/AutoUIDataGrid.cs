using FreeBuild.Extensions;
using FreeBuild.UI;
using FreeBuild.WPF.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace FreeBuild.WPF
{
    public class AutoUIDataGrid : DataGrid
    {
        public AutoUIDataGrid() : base()
        {
            AutoGenerateColumns = false; // Do not want to use standard column generation logic
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (AutoGenerateColumns == false)
                GenerateAutoUIColumns();
        }

        protected void GenerateAutoUIColumns()
        {
            if (ItemsSource != null)
            {
                Type collectionType = ItemsSource.GetType();
                Type type = collectionType.GetItemType();
                GenerateAutoUIColumns(type);
            }
        }

        protected void GenerateAutoUIColumns(Type type)
        {
            IList<PropertyInfo> properties = type.GetAutoUIProperties();
            GenerateAutoUIColumns(properties);
        }

        protected void GenerateAutoUIColumns(IList<PropertyInfo> properties)
        {
            Columns.Clear(); // Clear previous columns

            foreach (PropertyInfo property in properties)
            {
                Binding binding = new Binding(property.Name);
                DataGridColumn column;
                AutoUIAttribute aUI = property.GetCustomAttribute<AutoUIAttribute>(); ;
                if (property.HasAttribute(typeof(AutoUIComboBoxAttribute)))
                {
                    AutoUIComboBoxAttribute cBA = property.GetCustomAttribute<AutoUIComboBoxAttribute>();
                    var comboColumn = new DataGridComboBoxColumn();
                    column = comboColumn;
                    comboColumn.SelectedItemBinding = binding;
                    //TODO: Set ItemsSource binding
                    Binding sourceBinding;
                    if (string.IsNullOrEmpty(cBA.ItemsSource))
                         sourceBinding = new Binding(cBA.ItemsSource);
                    else
                    {
                        sourceBinding = new Binding();
                        sourceBinding.Converter = new ModelTableConverter();
                        sourceBinding.ConverterParameter = property.PropertyType;
                    }
                    BindingOperations.SetBinding(comboColumn, DataGridComboBoxColumn.ItemsSourceProperty,
                        sourceBinding);
                }
                else
                {
                    var textColumn = new DataGridTextColumn();
                    column = textColumn;
                    textColumn.Binding = binding;
                }
                if (aUI?.Label != null) column.Header = aUI.Label;
                else column.Header = property.Name.AutoSpace();

                Columns.Add(column);
            }

        }
    }
}

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
        private bool _GenerateSubTypeColumns = true;

        /// <summary>
        /// If set to true, columns will be generated for all detected sub-types
        /// of the items source type as well as the base.
        /// </summary>
        public bool GenerateSubTypeColumns
        {
            get { return _GenerateSubTypeColumns; }
            set { _GenerateSubTypeColumns = value; }
        }

        #region Constructors

        public AutoUIDataGrid() : base()
        {
            AutoGenerateColumns = false; // Do not want to use standard column generation logic
        }

        #endregion

        #region Methods

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
            if (GenerateSubTypeColumns)
            {
                IList<Type> subTypes = type.GetSubTypes();
                IList<PropertyInfo> properties = subTypes.GetAutoUIProperties();
                GenerateAutoUIColumns(properties);
            }
            else
            {
                IList<PropertyInfo> properties = type.GetAutoUIProperties();
                GenerateAutoUIColumns(properties);
            }
        }

        protected void GenerateAutoUIColumns(IList<PropertyInfo> properties)
        {
            Columns.Clear(); // Clear previous columns
            // TODO: Keep manually-added columns?

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

                    // Set ItemsSource binding:
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

        #endregion
    }
}

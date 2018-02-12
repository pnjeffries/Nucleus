using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Model;
using Nucleus.UI;
using Nucleus.WPF.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Nucleus.WPF
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
                IList<Type> subTypes = type.GetSubTypes(false, true);
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
            Model.Model model = null;
            if (ItemsSource != null && ItemsSource is IOwned<Model.Model>)
            {
                model = ((IOwned<Model.Model>)ItemsSource).Owner;
            }

            foreach (PropertyInfo property in properties)
            {
                Binding binding = new Binding(property.Name);
                binding.Converter = new TextConverter(model);
                if (!property.CanWrite) binding.Mode = BindingMode.OneWay;
                DataGridColumn column;
                AutoUIAttribute aUI = property.GetCustomAttribute<AutoUIAttribute>(); ;
                if (property.HasAttribute(typeof(AutoUIComboBoxAttribute)))
                {
                    AutoUIComboBoxAttribute cBA = property.GetCustomAttribute<AutoUIComboBoxAttribute>();
                    /*var comboColumn = new DataGridComboBoxColumn();
                    column = comboColumn;
                    comboColumn.SelectedItemBinding = binding;*/

                    /*BindingOperations.SetBinding(comboColumn, DataGridComboBoxColumn.ItemsSourceProperty,
                        sourceBinding);*/

                    var comboColumn = new DataGridTemplateColumn();
                    column = comboColumn;

                    var cellTemplate = new DataTemplate();
                    var tbFactory = new FrameworkElementFactory(typeof(TextBlock));
                    tbFactory.SetBinding(TextBlock.TextProperty, binding);
                    //tbFactory.SetValue(TextBlock.PaddingProperty, new Thickness(2.0,2.0,2.0,2.0));
                    cellTemplate.VisualTree = tbFactory;
                    comboColumn.CellTemplate = cellTemplate;

                    var editTemplate = new DataTemplate();
                    var cbFactory = new FrameworkElementFactory(typeof(ComboBox));
                    cbFactory.SetValue(ComboBox.IsTextSearchEnabledProperty, true);
                    cbFactory.SetValue(ComboBox.IsEditableProperty, true);
                    cbFactory.SetValue(ComboBox.PaddingProperty, new Thickness(2.0, 0.0, 2.0, 0.0));
                    //cbFactory.SetBinding(ComboBox.TextProperty, binding);

                    // Set ItemsSource binding:
                    Binding sourceBinding;
                    if (!string.IsNullOrEmpty(cBA.ItemsSource))
                    {
                        sourceBinding = new Binding();
                        sourceBinding.Path = new PropertyPath(cBA.ItemsSource);
                    }
                    else
                    {
                        sourceBinding = new Binding();
                        if (property.PropertyType.IsEnum)
                        {
                            sourceBinding.Source = Enum.GetValues(property.PropertyType);
                        }
                        else if (typeof(Family).IsAssignableFrom(property.PropertyType))
                        {
                            sourceBinding.Path = new PropertyPath("Model.Families");
                        }
                        //sourceBinding.Converter = new ModelTableConverter();
                        //sourceBinding.ConverterParameter = property.PropertyType;
                    }

                    cbFactory.SetBinding(ComboBox.SelectedItemProperty, new Binding(property.Name));
                    cbFactory.SetBinding(ComboBox.ItemsSourceProperty, sourceBinding);

                    editTemplate.VisualTree = cbFactory;
                    comboColumn.CellEditingTemplate = editTemplate;
                }
                else
                {
                    var textColumn = new DataGridTextColumn();
                    column = textColumn;
                    textColumn.Binding = binding;
                }
                if (aUI?.Label != null) column.Header = aUI.Label;
                else column.Header = property.Name.AutoSpace();

                column.MinWidth = 100;

                Columns.Add(column);
            }

        }

        #endregion
    }
}

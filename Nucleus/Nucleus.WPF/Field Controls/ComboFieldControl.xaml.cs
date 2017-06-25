using Nucleus.Base;
using Nucleus.Rendering;
using Nucleus.UI;
using Nucleus.WPF.Extensions;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for ComboFieldControl.xaml
    /// </summary>
    public partial class ComboFieldControl : FieldControl
    {
        #region Properties

        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ComboFieldControl));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ComboFieldControl));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboFieldControl));

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        public static DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ComboFieldControl));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty TextSearchPathProperty =
            DependencyProperty.Register("TextSearchPath", typeof(string), typeof(ComboFieldControl));

        public string TextSearchPath
        {
            get { return (string)GetValue(TextSearchPathProperty); }
            set { SetValue(TextSearchPathProperty, value); }
        }

        #endregion

        #region Constructors

        public ComboFieldControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }

        #endregion

        #region Methods

        public override void AdaptTo(PropertyInfo property)
        {
            base.AdaptTo(property);
            if (property.PropertyType.IsEnum)
            {
                ItemsSource = Enum.GetValues(property.PropertyType);
            }
            else
            {
                AutoUIComboBoxAttribute cBA = property.GetCustomAttribute<AutoUIComboBoxAttribute>();
                
                if (cBA != null)
                {
                    if (string.IsNullOrWhiteSpace(cBA.ExtraItemsSource))
                        SetBinding(ItemsSourceProperty, cBA.ItemsSource);
                    else
                    {
                        // Bind to both the standard and extra item sources
                        ItemsSource = cBA.GetCombinedSourceCollection(this);
                    }
                }
                ItemTemplate = new DataTemplate();
                FrameworkElementFactory fEFactory = new FrameworkElementFactory(typeof(TextBlock));

                if (typeof(INamed).IsAssignableFrom(property.PropertyType))
                {
                    string textTemplate = "Name";
                    fEFactory.SetBinding(TextBlock.TextProperty, new Binding(textTemplate));
                }
                else
                {
                   
                    if (typeof(Colour).IsAssignableFrom(property.PropertyType))
                    {
                        fEFactory = new FrameworkElementFactory(typeof(Border));

                        Binding binding = new Binding();
                        binding.Converter = new Converters.BrushConverter();
                        fEFactory.SetBinding(Border.BackgroundProperty, binding);
                        fEFactory.SetValue(Border.HeightProperty, 16d);
                        fEFactory.SetValue(Border.WidthProperty, 16d);
                    }
                    else
                    {
                        fEFactory.SetBinding(TextBlock.TextProperty, new Binding());
                    }
                }

                ItemTemplate.VisualTree = fEFactory;
            }
        }

        #endregion

    }
}

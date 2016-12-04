using FreeBuild.Extensions;
using FreeBuild.Geometry;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FB = FreeBuild.Geometry;

namespace FreeBuild.WPF
{
    /// <summary>
    /// A stack panel that generates a UI interface for marked properties
    /// of a type.
    /// </summary>
    public class AutoPropertiesPanel : StackPanel
    {
        #region Properties

        public static readonly DependencyProperty MyDataContextProperty =
        DependencyProperty.Register("MyDataContext",
                                    typeof(Object),
                                    typeof(AutoPropertiesPanel),
                                    new PropertyMetadata(MyDataContextChanged));

        private static void MyDataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            AutoPropertiesPanel myControl = (AutoPropertiesPanel)sender;
            myControl.Refresh();
        }

        public object MyDataContext
        {
            get { return GetValue(MyDataContextProperty); }
            set { SetValue(MyDataContextProperty, value); }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPropertiesPanel() : base()
        {
           Initialise();
        }

        #endregion

        #region Methods



        /// <summary>
        /// Initialise this panel
        /// </summary>
        protected void Initialise()
        {
            SetBinding(MyDataContextProperty, new Binding());
        }

        /// <summary>
        /// Called when the DataContext is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoPropertiesPanel_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Refresh the fields displayed by this panel
        /// </summary>
        public void Refresh()
        {
            Children.Clear();
            if (DataContext != null)
            {
                GenerateFieldsFor(DataContext.GetType());
            }
        }

        /// <summary>
        /// Populate this panel with controls for the specified type
        /// </summary>
        /// <param name="type"></param>
        protected void GenerateFieldsFor(Type type)
        {
            IList<PropertyInfo> properties = type.GetAutoUIProperties();
            GenerateFieldsFor(properties);
        }

        /// <summary>
        /// Populate this panel with controls for the specified list of properties
        /// </summary>
        /// <param name="properties"></param>
        protected void GenerateFieldsFor(IList<PropertyInfo> properties)
        { 
            foreach (PropertyInfo property in properties)
            {
                FieldControl control = null;
                Type pType = property.PropertyType;
                if (property.HasAttribute(typeof(AutoUIComboBoxAttribute)))
                {
                    control = new ComboFieldControl();
                }
                else if (pType.IsAssignableFrom(typeof(double))) //Numbers
                {
                    control = new SliderFieldControl();
                }
                else if (pType == typeof(bool))
                {
                    control = new CheckBoxFieldControl();
                }
                else if (pType == typeof(FB.Vector)) //Vectors
                {
                    control = new VectorFieldControl();
                }
                else if (pType.IsEnum)
                {
                    control = new ComboFieldControl();
                }
                else //Everything else!
                {
                    //TODO: Determine
                    control = new TextFieldControl();
                }

                if (control != null)
                {
                    control.AdaptTo(property);
                    Children.Add(control);
                }
            }
        }

        #endregion

    }
}

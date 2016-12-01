using FreeBuild.Extensions;
using FreeBuild.Geometry;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FreeBuild.WPF
{
    /// <summary>
    /// A stack panel that generates a UI interface for marked properties
    /// of a type.
    /// </summary>
    public class AutoPropertiesPanel : StackPanel
    {
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
            this.DataContextChanged += AutoPropertiesPanel_DataContextChanged;
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
                Type pType = property.GetType();
                if (property.HasAttribute(typeof(AutoUIComboBoxAttribute)))
                {
                    control = new ComboFieldControl();
                }
                if (pType.IsAssignableFrom(typeof(double))) //Numbers
                {
                    control = new SliderFieldControl();
                }
                else if (pType == typeof(Vector)) //Vectors
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

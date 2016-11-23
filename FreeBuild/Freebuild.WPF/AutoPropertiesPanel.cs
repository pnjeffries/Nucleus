using FreeBuild.Extensions;
using FreeBuild.Geometry;
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

        #region Methods

        /// <summary>
        /// Populate this panel with controls for the specified type
        /// </summary>
        /// <param name="type"></param>
        public void GenerateFieldsFor(Type type)
        {
            IList<PropertyInfo> properties = type.GetAutoUIProperties();
            foreach (PropertyInfo property in properties)
            {
                FieldControl control = null;
                Type pType = property.GetType();
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

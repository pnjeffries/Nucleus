using FreeBuild.Extensions;
using FreeBuild.Geometry;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using FB = FreeBuild.Geometry;

namespace FreeBuild.WPF
{
    /// <summary>
    /// Extension methods for UIElementCollection
    /// </summary>
    public static class UIElementCollectionExtensions
    {
        /// <summary>
        /// Populate this UIElementCollection with automatically generated controls for the specified type.
        /// </summary>
        /// <param name="type"></param>
        public static void GenerateControlsFor(this UIElementCollection collection, object source)
        {
            collection.GenerateControlsFor(source, source.GetType().GetAutoUIMembers());
        }

        /// <summary>
        /// Populate this UIElementCollection with automatically generated controls for the specified list of members.
        /// Annotate the properties with the AutoUIAttribute to control how this is done.
        /// </summary>
        /// <param name="members"></param>
        public static void GenerateControlsFor(this UIElementCollection collection, object source, IList<MemberInfo> members)
        {
            foreach (MemberInfo member in members)
            {
                if (member is PropertyInfo)
                {
                    // Properties
                    PropertyInfo property = (PropertyInfo)member;
                    FieldControl control = null;
                    Type pType = property.PropertyType;
                    if (property.HasAttribute(typeof(AutoUIComboBoxAttribute))) // Combo box attributes
                    {
                        control = new ComboFieldControl();
                    }
                    else if (pType.IsAssignableFrom(typeof(double))) // Numbers
                    {
                        control = new SliderFieldControl();
                    }
                    else if (pType == typeof(Angle)) // Angles
                    {
                        control = new SliderFieldControl();
                    }
                    else if (pType == typeof(bool)) // Booleans
                    {
                        control = new CheckBoxFieldControl();
                    }
                    else if (pType == typeof(FB.Vector)) // Vectors
                    {
                        control = new VectorFieldControl();
                    }
                    else if (pType.IsEnum) // Enums
                    {
                        control = new ComboFieldControl();
                    }
                    //else if (pType == typeof(string) && !property.CanWrite) // Text block
                    //{
                    //    var tb = new TextBlock();
                    //    tb.SetBinding(TextBlock.TextProperty, new Binding(property.Name));
                    //    collection.Add(tb);
                    //}
                    else // Everything else!
                    {
                        control = new TextFieldControl();
                    }

                    if (control != null)
                    {
                        control.AdaptTo(property);
                        collection.Add(control);
                    }

                }
                else if (member is MethodInfo)
                {
                    Button button = new Button();
                    button.Command = new InvokeMethodCommand(source, (MethodInfo)member);
                    AutoUIAttribute autoAtt = member.GetCustomAttribute<AutoUIAttribute>();
                    if (autoAtt != null)
                    {
                        // Use attribute-defined label if present:
                        if (autoAtt != null && !string.IsNullOrWhiteSpace(autoAtt.Label))
                            button.Content = autoAtt.Label;
                        // Otherwise, use property name:
                        else button.Content = member.Name;

                        // Label binding:
                        if (!string.IsNullOrWhiteSpace(autoAtt?.LabelBinding))
                        {
                            var bind = new Binding(autoAtt?.LabelBinding);
                            //bind.FallbackValue = Label;
                            button.SetBinding(Button.ContentProperty, bind);
                        }

                        // Visibility
                        if (!string.IsNullOrWhiteSpace(autoAtt?.VisibilityBinding))
                        {
                            var vB = new Binding(autoAtt.VisibilityBinding);
                            vB.Converter = new Converters.VisibilityConverter();
                            button.SetBinding(Button.VisibilityProperty, vB);
                        }

                        // Tooltip
                        if (!string.IsNullOrWhiteSpace(autoAtt?.ToolTip))
                        {
                            button.ToolTip = autoAtt.ToolTip;
                        }
                    }
                    collection.Add(button);
                }
            }
        }

        
        
    }
}

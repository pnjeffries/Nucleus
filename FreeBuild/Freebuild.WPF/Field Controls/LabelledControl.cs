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

namespace FreeBuild.WPF
{
    /// <summary>
    /// Base class for user controls with a label dependency property
    /// </summary>
    public abstract class LabelledControl : UserControl
    {
        #region Properties

        /// <summary>
        /// Label dependency property
        /// </summary>
        public static readonly DependencyProperty LabelProperty
            = DependencyProperty.Register("Label", typeof(string), typeof(LabelledControl), new PropertyMetadata(null));

        /// <summary>
        /// The label text of the field
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Extra content dependency property
        /// </summary>
        public static readonly DependencyProperty ExtraContentProperty
            = DependencyProperty.Register("ExtraContent", typeof(object), typeof(LabelledControl));

        /// <summary>
        /// Extra content hosted as part of the field
        /// </summary>
        public object ExtraContent
        {
            get { return GetValue(ExtraContentProperty); }
            set { SetValue(ExtraContentProperty, value); }
        }

        /// <summary>
        /// Units dependency property
        /// </summary>
        public static readonly DependencyProperty UnitsProperty
            = DependencyProperty.Register("Units", typeof(string), typeof(LabelledControl));

        /// <summary>
        /// The units the displayed quantity is in
        /// </summary>
        public string Units
        {
            get { return (string)GetValue(UnitsProperty); }
            set { SetValue(UnitsProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set up this control to display the specified object property
        /// </summary>
        /// <param name="property"></param>
        public virtual void AdaptTo(PropertyInfo property)
        {
            
            AutoUIAttribute autoAtt = property.GetCustomAttribute<AutoUIAttribute>();

            // Use attribute-defined label if present:
            if (autoAtt != null && !string.IsNullOrWhiteSpace(autoAtt.Label))
                Label = autoAtt.Label;
            // Otherwise, use property name:
            else Label = property.Name;

            // Label binding:
            if (!string.IsNullOrWhiteSpace(autoAtt?.LabelBinding))
            {
                var bind = new Binding(autoAtt?.LabelBinding);
                //bind.FallbackValue = Label;
                SetBinding(LabelProperty, bind);
            }

            // Visibility
            if (!string.IsNullOrWhiteSpace(autoAtt?.VisibilityBinding))
            {
                var vB = new Binding(autoAtt.VisibilityBinding);
                vB.Converter = new Converters.VisibilityConverter();
                SetBinding(VisibilityProperty, vB);
            }

            // Tooltip
            if (!string.IsNullOrWhiteSpace(autoAtt?.ToolTip))
            {
                ToolTip = autoAtt.ToolTip;
            }
           
        }

        #endregion
    }
}

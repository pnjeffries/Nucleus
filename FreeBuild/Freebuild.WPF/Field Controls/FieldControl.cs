using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nucleus.WPF
{
    /// <summary>
    /// Base class for controls used to represent data fields
    /// </summary>
    public abstract class FieldControl : LabelledControl
    {
        #region Properties

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback ValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValueChanged?.Invoke(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FieldControl)d).RaiseValueChanged(d, e);
            ((FieldControl)d).OnValueChanged(e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register("Value", typeof(object), typeof(FieldControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnValueChanged)));

        /// <summary>
        /// The value displayed in the field
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Overridable member function called when the value of the control is changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
           // By default does nothing!
        }

        /// <summary>
        /// Set up this control to display the specified object property
        /// </summary>
        /// <param name="property"></param>
        public override void AdaptTo(PropertyInfo property)
        {
            base.AdaptTo(property);
            SetBinding(ValueProperty, property.Name);
        }


        #endregion
    }
}

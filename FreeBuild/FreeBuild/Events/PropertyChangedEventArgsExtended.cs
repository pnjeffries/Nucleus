using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Events
{
    /// <summary>
    /// Extended version of PropertyChangedEventArgs that also includes the previous and new
    /// values of the property
    /// </summary>
    public class PropertyChangedEventArgsExtended : PropertyChangedEventArgs
    {
        /// <summary>
        /// The previous value of the property
        /// </summary>
        public object OldValue { get; }

        /// <summary>
        /// The new value of the property
        /// </summary>
        public object NewValue { get; }

        public PropertyChangedEventArgsExtended(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}

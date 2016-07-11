using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Reusable base class that provides a basic implementation of the INotifyPropertyChanged interface.
    /// Raises a PropertyChanged event that is used to update bound WPF UI controls
    /// </summary>
    public abstract class NotifyPropertyChangedBase : EventBase, INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised when a property of this object is changed
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise a PropertyChanged event for the specified property name
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            RaiseEvent(PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raise an event
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="args"></param>
        private void RaiseEvent(PropertyChangedEventHandler handler, PropertyChangedEventArgs args)
        {
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}

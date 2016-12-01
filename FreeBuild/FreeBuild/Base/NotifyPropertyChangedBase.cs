// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
    [Serializable]
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
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raise an event.
        /// Checks for a null handler before raising.
        /// </summary>
        /// <param name="handler">The event handler</param>
        /// <param name="args">The event args</param>
        protected void RaiseEvent(PropertyChangedEventHandler handler, PropertyChangedEventArgs args)
        {
            handler?.Invoke(this, args);
        }

        /// <summary>
        /// Raise an event, passing through the original sender.
        /// Checks for a null handler before raising.
        /// Used to 'bubble' up events from sub-objects
        /// </summary>
        /// <param name="handler">The event handler</param>
        /// <param name="sender">The original sender object</param>
        /// <param name="args">The event args</param>
        protected void RaiseEvent(PropertyChangedEventHandler handler, object sender, PropertyChangedEventArgs args)
        {
            handler?.Invoke(sender, args);
        }
    }
}

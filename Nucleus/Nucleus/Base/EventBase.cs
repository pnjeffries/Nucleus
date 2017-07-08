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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Reusable base class for objects that raise events.
    /// Provides a protected function which can be used to raise events safely.
    /// </summary>
    [Serializable]
    public abstract class EventBase
    {
        /// <summary>
        /// Raise an event.
        /// Checks for a null handler before raising.
        /// </summary>
        /// <param name="handler">The event handler</param>
        /// <param name="args">The event args</param>
        protected void RaiseEvent(EventHandler handler, EventArgs args)
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
        protected void RaiseEvent(EventHandler handler, object sender, EventArgs args)
        {
            handler?.Invoke(sender, args);
        }

        /// <summary>
        /// Raise an event with a generic handler
        /// </summary>
        /// <typeparam name="TArgs">The type of the event arguments</typeparam>
        /// <param name="handler">The event handler</param>
        /// <param name="args">The event args</param>
        protected void RaiseEvent<TArgs>(EventHandler<TArgs> handler, TArgs args)
            where TArgs : System.EventArgs
        {
            handler?.Invoke(this, args);
        }
    }
}

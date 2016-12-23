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

using FreeBuild.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract base class for objects which can raise message events.
    /// Typically used for objects which own and run a lengthy process
    /// that may need to raise Message events in order to communicate
    /// progress and errors encountered to the outside environment
    /// without interrupting program flow.
    /// </summary>
    [Serializable]
    public abstract class MessageRaiser : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Message event raised when this object has something to communicate to the
        /// outside world.
        /// </summary>
        [field:NonSerialized]
        public EventHandler<MessageRaisedEventArgs> Message;

        /// <summary>
        /// Raise a message event
        /// </summary>
        /// <param name="message"></param>
        protected void RaiseMessage(string message)
        {
            Message?.Invoke(this, new MessageRaisedEventArgs(message));
        }
    }
}

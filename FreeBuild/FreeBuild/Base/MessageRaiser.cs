using FreeBuild.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract base class for objects which can raise message events
    /// </summary>
    [Serializable]
    public abstract class MessageRaiser : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Event raised when this object
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

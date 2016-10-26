using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Events
{
    /// <summary>
    /// Event args for a MessageRaised event
    /// </summary>
    public class MessageRaisedEventArgs
    {
        /// <summary>
        /// The message string
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initialise a new MessageRaisedEventArgs object with the specified parameters
        /// </summary>
        /// <param name="message"></param>
        public MessageRaisedEventArgs(string message)
        {
            Message = message;
        }
    }
}

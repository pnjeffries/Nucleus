using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the EventHandler class
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Raise an event.
        /// Checks for a null handler before raising.
        /// </summary>
        /// <param name="handler">The event handler</param>
        /// <param name="sender">The original sender object</param>
        /// <param name="args">The event args</param>
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            handler?.Invoke(sender, args);
        }

        /// <summary>
        /// Raise an event with a generic handler.
        /// Checks for a null handler before raising.
        /// </summary>
        /// <typeparam name="TArgs">The type of the event arguments</typeparam>
        /// <param name="handler">The event handler</param>
        /// <param name="args">The event args</param>
        public static void Raise<TArgs>(this EventHandler<TArgs> handler, object sender, TArgs args)
            where TArgs : System.EventArgs
        {
            handler?.Invoke(sender, args);
        }
    }
}

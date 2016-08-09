using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
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
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Raise an event with a generic handler
        /// </summary>
        /// <typeparam name="TArgs">The type of the event arguments</typeparam>
        /// <param name="handler">The event handler</param>
        /// <param name="args">The event args</param>
        protected void RaiseEvent<TArgs>(EventHandler<TArgs> handler, TArgs args)
        {
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}

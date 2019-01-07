using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for objects which can be used to generate actions dynamically
    /// </summary>
    [Serializable]
    public abstract class ActionFactory
    {
        /// <summary>
        /// Generate actions given the specified context and add them to the available actions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="addTo"></param>
        public abstract void GenerateActions(TurnContext context, AvailableActions addTo);
    }
}

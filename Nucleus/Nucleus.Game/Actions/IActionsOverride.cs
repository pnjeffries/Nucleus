using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for objects which override the generation of available
    /// actions for an actor in order to provide its own.
    /// </summary>
    public interface IActionsOverride
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="addTo"></param>
        void GenerateOverrideActions(TurnContext context, AvailableActions addTo);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// Interface for components which should be processed when
    /// the actor is moved outside of its normal turn order
    /// </summary>
    public interface IOutOfTurnMove
    {
        /// <summary>
        /// Called when the actor is moved outside of its ordinary
        /// turn order
        /// </summary>
        /// <param name="context"></param>
        void OutOfTurnMove(TurnContext context);
    }
}

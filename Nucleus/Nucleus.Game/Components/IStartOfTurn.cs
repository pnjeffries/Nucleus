using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// Interface for data components which process at the start of 
    /// a turn.
    /// </summary>
    public interface IStartOfTurn
    {
        /// <summary>
        /// Process the start of a turn
        /// </summary>
        /// <param name="context"></param>
        void StartOfTurn(TurnContext context);
    }
}

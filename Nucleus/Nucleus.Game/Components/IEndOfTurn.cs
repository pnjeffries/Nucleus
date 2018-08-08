using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for data components which process at the end of 
    /// a turn.
    /// </summary>
    public interface IEndOfTurn : IDataComponent
    {
        /// <summary>
        /// Process the end of a turn
        /// </summary>
        /// <param name="context"></param>
        void EndOfTurn(TurnContext context);
    }
}

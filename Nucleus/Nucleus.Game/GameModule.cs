using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Abstract base class for game modules.
    /// Modules define top-level information about a game including 
    /// how it should be initialised and run.
    /// </summary>
    public abstract class GameModule
    {
        /// <summary>
        /// Initialise the game module
        /// </summary>
        public virtual void Initialise() { }

        /// <summary>
        /// Get the state which starts this game.
        /// </summary>
        /// <returns></returns>
        public abstract GameState StartingState();
    }
}

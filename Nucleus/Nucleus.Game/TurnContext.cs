using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Object to contain context data about a turn in a turn-based game
    /// </summary>
    [Serializable]
    public class TurnContext
    {
        #region Properties

        /// <summary>
        /// The current state
        /// </summary>
        public GameState State { get; set; }

        /// <summary>
        /// The current stage
        /// </summary>
        public MapStage Stage { get; set; }

        /// <summary>
        /// The element whose turn has completed
        /// (and whose components are currently being activated)
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// The random number generator used to provide randomisation
        /// </summary>
        public Random RNG { get; set; } = new Random();

        /// <summary>
        /// The log to be used to record textual output
        /// </summary>
        public IActionLog Log { get; set; }

        #endregion

        #region Constructors

        public TurnContext() { }

        public TurnContext(GameState state, MapStage stage, Element element, Random rng, IActionLog log)
        {
            State = state;
            Stage = stage;
            Element = element;
            RNG = rng;
            Log = log;
        }

        #endregion
    }
}

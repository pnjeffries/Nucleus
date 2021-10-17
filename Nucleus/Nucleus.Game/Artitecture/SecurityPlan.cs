using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Class to represent security feature generation parameters
    /// for a dungeon artitect
    /// </summary>
    [Serializable]
    public class SecurityPlan
    {
        /// <summary>
        /// The minimum number of rooms that must be placed before a locked
        /// door may be placed
        /// </summary>
        public int MinLockDistance { get; set; } = 2;

        /// <summary>
        /// Can the exit be behind a locked door?
        /// </summary>
        public bool AllowLockedExit { get; set; } = true;
    }
}

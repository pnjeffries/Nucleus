using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Enumerated value to represent different strategies for placing an exit in a room
    /// </summary>
    public enum ExitPlacement
    {
        Any, // Exits may be placed on any wall
        Opposite, // Exits only be placed directly opposite the entrance
        Opposite_Side, // Exits may be placed on the wall opposite the entrance
        Not_Opposite_Side // Exits may be placed on any wall but the opposite side
    }
}

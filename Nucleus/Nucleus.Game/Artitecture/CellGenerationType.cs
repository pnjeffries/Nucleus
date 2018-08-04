using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Artitecture
{
    /// <summary>
    /// An enum to store the type of a cell during map generation
    /// </summary>
    public enum CellGenerationType
    {
        Untouched,
        Void,
        Wall,
        WallCorner,
        Door
    }
}

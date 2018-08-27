using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
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

    public static class CellGenerationTypeExtensions
    {
        /// <summary>
        /// Is this a wall type?
        /// </summary>
        /// <param name="genType"></param>
        /// <returns></returns>
        public static bool IsWall(this CellGenerationType genType)
        {
            return (genType == CellGenerationType.Wall ||
                genType == CellGenerationType.WallCorner);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

namespace Nucleus.Game
{
    /// <summary>
    /// A map cell set up for use in games
    /// </summary>
    public class GameMapCell : MapCell<GameElement, GameElementCollection, GameMapCell>
    {
        protected override MapCellDataStore<GameMapCell> NewDataStore()
        {
            return new MapCellDataStore<GameMapCell>();
        }
    }
}

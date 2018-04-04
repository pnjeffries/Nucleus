using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A game stage which utilises a cell map as a way of
    /// representing and storing map geometry
    /// </summary>
    public class CellMapStage<TCellMap, TCell> : GameStage
        where TCellMap : ICellMap<TCell>
        where TCell : MapCell<GameElement, GameElementCollection, TCell>
    {

    }
}

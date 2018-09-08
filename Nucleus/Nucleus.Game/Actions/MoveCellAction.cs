using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An action which will (attempt to) move an element from one
    /// map cell to another
    /// </summary>
    public class MoveCellAction : GameAction
    {
        #region Constructors

        public MoveCellAction(MapCell cell) : base("Move")
        {
            Trigger = new ActionCellInputTrigger(cell.Index, InputFunction.Move);
            Effects.Add(new MoveCellEffect(cell.Index));
        }

        #endregion
    }
}

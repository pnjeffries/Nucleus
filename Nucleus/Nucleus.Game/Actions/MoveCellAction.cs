using Nucleus.Geometry;
using Nucleus.Model;
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
    public class MoveCellAction : ElementTargetingAction
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for the CellIndex property
        /// </summary>
        private int _CellIndex;

        /// <summary>
        /// The index of the cell this action will move an element into
        /// </summary>
        public int CellIndex
        {
            get { return _CellIndex; }
        }


        #endregion

        #region Constructors

        public MoveCellAction(Element actor, MapCell cell) : base("Move")
        {
            Target = actor;
            _CellIndex = cell.Index;
            Trigger = new ActionCellInputTrigger(cell.Index, InputFunction.Move);
            SelfEffects.Add(new ActorOrientationEffect(actor, cell));
            SelfEffects.Add(new MoveCellEffect(cell.Index));
        }

        #endregion

        #region Methods

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            Element self = context.Element;
            var mDS = self?.GetData<MapData>();
            var mA = self.GetData<MapAwareness>();
            
            // TEMP:
            Element target = ((RLState)context.State).Controlled;
            var mDT = target?.GetData<MapData>();

            if (mDT != null && !target.IsDeleted && mA.AwarenessOfCell(mDT.MapCell.Index) > 0)
            {
                // Manhatten distance calculation:
                Vector newPos = context.Stage.Map.CellPosition(_CellIndex);
                return mDS.Position.ManhattenDistanceTo(mDT.Position)
                    - newPos.ManhattenDistanceTo(mDT.Position) + 0.05;
            }
            else return context.RNG.NextDouble();

            // TODO: Employ influence maps
        }

        #endregion
    }
}

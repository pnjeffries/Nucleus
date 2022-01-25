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

        private Vector _Direction;

        /// <summary>
        /// The direction of movement
        /// </summary>
        public Vector Direction
        {
            get { return _Direction; }
        }


        #endregion

        #region Constructors

        public MoveCellAction(Element actor, MapCell cell) : base("Move")
        {
            Target = actor;
            _CellIndex = cell.Index;
            Vector actorPos = actor.GetNominalPosition();
            _Direction = cell.Position - actorPos;
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
            var targetAI = self.GetData<TargetingAI>();
            Element target = targetAI?.PrimaryTarget?.Target;
            var mDT = target?.GetData<MapData>();

            if (mDT != null && !target.IsDeleted)
            {
                Vector newPos = context.Stage.Map.CellPosition(_CellIndex);

                Vector targetPos;
                if (mA.AwarenessOfCell(mDT.MapCell.Index) >= MapAwareness.Visible)
                {
                    targetPos = mDT.Position;
                }
                else
                {
                    targetPos = targetAI.PrimaryTarget.LastKnownPosition;
                    if (targetPos.IsValid() && newPos.DistanceToSquared(targetPos) < 0.5)
                    {
                        // Reached the target position - clear the record
                        targetAI.PrimaryTarget.LastKnownPosition = Vector.Unset;
                        // TODO: Switch target?
                    }
                }
                // Manhatten distance calculation:
                if (targetPos.IsValid())
                {
                    return mDS.Position.DistanceTo(targetPos)
                        - newPos.DistanceTo(targetPos) + 0.5;
                }
            }
            
            return context.RNG.NextDouble();

            // TODO: Employ influence maps
        }

        #endregion
    }
}

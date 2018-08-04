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
    /// A game state for Roguelikes
    /// </summary>
    [Serializable]
    public class RLState : BasicGameState<MapStage>
    {
        #region Constructor

        public RLState()
        {
           

        }

        #endregion

        #region Methods

        public override void StartUp()
        {
            EndTurnOf(Controlled);
        }

        /// <summary>
        /// Called when the user releases a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public override void InputRelease(InputFunction input, Vector direction)
        {
            base.InputRelease(input, direction);
            Element controlled = Controlled;
            if (controlled != null)
            {
                //Move element:
                MapData mD = controlled.GetData<MapData>();
                if (mD != null && mD.MapCell != null)
                {
                    MapCell newCell = Stage.Map.AdjacentCell(mD.MapCell.Index, direction);
                    if (newCell != null && (controlled.GetData<MapCellCollider>()?.CanEnter(newCell) ?? true))
                        newCell.PlaceInCell(controlled);
                    EndTurnOf(controlled);
                }

            }
        }

        /// <summary>
        /// Process the end of the turn
        /// </summary>
        public void EndTurnOf(Element element)
        {
            var context = new TurnContext(this, Stage, element);
            foreach (IElementDataComponent dC in element.Data)
            {
                if (dC is IEndOfTurn)
                {
                    ((IEndOfTurn)dC).EndOfTurn(context);
                }
            }
        }

        #endregion
    }
}

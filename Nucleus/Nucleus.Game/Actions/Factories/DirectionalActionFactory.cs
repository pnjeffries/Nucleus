using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for ActionFactory types which produce actions for all 
    /// surrounding directions
    /// </summary>
    [Serializable]
    public abstract class DirectionalActionFactory : ActionFactory
    {
        /// <summary>
        /// Generate actions given the specified context and add them to the available actions.
        /// Basic implementation that provides the boilerplate for setting up different directions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="addTo"></param>
        public override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            MapData mD = context.Element?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                IList<MapCell> adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                foreach (var cell in adjacent)
                {
                    Vector position = mD.MapCell.Position;
                    Vector direction = cell.Position -position;
                    GameAction action = ActionForDirection(position, direction, cell, context);
                    if (action != null)
                    {
                        if (ActionName != null) action.Name = ActionName;
                        addTo.Actions.Add(action);
                    }
                }
            }
        }

        public override IList<MapCell> TargetableCells(TurnContext context)
        {
            var result = new List<MapCell>();
            MapData mD = context.Element?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                IList<MapCell> adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                foreach (var cell in adjacent)
                {
                    Vector position = mD.MapCell.Position;
                    Vector direction = cell.Position - position;
                    var cells = TargetableCells(position, direction, context);
                    result.AddRange(cells);
                }
            }
            return result;
        }

        /// <summary>
        /// Generate the action for the specified direction and trigger cell
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="triggerCell"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract GameAction ActionForDirection(Vector position, Vector direction, MapCell triggerCell, TurnContext context);
    }
}

using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// An element data component which indicates that the element is
    /// able to move between cells of its own volition and will generate
    /// suitable actions for open spaces
    /// </summary>
    public class MoveCellProposer : ActionProposer
    {
        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            // Movement & Bump attacking:
            MapData mD = context.Element?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                IList<MapCell> adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                //TODO: Diagonal?
                foreach (var cell in adjacent)
                {
                    if (context.Element.GetData<MapCellCollider>()?.CanEnter(cell) ?? true)
                    {
                        addTo.Actions.Add(new MoveCellAction(cell));
                    }
                }
            }
        }
    }
}

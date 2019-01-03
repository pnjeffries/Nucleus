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
    /// An element data component which indicates that the element is
    /// able to move between cells of its own volition and will generate
    /// suitable actions for open spaces
    /// </summary>
    [Serializable]
    public class MoveCellAbility : Ability
    {
        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            // Movement:
            Element self = context.Element;
            MapData mD = self?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                IList<MapCell> adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                //TODO: Diagonal?
                foreach (var cell in adjacent)
                {
                    if (context.Element.GetData<MapCellCollider>()?.CanEnter(cell) ?? true)
                    {
                        addTo.Actions.Add(new MoveCellAction(self, cell));
                    }
                }
            }
        }
    }
}

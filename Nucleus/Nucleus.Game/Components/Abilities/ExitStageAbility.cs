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
    /// Grants the ability to move between stages through stage exits
    /// </summary>
    [Serializable]
    public class ExitStageAbility : Ability
    {
        public override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            // Movement:
            Element self = context.Element;
            MapData mD = self?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                var adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                foreach (var cell in adjacent)
                {
                    foreach (var element in cell.Contents)
                    {
                        var exit = element.GetData<StageExit>();
                        if (exit != null)
                        {
                            addTo.Actions.Add(new ExitStageAction(cell, exit));
                        }
                    }
                }
            }
        }
    }
}

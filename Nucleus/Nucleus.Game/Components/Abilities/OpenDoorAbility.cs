using Nucleus.Game.Actions;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components.Abilities
{
    /// <summary>
    /// Data component which confers the ability to open doors
    /// </summary>
    [Serializable]
    public class OpenDoorAbility : Ability
    {
        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            MapData mD = context.Element?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                var adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                foreach (var cell in adjacent)
                {
                    Vector direction = cell.Position - mD.MapCell.Position;
                    Element target = context.Element.GetData<MapCellCollider>()?.Blocker(cell); //cell.Contents.FirstWithDataComponent<Door>();
                    if (target != null && target.HasData<Door>())
                    {
                        addTo.Actions.Add(new OpenDoorAction(target));
                    }
                }
            }
        }
    }
}

using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    public class DirectionalItemUseAbility : TemporaryAbility
    {
        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            // Bump attack:
            MapData mD = context.Element?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                IList<MapCell> adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                //TODO: Diagonal?
                foreach (var cell in adjacent)
                {
                    Vector direction = cell.Position - mD.MapCell.Position;
                    IList<Vector> offsets = new Vector[]
                    {
                        new Vector(1,-1),
                        new Vector(1,0),
                        new Vector(1,1)
                    };
                    var pattern = offsets.Rotate(direction.Angle).Move(mD.MapCell.Position);
                    var cells = context.Stage.Map.CellsAt(pattern);
                    addTo.Actions.Add(new AOEAttackAction(cells, cell, direction));

                }
            }
        }
    }
}

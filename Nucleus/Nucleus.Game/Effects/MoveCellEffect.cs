using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    public class MoveCellEffect : BasicEffect
    {
        public override bool Apply(IEffectLog log, EffectContext context)
        {
            Element mover = context.Target;
            if (mover != null)
            {
                //Move element:
                MapData mD = mover.GetData<MapData>();
                if (mD != null && mD.MapCell != null)
                {
                    
                    MapCell newCell = context.Map.AdjacentCell(mD.MapCell.Index, context.Direction);
                    if (newCell != null && (mover.GetData<MapCellCollider>()?.CanEnter(newCell) ?? true))
                    {
                        newCell.PlaceInCell(mover);
                        return true;
                    }
                }

            }
            return false;
        }
    }
}

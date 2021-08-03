using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using Nucleus.Logs;

namespace Nucleus.Game
{
    /// <summary>
    /// An effect which picks up the target item and adds it to
    /// the inventory of the actor
    /// </summary>
    [Serializable]
    public class PickUpItemEffect : BasicEffect
    {
        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            PickUp pickUp = context.Target?.GetData<PickUp>();
            if (pickUp != null) // Can pick up
            {
                // Does it still exist on the map?
                MapData mD = context.Target.GetData<MapData>();
                if (mD?.MapCell != null)
                {
                    // Add to inventory:
                    Inventory inventory = context.Actor?.GetData<Inventory>();
                    if (inventory != null && inventory.AddItem(context.Target))
                    {
                        mD.MapCell.RemoveFromCell(context.Target);
                        context.State.Elements.Remove(context.Target);
                        mD.MapCell = null;
                        context.Target.Data.RemoveData<MapData>();
                    }
                    else
                    {
                        log.WriteScripted(context, "PickUp_Fail", context.Actor, context.Target);
                    }  
                }
            }
            return false;
        }

        #endregion
    }
}

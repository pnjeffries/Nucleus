using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// Effect which drops an item
    /// </summary>
    [Serializable]
    public class DropItemEffect : BasicEffect
    {
        public override bool Apply(IActionLog log, EffectContext context)
        {
            if (context.Target == null) return false;

            var inventory = context.Actor?.GetData<Inventory>();
            if (inventory == null) return false;

            return inventory.DropItem(context.Target, context.Actor, context);
        }
    }
}

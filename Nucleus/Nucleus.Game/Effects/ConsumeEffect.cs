using Nucleus.Game.Components;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// Consume a use of a consumable item, and remove it if spent
    /// </summary>
    [Serializable]
    public class ConsumeEffect : BasicEffect
    {
        public override bool Apply(IActionLog log, EffectContext context)
        {
            ConsumableItem cItem = context.Target.GetData<ConsumableItem>();
            if (cItem == null) return false;

            cItem.Uses -= 1;

            if (cItem.Uses <= 0)
            {
                Inventory inventory = context.Actor?.GetData<Inventory>();
                if (inventory != null)
                {
                    inventory.RemoveItem(context.Target);
                }
                WriteConsumptionToLog(log, context);
                context.Target.Delete();
            }

            return true;
        }

        private void WriteConsumptionToLog(IActionLog log, EffectContext context)
        {
            string key = "Consumed_" + context.Target?.Name;
            if (!log.HasScriptFor(key))
            {
                // Fallback generic death message
                key = "Consumed";
            }
            log.WriteScripted(context, key, context.Actor, context.Target);
        }
    }
}

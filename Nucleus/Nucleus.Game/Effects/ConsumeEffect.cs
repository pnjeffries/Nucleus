using Nucleus.Game.Components;
using Nucleus.Logs;
using Nucleus.Model;
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
        private Element _TargetOverride = null;

        /// <summary>
        /// The item to be consumed.
        /// If null, the contextual target will be used instead.
        /// </summary>
        public Element TargetOverride
        {
            get { return _TargetOverride; }
            set { _TargetOverride = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConsumeEffect() { }

        /// <summary>
        /// Target item constructor
        /// </summary>
        /// <param name="targetItem">The item to be consumed</param>
        public ConsumeEffect(Element targetItem) 
        {
            _TargetOverride = targetItem;
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            var target = TargetOverride;
            if (target == null) target = context.Target;
            ConsumableItem cItem = target.GetData<ConsumableItem>();
            if (cItem == null) return false;

            cItem.Uses -= 1;

            if (cItem.Uses <= 0)
            {
                Inventory inventory = context.Actor?.GetData<Inventory>();
                if (inventory != null)
                {
                    inventory.RemoveItem(target);
                }
                WriteConsumptionToLog(log, target, context);
                target.Delete();
            }

            return true;
        }

        private void WriteConsumptionToLog(IActionLog log, Element target, EffectContext context)
        {
            string key = "Consumed_" + target?.Name;
            if (!log.HasScriptFor(key))
            {
                // Fallback generic death message
                key = "Consumed";
            }
            log.WriteScripted(context, key, context.Actor, target);
        }
    }
}

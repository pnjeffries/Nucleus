using Nucleus.Base;
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
    /// Effect which applies a status effect to the target
    /// </summary>
    [Serializable]
    public class ApplyStatusEffect : BasicEffect
    {
        private IStatusEffect _StatusEffect;

        /// <summary>
        /// The template status effect to be applied
        /// </summary>
        public IStatusEffect StatusEffect { get { return _StatusEffect; } set { StatusEffect = value; } }

        public ApplyStatusEffect() { }

        public ApplyStatusEffect(IStatusEffect statusEffect)
        {
            _StatusEffect = statusEffect;
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            var target = context.Target;
            return ApplyStatus(log, context, target);
        }

        protected virtual bool ApplyStatus(IActionLog log, EffectContext context, Element target)
        {
            var status = target?.GetData<Status>();
            if (status == null) return false;
            if (StatusEffect == null) return false;

            status.AddEffect(StatusEffect.Duplicate());
            WriteStatusChangeToLog(log, context, target);
            return true;
        }
        
        private void WriteStatusChangeToLog(IActionLog log, EffectContext context, Element target)
        {
            string key = "ApplyStatusEffect_" + StatusEffect?.GetType().Name;
            if (log.HasScriptFor(key) && context.IsPlayerAwareOf(target))
            {
                log.WriteLine();
                log.WriteScripted(context, key, context.Actor, target);
            }
        }
    }
}

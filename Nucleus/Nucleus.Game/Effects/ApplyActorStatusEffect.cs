using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// Effect which applies a status effect to the actor when a target
    /// is valid.  Used in preference to a standard SelfEffect when the
    /// status to be applied works per target hit (for example, with combos)
    /// </summary>
    [Serializable]
    public class ApplyActorStatusEffect : ApplyStatusEffect
    {
        public ApplyActorStatusEffect()
        {
        }

        public ApplyActorStatusEffect(IStatusEffect statusEffect) : base(statusEffect)
        {
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            if (context.Target?.GetData<Faction>() == null) return false;

            return ApplyStatus(log, context, context.Actor);
        }
    }
}

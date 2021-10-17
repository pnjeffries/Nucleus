using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Status effect which increases knockback power and cures 
    /// </summary>
    [Serializable]
    public class Strong : StatusEffect, IKnockbackModifier
    {
        public Strong() : base(50)
        {
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            var weakened = context.Target?.GetData<Status>()?.GetEffect<Weakened>();
            if (weakened != null) weakened.TimeRemaining = 0;
            return true;
        }

        public int ModifyKnockback(int knockback, IActionLog log, EffectContext context)
        {
            return knockback + 1;
        }
    }
}

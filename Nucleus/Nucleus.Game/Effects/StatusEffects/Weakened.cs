using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Status effect which decreases knockback
    /// </summary>
    [Serializable]
    public class Weakened : StatusEffect, IKnockbackModifier
    {
        public Weakened() : base(20) { }

        public override bool Apply(IActionLog log, EffectContext context)
        {

            return true;
        }

        public int ModifyKnockback(int knockback, IActionLog log, EffectContext context)
        {
            return Math.Max(0, knockback - 1);
        }
    }
}

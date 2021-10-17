using Nucleus.Base;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Temporary invincibility
    /// </summary>
    [Serializable]
    public class Invincible : StatusEffect, IDefense, IFastDuplicatable
    {
        public Invincible() : base(5) { }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new Invincible();
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            return false;
        }

        public Damage Defend(Damage damage, IActionLog log, EffectContext context)
        {
            // Blocks all damage types
            return damage.WithValue(0);
        }

    }
}

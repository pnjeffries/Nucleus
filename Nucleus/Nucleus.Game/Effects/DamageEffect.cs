using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Logs;
using Nucleus.Game.Components;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// An effect which reduces element hitpoints
    /// </summary>
    public class DamageEffect : BasicEffect
    {
        public override bool Apply(IActionLog log, EffectContext context)
        {
            if (context?.Target != null && context.Target.HasData<HitPoints>())
            {

            }
            else return false;
        }
    }
}

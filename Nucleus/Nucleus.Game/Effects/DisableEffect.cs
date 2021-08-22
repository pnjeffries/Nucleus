using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Base;
using Nucleus.Logs;

namespace Nucleus.Game
{
    /// <summary>
    /// Disable all actions for a turn
    /// </summary>
    [Serializable]
    public class DisableEffect : BasicEffect, IFastDuplicatable
    {
        public override bool Apply(IActionLog log, EffectContext context)
        {
            if (context.Target != null)
            {
                context.Target.GetData<DisableActions>(true).LifeSpan += 1;
                return true;
            }
            return false;
        }

        public IFastDuplicatable FastDuplicate_Internal()
        {
            return new DisableEffect();
        }
    }
}

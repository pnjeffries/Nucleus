using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Logs;

namespace Nucleus.Game
{
    /// <summary>
    /// Disable all actions
    /// </summary>
    [Serializable]
    public class DisableEffect : BasicEffect
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
    }
}

using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Actions
{
    /// <summary>
    /// Action which is used to abort multi-step operations
    /// </summary>
    [Serializable]
    public class AbortAction : GameAction
    {
        public override double ExecutionTime => 0;

        public override bool Enact(IActionLog log, EffectContext context)
        {
            //var disAct = context.Actor?.GetData<DisableActions>();
            //if (disAct != null) disAct.ReduceLifespan();
            context.Actor.Data.RemoveData<DisableActions>();
            return base.Enact(log, context);
        }
    }
}

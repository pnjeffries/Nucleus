using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    public class AOEAttackAction : MultiCellTargetingAction
    {
        #region Constructor

        public AOEAttackAction(IList<MapCell> targets, MapCell triggerCell, Vector direction,
            string sfxKeyword = null)
        {
            Target = targets;
            Trigger = new ActionCellInputTrigger(triggerCell.Index, InputFunction.Move);

            Effects.Add(new SFXImpactEffect());
            Effects.Add(new DamageEffect(2));
            Effects.Add(new KnockbackEffect(direction, 3));
            SelfEffects.Add(new ActorOrientationEffect(direction));
            if (sfxKeyword != null)
                SelfEffects.Add(new SFXEffect(sfxKeyword, true, direction));
        }

        public override bool Enact(IActionLog log, EffectContext context)
        {
            return base.Enact(log, context);
        }

        #endregion
    }
}

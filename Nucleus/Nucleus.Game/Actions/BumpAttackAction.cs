using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A 'bump' melee attack action
    /// </summary>
    [Serializable]
    public class BumpAttackAction : ElementTargetingAction
    {
        #region Constructor

        public BumpAttackAction(Element target, MapCell cell, Vector direction, double damage = 1, double knockback = 1)
        {
            Target = target;
            Trigger = new ActionCellInputTrigger(cell.Index, InputFunction.Move);

            SelfEffects.Add(new ActorOrientationEffect(direction));
            Effects.Add(new SFXImpactEffect());
            Effects.Add(new KnockbackEffect(direction, knockback));
            Effects.Add(new DamageEffect(damage));
            
        }

        #endregion

        #region Methods

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            return 2.0;
        }

        #endregion
    }
}

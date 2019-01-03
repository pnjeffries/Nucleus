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

        public BumpAttackAction(Element target, MapCell cell, Vector direction)
        {
            Target = target;
            Trigger = new ActionCellInputTrigger(cell.Index, InputFunction.Move);
           
            Effects.Add(new SFXImpactEffect());
            Effects.Add(new DamageEffect(1));
            Effects.Add(new KnockbackEffect(direction, 2));
            SelfEffects.Add(new ActorOrientationEffect(direction));
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

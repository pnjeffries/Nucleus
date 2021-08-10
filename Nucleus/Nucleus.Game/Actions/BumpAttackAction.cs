using Nucleus.Base;
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

        public BumpAttackAction(Element target, MapCell cell, Vector direction, double damage = 1, double knockback = 1, IList<IEffect> otherEffects = null)
        {
            Target = target;
            Trigger = new ActionCellInputTrigger(cell.Index, InputFunction.Move);

            SelfEffects.Add(new ActorOrientationEffect(direction));
            Effects.Add(new SFXImpactEffect());
            Effects.Add(new KnockbackEffect(direction, knockback));
            Effects.Add(new DamageEffect(damage));
            if (otherEffects != null)
            {
                foreach (var effect in otherEffects)
                {
                    var newEffect = effect.Duplicate();
                    if (newEffect is IDirectionalEffect dEffect) dEffect.Direction = direction;
                    Effects.Add(newEffect);
                }
            }  
        }

        public BumpAttackAction(Element target, MapCell cell, Vector direction, IList<IEffect> effects = null)
        {
            Target = target;
            Trigger = new ActionCellInputTrigger(cell.Index, InputFunction.Move);

            SelfEffects.Add(new ActorOrientationEffect(direction));
            Effects.Add(new SFXImpactEffect());
            if (effects != null)
            {
                foreach (var effect in effects)
                {
                    var newEffect = effect.Duplicate();
                    if (newEffect is IDirectionalEffect dEffect) dEffect.Direction = direction;
                    Effects.Add(newEffect);
                }
            }

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

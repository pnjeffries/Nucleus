using Nucleus.Base;
using Nucleus.Game.Effects;
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
            SelfEffects.Add(new ApplyStatusEffect(new Combo()));
            Effects.Add(new SFXImpactEffect());
            Effects.Add(new KnockbackEffect(direction, knockback));
            Effects.Add(new DamageEffect(damage));
            if (otherEffects != null)
            {
                foreach (var effect in otherEffects)
                {
                    var newEffect = DuplicateEffect(effect);
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
            SelfEffects.Add(new ApplyStatusEffect(new Combo()));
            Effects.Add(new SFXImpactEffect());
            if (effects != null)
            {
                foreach (var effect in effects)
                {
                    var newEffect = DuplicateEffect(effect);
                    if (newEffect is IDirectionalEffect dEffect) dEffect.Direction = direction;
                    Effects.Add(newEffect);
                }
            }

        }

        #endregion

        #region Methods

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            var targetAI = context.Element.GetData<TargetingAI>();
            if (targetAI != null)
            {
                if (targetAI.Targets.Contains(Target.GUID))
                {
                    var record = targetAI.Targets[Target.GUID];
                    if (record.Aggro >= 1) return 1.0 + record.Aggro;
                }
                return -1; //Don't attack innocent bystanders!
            }
            else return 2.0;
        }

        private IEffect DuplicateEffect(IEffect effect)
        {
            if (effect is IFastDuplicatable fastDup) return fastDup.FastDuplicate() as IEffect;
            else return effect.Duplicate();
        }

        #endregion
    }
}

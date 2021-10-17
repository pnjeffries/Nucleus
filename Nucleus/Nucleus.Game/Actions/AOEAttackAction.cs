using Nucleus.Base;
using Nucleus.Game.Effects;
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

        public AOEAttackAction(IList<GameMapCell> targets, MapCell triggerCell, Vector direction, int damage, int knockBack,
            string sfxKeyword = null)
        {
            Target = targets;
            Trigger = new ActionCellInputTrigger(triggerCell.Index, InputFunction.Move);

            Effects.Add(new SFXImpactEffect());
            Effects.Add(new DamageEffect(damage));
            Effects.Add(new KnockbackEffect(direction, knockBack));
            Effects.Add(new ApplyActorStatusEffect(new Combo()));
            SelfEffects.Add(new ActorOrientationEffect(direction));
            if (sfxKeyword != null)
                SelfEffects.Add(new SFXEffect(sfxKeyword, true, direction));
        }

        public AOEAttackAction(IList<GameMapCell> targets, MapCell triggerCell, Vector direction, IList<IEffect> effects, IList<IEffect> selfEffects,
            string sfxKeyword = null)
        {
            Target = targets;
            Trigger = new ActionCellInputTrigger(triggerCell.Index, InputFunction.Move);

            foreach(var effect in effects)
            {
                var copyEffect = DuplicateEffect(effect);
                if (copyEffect is IDirectionalEffect dirEffect) dirEffect.Direction = direction;
                Effects.Add(copyEffect);
            }
            Effects.Add(new ApplyActorStatusEffect(new Combo()));

            SelfEffects.Add(new ActorOrientationEffect(direction));
            if (sfxKeyword != null)
                SelfEffects.Add(new SFXEffect(sfxKeyword, true, direction));

            if (selfEffects != null)
            {
                foreach (var effect in selfEffects)
                {
                    var copyEffect = DuplicateEffect(effect);
                    if (copyEffect is IDirectionalEffect dirEffect) dirEffect.Direction = direction;
                    SelfEffects.Add(copyEffect);
                }
            }
        }

        public override bool Enact(IActionLog log, EffectContext context)
        {
            return base.Enact(log, context);
        }

        private IEffect DuplicateEffect(IEffect effect)
        {
            if (effect is IFastDuplicatable fastDup) return fastDup.FastDuplicate() as IEffect;
            else return effect.Duplicate();
        }

        #endregion
    }
}

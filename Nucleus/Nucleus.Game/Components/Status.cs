using Nucleus.Base;
using Nucleus.Game.Effects.StatusEffects;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Element status effects
    /// </summary>
    [Serializable]
    public class Status : Unique, IElementDataComponent, IEndOfTurn, IDefense, IStartOfTurn
    {
        private StatusEffectCollection _Effects = new StatusEffectCollection();

        /// <summary>
        /// Get the current collection of status effects acting on the parent object
        /// </summary>
        public StatusEffectCollection Effects
        {
            get { return _Effects; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Status()
        {

        }

        /// <summary>
        /// Initial status effects constructor
        /// </summary>
        /// <param name="effects"></param>
        public Status(params IStatusEffect[] effects)
        {
            foreach (var effect in effects) Effects.Add(effect);
        }

        public void StartOfTurn(TurnContext context)
        {
            // Remove spent effects
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                var effect = Effects[i];
                // Decrease lifespan
                if (!double.IsNaN(effect.TimeRemaining))
                {
                    effect.TimeRemaining -= 1;
                    if (effect.TimeRemaining <= 0)
                    {
                        Effects.RemoveAt(i);
                    }
                }
            }
        }

        public void EndOfTurn(TurnContext context)
        {
            // At the end of the turn, apply all status effects
            var effectContext = new EffectContext(context.Element, context.Element, context.State, context.Stage, context.RNG);
            foreach (var effect in Effects)
            {
                effect.Apply(context.Log, effectContext);
            }
        }

        /// <summary>
        /// Apply a new status effect to the parent element
        /// </summary>
        /// <param name="statusEffect"></param>
        public void AddEffect(IStatusEffect statusEffect)
        {
            var existing = GetEffect(statusEffect.GetType());
            if (existing == null)
            {
                Effects.Add(statusEffect);
            }
            else
            {
                // TODO: different behaviour for different things?
                existing.TimeRemaining += statusEffect.TimeRemaining;
            }
        }

        /// <summary>
        /// Clear all effects of the specified type from the status
        /// </summary>
        /// <typeparam name="TEffect"></typeparam>
        /// <returns>True if any effects were successfully removed</returns>
        public bool ClearEffects<TEffect>()
            where TEffect : IStatusEffect
        {
            bool result = false;
            // Remove spent effects
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                var effect = Effects[i];
                // TODO: Decrease count?
                if (effect is TEffect)
                {
                    Effects.RemoveAt(i);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Get the first status effect of the specified type currently
        /// applied to this component
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IStatusEffect GetEffect(Type type)
        {
            foreach (var effect in Effects)
            {
                if (type.IsAssignableFrom(effect.GetType()))
                {
                    return effect;
                }
            }
            return null;
        }

        /// <summary>
        /// Adjust the specified damage value based on this defense
        /// </summary>
        /// <returns></returns>
        public Damage Defend(Damage damage)
        {
            foreach (var status in Effects)
            {
                if (status is IDefense defense) damage = defense.Defend(damage);
            }
            return damage;
        }
    }
}

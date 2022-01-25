using Nucleus.Base;
using Nucleus.Logs;
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
    public class Status : Unique, IElementDataComponent, IStartOfTurn, IEndOfTurn, ISubModifiers
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
                    if (effect.TimeRemaining < 0)
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
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Apply(context.Log, effectContext);
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
                // Merge two effects of the same type together
                existing.Merge(statusEffect);
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
                    //Effects.RemoveAt(i);
                    effect.TimeRemaining = 0;
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
        /// Get the first status effect of the specified type currently
        /// applied to this component
        /// </summary>
        /// <returns></returns>
        public IStatusEffect GetEffect<TStatus>()
        {
            foreach (var effect in Effects)
            {
                if (effect is TStatus)
                {
                    return effect;
                }
            }
            return null;
        }

        ///// <summary>
        ///// Adjust the specified damage value based on this defense
        ///// </summary>
        ///// <returns></returns>
        //public Damage Defend(Damage damage, IActionLog log, EffectContext context)
        //{
        //    foreach (var status in Effects)
        //    {
        //        if (status is IDefense defense) damage = defense.Defend(damage, log, context);
        //    }
        //    return damage;
        //}

        ///// <summary>
        ///// Adjust a critical 
        ///// </summary>
        ///// <param name="critChance"></param>
        ///// <param name="log"></param>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public double ModifyCritChance(double critChance, IActionLog log, EffectContext context)
        //{
        //    foreach (var status in Effects)
        //    {
        //        if (status is ICritChanceModifier defense) critChance = defense.ModifyCritChance(critChance, log, context);
        //    }
        //    return critChance;
        //}

        //public int ModifyKnockback(int knockback, IActionLog log, EffectContext context)
        //{
        //    foreach (var status in Effects)
        //    {
        //        if (status is IKnockbackModifier kMod) knockback = kMod.ModifyKnockback(knockback, log, context);
        //    }
        //    return knockback;
        //}

        //public double ModifySpeed(double speed)
        //{
        //    foreach (var status in Effects)
        //    {
        //        if (status is ISpeedModifier sMod) speed = sMod.ModifySpeed(speed);
        //    }
        //    return speed;
        //}

        public T ApplyModifiers<T, TInterface>(T value, Func<T, TInterface, T> function)
        {
            foreach (var status in Effects)
            {
                if (status is TInterface iStatus) value = function.Invoke(value, iStatus);
            }
            return value;
        }
    }
}

using Nucleus.Base;
using Nucleus.Log;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for an effect which is to be applied to an element
    /// </summary>
    public interface IEffect : IUnique
    {
        /// <summary>
        /// Has the effect been spent?
        /// This should return true when the effect has been fully applied
        /// and shoul dno longer be executed any further.
        /// </summary>
        bool Spent { get; }

        /// <summary>
        /// Attempt to apply this effect to an element
        /// </summary>
        /// <param name="target">The target element</param>
        /// <param name="log">A message log used to report back the outcome of the effect</param>
        /// <returns></returns>
        bool Apply(IEffectLog log);
    }

    /// <summary>
    /// Generic interface for an effect which is to be applied to a target
    /// object of a specifed type
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public interface IEffect<TTarget> : IEffect
    {
        /// <summary>
        /// The target of the effect
        /// </summary>
        TTarget Target { get; }
    }

    /// <summary>
    /// Extension methods for the IEffect interface
    /// </summary>
    public static class IEffectExtensions
    {
        /// <summary>
        /// Remove all spent effects from this collection
        /// </summary>
        /// <param name="list"></param>
        public static void RemoveSpent(this IList<IEffect> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Spent) list.RemoveAt(i);
            }
        }

        /// <summary>
        /// Apply all unspent effects in this collection to the target object
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <param name="log"></param>
        public static void ApplyAll<TTarget>(this IList<IEffect<TTarget>> list, IEffectLog log)
        {
            foreach (var effect in list)
            {
                if (!effect.Spent) effect.Apply(log);
            }
        }
    }
}

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
    /// Interface for an effect which is to be applied to an element
    /// </summary>
    public interface IEffect : IUnique, IDuplicatable
    {
        /// <summary>
        /// Attempt to apply this effect to an element
        /// </summary>
        /// <param name="target">The target element</param>
        /// <param name="log">A message log used to report back the outcome of the effect</param>
        /// <param name="context">The context in which the effect is being applied</param>
        /// <returns></returns>
        bool Apply(IActionLog log, EffectContext context = null);
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
        /// Apply all effects in this collection to the target object
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <param name="log"></param>
        public static void ApplyAll<TTarget>(this IList<IEffect<TTarget>> list, IActionLog log)
        {
            foreach (var effect in list)
            {
                effect.Apply(log);
            }
        }
    }
}

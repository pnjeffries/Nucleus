using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Component which allows the element to which it is attached to perform critical hits
    /// </summary>
    public class CritHitter : Unique, IElementDataComponent
    {
        private double _BaseChance = 0.1;

        /// <summary>
        /// The base chance of this element performing a critical hit
        /// </summary>
        public double BaseChance
        {
            get { return _BaseChance; }
            set { ChangeProperty(ref _BaseChance, value); }
        }

        /// <summary>
        /// Determine the overall critical success chance
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public double CombinedCritChance(EffectContext context)
        {
            var chance = BaseChance;
            chance = context.Actor.Data.ApplyModifiers<double, ICritChanceModifier>(chance, (value, iModifier) => iModifier.ModifyCritChance(value, null, context));
            return chance;
        }

        /// <summary>
        /// Roll randomly to determine whether a critical success has been achieved
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool RollToCrit(EffectContext context)
        {
            return context.RNG.NextDouble() < CombinedCritChance(context);
        }
    }
}

using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects.StatusEffects
{
    /// <summary>
    /// Interface for status effects, items etc. that
    /// defend against damage
    /// </summary>
    public interface IDefense
    {
        /// <summary>
        /// Adjust the specified damage value based on this defense
        /// </summary>
        /// <returns></returns>
        Damage Defend(Damage damage, IActionLog log, EffectContext context);
    }
}

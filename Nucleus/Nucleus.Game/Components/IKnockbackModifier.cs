using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for components, items, status effects etc. which modify
    /// the amount of knockback done by an actor
    /// </summary>
    public interface IKnockbackModifier
    {
        /// <summary>
        /// Modify the specified base knockback
        /// </summary>
        /// <param name="knockback"></param>
        /// <param name="log"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        int ModifyKnockback(int knockback, IActionLog log, EffectContext context);
    }
}

using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for components, status effects, items etc. that
    /// modify critical hit chance
    /// </summary>
    public interface ICritChanceModifier
    {
        /// <summary>
        /// Modify a critical success chance
        /// </summary>
        /// <param name="critChance"></param>
        /// <returns></returns>
        double ModifyCritChance(double critChance, IActionLog log, EffectContext context);
    }
}

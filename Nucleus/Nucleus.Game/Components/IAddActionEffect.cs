using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for components, items etc. which add additional
    /// effects to specific types of action
    /// </summary>
    public interface IAddActionEffect
    {
        /// <summary>
        /// Generate a list of additional effects to be applied
        /// for the specified action.  Returns null if the action is
        /// of an incorrect type.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IList<IEffect> AdditionalEffectsFor(GameAction action, IActionLog log, EffectContext context);
    }
}

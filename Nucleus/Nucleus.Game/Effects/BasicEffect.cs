using Nucleus.Base;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for simple effects
    /// </summary>
    public abstract class BasicEffect : Unique, IEffect
    {
        public abstract bool Apply(IActionLog log, EffectContext context);
    }
}

using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// The context within which an effect is being applied
    /// </summary>
    public class EffectContext
    {
        public Element Target { get; set; }
    }
}

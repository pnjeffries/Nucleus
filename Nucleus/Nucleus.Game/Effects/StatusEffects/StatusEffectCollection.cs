using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A collection of status effects
    /// </summary>
    [Serializable]
    public class StatusEffectCollection : UniquesCollection<IStatusEffect>
    {
    }
}

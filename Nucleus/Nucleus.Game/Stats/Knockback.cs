using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Stats
{
    /// <summary>
    /// Knockback stat
    /// </summary>
    [Serializable]
    public class Knockback : Stat
    {
        public Knockback()
        {
        }

        public Knockback(double value) : base(value)
        {
        }
    }
}

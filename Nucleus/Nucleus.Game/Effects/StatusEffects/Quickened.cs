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
    /// A Quickened status effect which doubles the actor's speed
    /// </summary>
    [Serializable]
    public class Quickened : StatusEffect, ISpeedModifier, IFastDuplicatable
    {
        public Quickened() : base(10) { }

        public Quickened(Quickened other) : this() { }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            return false;
        }

        public double ModifySpeed(double speed)
        {
            return speed * 2;
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new Quickened(this);
        }
    }
}

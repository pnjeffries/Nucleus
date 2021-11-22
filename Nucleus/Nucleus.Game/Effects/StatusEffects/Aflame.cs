using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Status effect representing when a flammable entity has been set on fire
    /// </summary>
    [Serializable]
    public class Aflame : DamageEffect, IStatusEffect
    {
        private double _TimeRemaining = 3;

        /// <summary>
        /// The time remaining for this status effect.
        /// Should be set to double.NaN for those which
        /// will apply indefinitely.
        /// </summary>
        public double TimeRemaining
        {
            get { return _TimeRemaining; }
            set { ChangeProperty(ref _TimeRemaining, value); }
        }

        public string Description => GetType().Name;

        public Aflame() : base(1, DamageType.Fire)
        {

        }

        public void Merge(IStatusEffect other)
        {
            TimeRemaining = Math.Max(TimeRemaining, other.TimeRemaining);
        }
    }
}

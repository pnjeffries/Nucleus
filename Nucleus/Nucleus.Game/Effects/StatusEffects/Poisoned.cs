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
    /// Poisoned status effect.
    /// Does damage over time.
    /// </summary>
    [Serializable]
    public class Poisoned : DamageEffect, IStatusEffect
    {
        private double _TimeRemaining = 5;

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

        public Poisoned() : base(1, DamageType.Poison)
        {

        }

        public void Merge(IStatusEffect other)
        {
            TimeRemaining += other.TimeRemaining;
        }
    }
}

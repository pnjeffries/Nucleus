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
        private double _TimeRemaining = 15;

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

        private int _DamegeDelay = 3;

        private int _DamageCountDown = 3;

        public string Description => GetType().Name;

        public Poisoned() : base(1, DamageType.Poison)
        {

        }

        public void Merge(IStatusEffect other)
        {
            TimeRemaining += other.TimeRemaining;
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            _DamageCountDown -= 1;
            if (_DamageCountDown <= 0)
            {
                _DamageCountDown = _DamegeDelay;
                return base.Apply(log, context);
            }
            return false;
        }
    }
}

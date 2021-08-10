using Nucleus.Base;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
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

        public Poisoned() : base(1, DamageType.Poison)
        {

        }
    }
}

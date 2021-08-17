using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for status effects which provides a simple implementation
    /// of the IStatusEffect interface.  Note that not all status effects need
    /// inherit from this class - check the interface instead.
    /// </summary>
    [Serializable]
    public abstract class StatusEffect : BasicEffect, IStatusEffect
    {
        private double _TimeRemaining = double.NaN;

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

        /// <summary>
        /// Default constructor
        /// </summary>
        public StatusEffect() { }

        /// <summary>
        /// Duration constructor
        /// </summary>
        /// <param name="duration"></param>
        public StatusEffect(double duration)
        {
            _TimeRemaining = duration;
        }
    }
}

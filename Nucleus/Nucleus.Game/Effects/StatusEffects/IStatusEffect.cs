using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for status effects
    /// </summary>
    public interface IStatusEffect : IEffect
    {
        /// <summary>
        /// The time remaining for this status effect.
        /// Should be set to double.NaN for those which
        /// will apply indefinitely.
        /// </summary>
        double TimeRemaining { get; set; }
    }
}

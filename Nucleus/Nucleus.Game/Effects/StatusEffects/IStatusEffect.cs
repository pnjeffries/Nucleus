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

        /// <summary>
        /// The description string for the status effect (usually the effect's name)
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Merge another status effect into this one
        /// </summary>
        /// <param name="other"></param>
        void Merge(IStatusEffect other);
    }
}

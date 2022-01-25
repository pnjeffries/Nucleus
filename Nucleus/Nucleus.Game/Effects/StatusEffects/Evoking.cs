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
    /// Status effect which represents evocation of a particular magical effect.
    /// </summary>
    [Serializable]
    public class Evoking : StatusEffect, IAddActionEffect
    {
        [CollectionCopy(CopyBehaviour.DUPLICATE, CopyBehaviour.DUPLICATE)]
        private EffectCollection _Effects = new EffectCollection();

        /// <summary>
        /// The effects which this evocation will apply
        /// </summary>
        public EffectCollection Effects
        {
            get { return _Effects; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="effects"></param>
        public Evoking(params IEffect[] effects)
        {
            foreach (var effect in effects) _Effects.Add(effect);
        }

        public IList<IEffect> AdditionalEffectsFor(GameAction action, IActionLog log, EffectContext context)
        {
            if (action is BumpAttackAction)
            {
                TimeRemaining = 0;
                return Effects;
            }
            return null;
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            // TODO: Backfire
            return true;
        }

    }
}

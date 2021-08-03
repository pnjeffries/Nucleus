using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Element status effects
    /// </summary>
    [Serializable]
    public class Status : Unique, IElementDataComponent, IEndOfTurn
    {
        private StatusEffectCollection _Effects = new StatusEffectCollection();

        /// <summary>
        /// Get the current collection of status effects acting on the parent object
        /// </summary>
        public StatusEffectCollection Effects
        {
            get { return _Effects; }
        }

        public void EndOfTurn(TurnContext context)
        {
            // At the end of the turn, apply all status effects
            var effectContext = new EffectContext(context.Element, context.Element, context.State, context.Stage, context.RNG);
            foreach (var effect in Effects)
            {
                effect.Apply(context.Log, effectContext);
            }
        }
    }
}

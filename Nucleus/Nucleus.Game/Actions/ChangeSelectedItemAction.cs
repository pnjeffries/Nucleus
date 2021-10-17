using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An action which changes the selected item in the actor's inventory
    /// </summary>
    [Serializable]
    public class ChangeSelectedItemAction : GameAction
    {

        public override double ExecutionTime => 0;

        /// <summary>
        /// Effect + trigger constructor
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="trigger"></param>
        public ChangeSelectedItemAction(IEffect effect, ActionInputTrigger trigger) : base()
        {
            SelfEffects.Add(effect);
            Trigger = trigger;
        }
    }
}

using Nucleus.Game.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An action which prepares to drop an item
    /// </summary>
    [Serializable]
    public class PrepareToDropItemAction : GameAction
    {
        public override double ExecutionTime => 0;

        public PrepareToDropItemAction()
        {
            SelfEffects.Add(new DisableEffect());
            SelfEffects.Add(new AddAbilityEffect(new SlotItemDropAbility()));
        }
    }
}

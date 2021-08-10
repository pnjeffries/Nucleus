using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Ability to drop items
    /// </summary>
    [Serializable]
    public class DropAbility : Ability
    {
        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            var action = new PrepareToDropItemAction();
            action.Trigger = new ActionInputTrigger(InputFunction.Drop);
            addTo.Actions.Add(action);

            //TODO: Add drop selected action
        }
    }
}

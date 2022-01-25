using Nucleus.Game.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Ability which allows the actor to cycle through their selected item slot
    /// </summary>
    [Serializable]
    public class ChangeSelectedItemAbility : Ability
    {
        public override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            addTo.Actions.Add(new ChangeSelectedItemAction(new SelectPreviousItemEffect(), new ActionInputTrigger(InputFunction.SelectPrevious)));
            addTo.Actions.Add(new ChangeSelectedItemAction(new SelectNextItemEffect(), new ActionInputTrigger(InputFunction.SelectNext)));

            // Selected item info:
            var inventory = context.Element?.GetData<Inventory>();
            if (inventory?.Selected?.Item != null)
            {
                addTo.Actions.Add(
                    new ChangeSelectedItemAction(new ShowItemInfoEffect(), new ActionInputTrigger(InputFunction.ShowInfo)));
            }
        }
    }
}

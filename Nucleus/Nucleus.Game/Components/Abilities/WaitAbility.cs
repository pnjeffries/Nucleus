using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Ability data component which allows an element to take a 'wait' action
    /// </summary>
    [Serializable]
    public class WaitAbility : Ability
    {
        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            //TODO: Add extra effects
            addTo.Actions.Add(new WaitAction());
        }
    }
}

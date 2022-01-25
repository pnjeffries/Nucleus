using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Wrapper ability which allows for other abilities to be conditional on 
    /// </summary>
    [Serializable]
    public class ConditionalAbility : Ability
    {
        private Func<TurnContext, bool> _Condition;
        private Ability _Ability;

        public ConditionalAbility(Func<TurnContext, bool> condition, Ability ability)
        {
            _Condition = condition;
            _Ability = ability;
        }

        public override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            if (!_Condition.Invoke(context)) return;

            _Ability.GenerateActions(context, addTo);
        }
    }
}

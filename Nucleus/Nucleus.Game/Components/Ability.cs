using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for element data components which propose an action
    /// </summary>
    [Serializable]
    public abstract class Ability : IElementDataComponent, IStartOfTurn
    {
        public void StartOfTurn(TurnContext context)
        {
            var actions = context?.Element?.GetData<AvailableActions>();
            // TODO: Test for incapacity, etc?
            if (actions != null)
            {
                GenerateActions(context, actions);
            }
        }

        protected abstract void GenerateActions(TurnContext context, AvailableActions addTo);

    }
}

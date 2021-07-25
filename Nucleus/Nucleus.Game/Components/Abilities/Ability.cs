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
    /// Base class for element data components which propose an action
    /// </summary>
    [Serializable]
    public abstract class Ability : Deletable, IElementDataComponent, IStartOfTurn
    {
        public virtual void StartOfTurn(TurnContext context)
        {
            var actions = context?.Element?.GetData<AvailableActions>();
            // TODO: Test for incapacity, etc?
            if (actions != null)
            {
                if (!IsDisabled(context))
                    GenerateActions(context, actions);
            }
        }

        protected abstract void GenerateActions(TurnContext context, AvailableActions addTo);

        /// <summary>
        /// Is this ability currently disabled by a status effect?
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool IsDisabled(TurnContext context)
        {
            return context.Element.HasData<DisableActions>();
        }

    }
}

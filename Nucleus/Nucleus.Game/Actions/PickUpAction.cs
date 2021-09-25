using Nucleus.Game.Effects;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Action to pick up items
    /// </summary>
    [Serializable]
    public class PickUpAction : ElementTargetingAction
    {
        #region Constructors

        /// <summary>
        /// Creates an action to pick up the target item
        /// </summary>
        /// <param name="target"></param>
        public PickUpAction(Element target)
        {
            Trigger = new ActionInputTrigger(InputFunction.PickUp);
            Target = target;
            Effects.Add(new PickUpItemEffect());
            Effects.Add(new ShowItemInfoEffect(target));
        }

        #endregion

    }
}

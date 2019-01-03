using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Action whereby an element waits or rests for a turn
    /// </summary>
    public class WaitAction : GameAction
    {
        #region Constructor

        public WaitAction() : base("Wait")
        {
            Trigger = new ActionInputTrigger(InputFunction.Wait);
        }

        #endregion

        #region Methods

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            return 0;
        }

        #endregion
    }
}

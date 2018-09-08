using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    public class WaitAction : GameAction
    {
        #region Constructor

        public WaitAction() : base("Wait")
        {
            Trigger = new ActionInputTrigger(InputFunction.Wait);
        }

        #endregion
    }
}

using Nucleus.Game.Effects;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Actions
{
    /// <summary>
    /// Action which allows an actor to move between stages
    /// </summary>
    [Serializable]
    public class ExitStageAction : GameAction
    {
        #region Constructors

        public ExitStageAction(StageExit exit)
        {
            Trigger = new ActionInputTrigger(InputFunction.UseExit);
            SelfEffects.Add(new ExitStageEffect(exit));
        }

        #endregion
    }
}

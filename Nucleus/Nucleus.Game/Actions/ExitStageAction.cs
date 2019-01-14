using Nucleus.Game.Effects;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
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

        public ExitStageAction(MapCell cell, StageExit exit)
        {
            Trigger = new ActionCellInputTrigger(cell.Index, InputFunction.Move);
            SelfEffects.Add(new ExitStageEffect(exit));
        }

        #endregion
    }
}

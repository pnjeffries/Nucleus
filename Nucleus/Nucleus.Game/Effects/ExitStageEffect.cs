using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Logs;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// Effect which will move the actor through the
    /// specified stage exit
    /// </summary>
    public class ExitStageEffect : BasicEffect
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Exit property
        /// </summary>
        private StageExit _Exit = null;

        /// <summary>
        /// The exit to move the target through
        /// </summary>
        public StageExit Exit
        {
            get { return _Exit; }
            set { _Exit = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an ExitStageEffect to move through the specified
        /// exit
        /// </summary>
        /// <param name="exit"></param>
        public ExitStageEffect(StageExit exit)
        {
            _Exit = exit;
        }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            if (Exit.TravelTo != null && context.Target != null)
            {
                if (context.State is BasicGameState<MapStage>)
                {
                    var bgstate = (BasicGameState<MapStage>)context.State;
                    if (bgstate.Elements.Remove(context.Target))
                    {
                        context.Target.GetData<MapAwareness>()?.StageChanged(Exit.TravelTo);
                        Exit.TravelTo.Elements.Add(context.Target);
                        Exit.TravelTo.Map[Exit.CellIndex].PlaceInCell(context.Target);
                        if (context.Target == bgstate.Controlled)
                        {
                            // Change the current stage
                            bgstate.Stage = Exit.TravelTo;
                        }
                        context.ElementMovedOutOfTurn(context.Target, Exit.TravelTo);
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}

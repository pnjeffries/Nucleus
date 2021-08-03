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
    /// Action whereby a game element spends a turn preparing to use an
    /// item or ability
    /// </summary>
    public class WindUpAction : GameAction
    {
        #region Properties

        #endregion

        #region Constructor


        public WindUpAction(ActionFactory actionFactory) : base()
        {
            SelfEffects.Add(new DisableEffect());
            SelfEffects.Add(new AddAbilityEffect(new DirectionalItemUseAbility(actionFactory)));
        }

        public WindUpAction(string name, ActionFactory actionFactory) : base(name)
        {
            SelfEffects.Add(new DisableEffect());
            SelfEffects.Add(new AddAbilityEffect(new DirectionalItemUseAbility(actionFactory)));
        }

        public WindUpAction(string name, ActionFactory actionFactory, InputFunction input) : this(name, actionFactory)
        {
            Trigger = new ActionInputTrigger(input);
        }

        #endregion

        #region Methods

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            Element self = context.Element;
            var mDS = self?.GetData<MapData>();
            var mA = self.GetData<MapAwareness>();

            // TEMP:
            Element target = ((RLState)context.State).Controlled;
            var mDT = target?.GetData<MapData>();

            if (mDT != null && !target.IsDeleted && mA.AwarenessOfCell(mDT.MapCell.Index) > 0)
            {
                // Distance calculation:
                double manDist = mDS.Position.DistanceTo(mDT.Position);
                // Works for sword slash, but need more detail for other things:
                if (manDist < 2.5) //TEMP
                {
                    return context.RNG.NextDouble() * 2;
                }
            }
            return -0.5;
        }

        #endregion
    }
}

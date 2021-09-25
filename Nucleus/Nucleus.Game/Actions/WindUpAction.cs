using Nucleus.Extensions;
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
    public class WindUpAction : ResourceAction
    {

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

        public WindUpAction(string name, Resource resourceRequired, ActionFactory actionFactory) : base(name, resourceRequired)
        {
            SelfEffects.Add(new DisableEffect());
            SelfEffects.Add(new AddAbilityEffect(new DirectionalItemUseAbility(actionFactory)));
        }

        public WindUpAction(string name, ActionFactory actionFactory, params IEffect[] selfEffects) : this(name, actionFactory)
        {
            foreach (var selfEffect in selfEffects)
                SelfEffects.Add(selfEffect);
        }

        public WindUpAction(string name, ActionFactory actionFactory, InputFunction input) : this(name, actionFactory)
        {
            Trigger = new ActionInputTrigger(input);
        }

        public WindUpAction(string name, ActionFactory actionFactory, InputFunction input, params IEffect[] selfEffects) : this(name, actionFactory, input)
        {
            foreach (var selfEffect in selfEffects)
                SelfEffects.Add(selfEffect);
        }

        #endregion

        #region Methods

        private IList<GameMapCell> GetTargetableCells(TurnContext context)
        {
            var aAE = SelfEffects.FirstOfType<AddAbilityEffect>();
            if (aAE == null) return null;

            if (aAE.Ability is DirectionalItemUseAbility dIUA)
            {
                return dIUA.ActionFactory?.TargetableCells(context);
            }
            return null;
        }

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            Element self = context.Element;

            if (!HasRequiredResources(self)) return -0.5;

            var mDS = self?.GetData<MapData>();
            var mA = self.GetData<MapAwareness>();

            var cells = GetTargetableCells(context);

            // TEMP:
            Element target = ((RLState)context.State).Controlled;
            var mDT = target?.GetData<MapData>();

            if (mDT?.MapCell != null && !target.IsDeleted && mA.AwarenessOfCell(mDT.MapCell.Index) >= MapAwareness.Visible)
            {
                if (cells.Contains(mDT.MapCell) || cells.ContainsAny(mDT.MapCell.AdjacentCells()))
                {
                    return context.RNG.NextDouble() * 2;
                }
            }
            return -0.5;
        }

        #endregion
    }
}

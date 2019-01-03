using Nucleus.Geometry;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for actions which may target multiple map cells
    /// </summary>
    public abstract class MultiCellTargetingAction : TargetedAction<IList<MapCell>>
    {
        #region Constructors

        public MultiCellTargetingAction()
        {
        }

        public MultiCellTargetingAction(string name) : base(name)
        {
        }

        public MultiCellTargetingAction(string name, IEffect effect) : base(name, effect)
        {
        }

        public MultiCellTargetingAction(string name, params IEffect[] effects) : base(name, effects)
        {
        }

        public MultiCellTargetingAction(string name, IList<MapCell> target, params IEffect[] effects) : base(name, target, effects)
        {
        }

        #endregion

        #region Methods

        protected override void ApplyEffects(IActionLog log, EffectContext context)
        {
            // Apply to all viable elements in all targeted cells

            foreach (MapCell cell in Target)
            {
                // Create a copy so that modifications to cell contents
                // don't screw up the enumeration:
                var elements = cell.Contents.ToList();

                foreach (var element in elements)
                {
                    if (CanTarget(element))
                    {
                        context.Target = element;
                        base.ApplyEffects(log, context);
                    }
                }
            }
        }

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            double result = 0;
            foreach (MapCell cell in Target)
            {
                // Create a copy so that modifications to cell contents
                // don't screw up the enumeration:
                var elements = cell.Contents.ToList();

                foreach (var element in elements)
                {
                    if (context.Element?.GetData<Faction>()?.IsEnemy(element?.GetData<Faction>()) ?? false)
                    {
                        result += 10;
                    }
                    if (context.Element?.GetData<Faction>()?.IsAlly(element?.GetData<Faction>()) ?? false)
                    {
                        result -= context.RNG.NextDouble();
                    }
                }
            }
            return result;
        }

        #endregion
    }
}

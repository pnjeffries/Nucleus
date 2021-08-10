﻿using Nucleus.Geometry;
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
                        WriteTargetLog(log, context);
                        base.ApplyEffects(log, context);
                    }
                }
            }
        }

        protected virtual void WriteTargetLog(IActionLog log, EffectContext context)
        {
            string postfix = "_Target";
            if (log == null) return;
            string key = this.GetType().Name + postfix;
            if (Name != null && log.HasScriptFor(Name + postfix)) key = Name + postfix;
            else if (!log.HasScriptFor(key)) return;

            log.WriteScripted(context, key, context.Actor, context.Target);
        }

        public override double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            double result = 0;
            var awareness = context.Element?.GetData<MapAwareness>();

            // TEMP:
            var target = ((RLState)context.State).Controlled;
            // Ignore if the actor can't see the target
            if (awareness != null && awareness.AwareOf(target) == false) target = null;

            foreach (MapCell cell in Target)
            {
                
                // Create a copy so that modifications to cell contents
                // don't screw up the enumeration:
                var elements = cell.Contents.ToList();

                foreach (var element in elements)
                {
                    // Ignore if the actor can't see the element
                    if (awareness != null && awareness.AwareOf(element) == false) continue;

                    var faction = element?.GetData<Faction>();
                    if (faction?.IsEnemy(element?.GetData<Faction>()) ?? false)
                    {
                        result += 10;
                    }
                    if (faction?.IsAlly(element?.GetData<Faction>()) ?? false)
                    {
                        result -= 10 * context.RNG.NextDouble();
                    }
                }

                // Tiebreaker:
                if (target != null)
                {
                    double dist = cell.Position.DistanceToSquared(target.GetNominalPosition());
                    if (dist == 0) result += 1;
                    else result += 0.1 / dist;
                }
            }

            return result;
        }

        #endregion
    }
}

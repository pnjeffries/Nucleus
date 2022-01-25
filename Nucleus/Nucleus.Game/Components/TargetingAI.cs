using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// AI component responsible for tracking potential targets
    /// </summary>
    [Serializable]
    public class TargetingAI : IElementDataComponent, IEndOfTurn
    {
        #region Properties

        private TargetAIRecordCollection _Targets = new TargetAIRecordCollection();

        /// <summary>
        /// The set of current targets
        /// </summary>
        public TargetAIRecordCollection Targets
        {
            get { return _Targets; }
        }

        /// <summary>
        /// Get the target with the highest aggro rating (either positive or negative)
        /// </summary>
        public TargetAIRecord PrimaryTarget
        {
            get { return Targets.ItemWithMaxWhere(t => t.Aggro.Abs(), t => t.Aggro.Abs() >= 1); }
        }

        private double _Alertness = 0;

        /// <summary>
        /// The level of alertness of this AI
        /// </summary>
        public double Alertness
        {
            get { return _Alertness; }
            set { _Alertness = value; }
        }

        /// <summary>
        /// Is this AI currently in an alert state?
        /// </summary>
        public bool IsAlert
        {
            get { return _Alertness >= 1; }
        }

        private double _Aggression = 1;

        /// <summary>
        /// The aggression rating of the element this is attached to; determines how rapidly
        /// the actor becomes aggressive.
        /// </summary>
        public double Aggression
        {
            get { return _Aggression; }
            set { _Aggression = value; }
        }

        #endregion

        #region Constructor

        public TargetingAI(double agression = 1)
        {
            _Aggression = agression;
        }

        #endregion

        public void EndOfTurn(TurnContext context)
        {
            UpdateTargeting(context);
        }

        public void UpdateTargeting(TurnContext context)
        {
            var faction = context.Element.GetData<Faction>();
            var awareness = context.Element.GetData<MapAwareness>();
            if (awareness?.FieldOfView == null) return;

            for (int i = 0; i < awareness.FieldOfView.CellCount; i++)
            {
                if (awareness.FieldOfView[i] >= MapAwareness.Visible)
                {
                    foreach (var element in context.Stage.Map[i].Contents)
                    {
                        var faction2 = element.GetData<Faction>();
                        if (faction2 != null && faction.IsEnemy(faction2)) // Enemy target
                        {
                            TargetAIRecord record;
                            if (!Targets.Contains(element.GUID))
                            {
                                record = new TargetAIRecord(element);
                                Targets.Add(record);
                            }
                            else record = Targets[element.GUID];
                            record.LastKnownPosition = element.GetNominalPosition();
                            double dAggro = Aggression / StealthOf(element); //Temp
                            if (Alertness < 1 && Alertness + dAggro >= 1)
                            {
                                //Now aggroed!  Special effects:
                                context.SFX.Trigger(SFXKeywords.Alert, context.Element);
                            }
                            Alertness += dAggro;
                            record.Aggro += dAggro;
                        }
                    }
                }
            }

            // Decline alertness
            // TODO
        }

        private double StealthOf(Element element)
        {
            return 1;
        }
    }
}

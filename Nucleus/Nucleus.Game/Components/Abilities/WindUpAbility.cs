using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Ability that has a wind-up phase
    /// </summary>
    [Serializable]
    public class WindUpAbility : Ability
    {
        /// <summary>
        /// Private backing member variable for the Prototype property
        /// </summary>
        private WindUpAction _Prototype;

        /// <summary>
        /// The prototype of the wind-up action.
        /// </summary>
        public WindUpAction Prototype
        {
            get { return _Prototype; }
            set { _Prototype = value; }
        }

        public WindUpAbility(WindUpAction prototype)
        {
            _Prototype = prototype;
        }

        private GameAction DuplicatePrototype()
        {
            if (Prototype is IFastDuplicatable fastDup) return (GameAction)fastDup.FastDuplicate();
            else return Prototype.Duplicate();
        }

        public override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            if (Prototype != null)
            {
                var action = DuplicatePrototype();
                addTo.Actions.Add(action);
            }
        }
    }
}

using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A component which designates am item
    /// </summary>
    [Serializable]
    public class Armour : IElementDataComponent, IDefense
    {
        private DamageType _ResistType = DamageType.Sharp;

        /// <summary>
        /// The type of damage this armour can resist
        /// </summary>
        public DamageType ResistType
        {
            get { return _ResistType; }
            set { _ResistType = value; }
        }

        public Damage Defend(Damage damage, IActionLog log, EffectContext context)
        {
            if (damage.DamageType == _ResistType && damage.Value > 1)
                return damage.WithValue(damage.Value - 1);
            else return damage;
        }
    }
}

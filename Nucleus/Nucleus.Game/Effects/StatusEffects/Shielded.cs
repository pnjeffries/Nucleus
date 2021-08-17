using Nucleus.Game.Effects.StatusEffects;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Status effect applied when a character is being shielded from harm
    /// </summary>
    [Serializable]
    public class Shielded : StatusEffect, IDefense
    {

        public Shielded() : base(1) { }

        /// <summary>
        /// Adjust the specified damage value based on this defense
        /// </summary>
        /// <returns></returns>
        public Damage Defend(Damage damage)
        {
            if (damage.DamageType == DamageType.Base) //TODO: Other types
                return damage.WithValue(0);
            else return damage;
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            return false;
        }
    }
}

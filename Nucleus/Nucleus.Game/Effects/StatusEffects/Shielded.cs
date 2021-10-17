using Nucleus.Base;
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
    public class Shielded : StatusEffect, IDefense, IFastDuplicatable
    {

        public Shielded() : base(0) { }

        /// <summary>
        /// Adjust the specified damage value based on this defense
        /// </summary>
        /// <returns></returns>
        public Damage Defend(Damage damage, IActionLog log, EffectContext context)
        {
            if (damage.DamageType == DamageType.Base) //TODO: Other types
            {
                WriteBlockToLog(log, context);
                return damage.WithValue(0);
            }
            else return damage;
        }

        private void WriteBlockToLog(IActionLog log, EffectContext context)
        {
            log.WriteScripted(context, "Shield_Block", context.Actor, context.Target);
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            return false;
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new Shielded();
        }
    }
}

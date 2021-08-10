using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Component for weapon equipment which can be used for quick (bump)
    /// attacks.
    /// </summary>
    [Serializable]
    public class QuickAttack : Unique, IElementDataComponent
    {
        private EffectCollection _Effects = new EffectCollection();

        /// <summary>
        /// The effects which are to be applied when this item is equipped
        /// and used in a quick attack
        /// </summary>
        public EffectCollection Effects
        {
            get { return _Effects; }
            set { _Effects = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public QuickAttack() { }

        /// <summary>
        /// Effects constructor
        /// </summary>
        /// <param name="effects"></param>
        public QuickAttack(params IEffect[] effects)
        {
            foreach (var effect in effects) Effects.Add(effect);
        }

        /// <summary>
        /// Damage + Knockback constructor.
        /// Automatically creates a damage and knockback effect with the specified
        /// values
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageType"></param>
        /// <param name="knockback"></param>
        public QuickAttack(double damage, DamageType damageType, double knockback)
            : this(new KnockbackEffect(knockback), new DamageEffect(damage, damageType)) { }
    }
}

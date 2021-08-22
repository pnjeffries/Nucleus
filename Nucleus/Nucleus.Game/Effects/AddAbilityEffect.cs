using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Base;
using Nucleus.Logs;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// An effect which adds an ability to an 
    /// </summary>
    [Serializable]
    public class AddAbilityEffect : BasicEffect, IFastDuplicatable
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Ability property
        /// </summary>
        private Ability _Ability = null;

        /// <summary>
        /// The ability to be added to the target
        /// </summary>
        public Ability Ability
        {
            get { return _Ability; }
            set { _Ability = value; }
        }

        #endregion

        #region Constructors

        public AddAbilityEffect(Ability ability)
        {
            Ability = ability;
        }

        public AddAbilityEffect(AddAbilityEffect other) : this(other.Ability)
        {

        }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            context.SFX.Trigger("ChargeUp", context.Target.GetNominalPosition());
            if (context?.Target != null && Ability != null && 
                !context.Target.HasData(Ability.GetType()))
            {
                context.Target.Data.Add(Ability.Duplicate());
            }
            return false;
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new AddAbilityEffect(this);
        }

        #endregion
    }
}

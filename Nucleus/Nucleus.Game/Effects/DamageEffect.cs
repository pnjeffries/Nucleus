using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Logs;

namespace Nucleus.Game
{
    /// <summary>
    /// An effect which reduces element hitpoints
    /// </summary>
    [Serializable]
    public class DamageEffect : BasicEffect, IFastDuplicatable
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Damage property
        /// </summary>
        private Damage _Damage = new Damage(1);

        /// <summary>
        /// The value of the damage to be inflicted
        /// </summary>
        public Damage Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }


        #endregion

        #region Constructors

        public DamageEffect(double damage)
        {
            _Damage = new Damage(damage);
        }

        public DamageEffect(double damage, DamageType damageType)
        {
            _Damage = new Damage(damage, damageType);
        }

        public DamageEffect(Damage damage)
        {
            _Damage = damage;
        }

        public DamageEffect(DamageEffect other) : this(other.Damage.FastDuplicate()) { }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            // TODO: Test for immunity
            HitPoints hP = context?.Target?.GetData<HitPoints>();
            if (hP != null)
            {
                var damage = Damage;

                if (context.Critical) damage = damage.WithValue(damage.Value + 1); //Temp?

                // Apply damage modifiers
                damage = context.Target.Data.ApplyModifiers<Damage, IDefense>(damage, (value, i) => i.Defend(value, log, context));

                if (damage <= 0) return true;

                // Apply damage
                hP.Value -= damage;

                // End target's combo (if applicable)
                var status = context.Target.GetData<Status>();
                if (status != null) status.ClearEffects<Combo>();

                // Kill the target (if applicable)
                if (!context.Target.IsDeleted && hP.Value <= 0)
                {
                    Vector position = context.Target.GetData<MapData>()?.Position ?? Vector.Unset;
                    context.SFX.Trigger(SFXKeywords.Bang, position);
                    // Destroy!
                    Inventory inventory = context.Target.GetData<Inventory>();
                    if (inventory != null)
                    {
                        // Drop held items!
                        inventory.DropAll(context.Target, context);
                    }
                    context.Target.Delete();
                    WriteDeathToLog(log, context);
                }
                return true;
            }
            else return false;
        }

        private void WriteDeathToLog(IActionLog log, EffectContext context)
        {
            string key = "Death_" + context.Target?.Name;
            if (!log.HasScriptFor(key))
            {
                // Fallback generic death message
                key = "Death";
            }
            log.WriteScripted(context, key, context.Actor, context.Target);
        }

        public IFastDuplicatable FastDuplicate_Internal()
        {
            return new DamageEffect(this);
        }

        #endregion
    }
}

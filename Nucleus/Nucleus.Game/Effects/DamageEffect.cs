using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using Nucleus.Logs;

namespace Nucleus.Game
{
    /// <summary>
    /// An effect which reduces element hitpoints
    /// </summary>
    [Serializable]
    public class DamageEffect : BasicEffect
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

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            // TODO: Test for immunity
            HitPoints hP = context?.Target?.GetData<HitPoints>();
            if (hP != null)
            {
                // Calculate damage (taking account of target resistances/vulnerabilities)
                double damage = Damage.Value * Damage.DamageType.MultiplierFor(context.Target);
                
                // Apply damage
                hP.Value -= damage;

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

        #endregion
    }
}

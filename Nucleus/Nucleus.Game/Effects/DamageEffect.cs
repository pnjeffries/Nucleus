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
        private double _Damage = 1;

        /// <summary>
        /// The value of the damage to be inflicted
        /// </summary>
        public double Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }

        /// <summary>
        /// Private backing member variable for the DamageType property
        /// </summary>
        private DamageType _DamageType = DamageType.Base;

        /// <summary>
        /// The type of the damage
        /// </summary>
        public DamageType DamageType
        {
            get { return _DamageType; }
            set { _DamageType = value; }
        }


        #endregion

        #region Constructors

        public DamageEffect(double damage)
        {
            Damage = damage;
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
                double damage = Damage * DamageType.MultiplierFor(context.Target);
                
                // Apply damage
                hP.Value -= damage;

                // Kill the target (if applicable)
                if (hP.Value <= 0)
                {
                    Vector position = context.Target.GetData<MapData>()?.Position ?? Vector.Unset;
                    context.SFX.Trigger(SFXKeywords.Bang, position);
                    // Destroy!
                    Equipped equipped = context.Target.GetData<Equipped>();
                    if (equipped != null)
                    {
                        // Drop held items!
                        equipped.DropAll(context.Target, context);
                    }
                    context.Target.Delete();     
                }
                return true;
            }
            else return false;
        }

        #endregion
    }
}

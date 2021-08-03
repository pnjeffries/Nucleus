using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// An effect which heals an actor
    /// </summary>
    [Serializable]
    public class HealEffect : BasicEffect
    {
        #region Properties

        private double _Healing = 1;

        /// <summary>
        /// The hitpoints which will be recovered
        /// </summary>
        public double Healing
        {
            get { return _Healing; }
            set { _Healing = value; }
        }

        #endregion

        #region Constructors

        public HealEffect() { }

        public HealEffect(double healing)
        {
            Healing = healing;
        }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            HitPoints hP = context?.Target?.GetData<HitPoints>();
            if (hP != null)
            {
                double healing = Healing;

                hP.Value += healing;
                return true;
            }
            else return false;
        }

        #endregion
    }
}

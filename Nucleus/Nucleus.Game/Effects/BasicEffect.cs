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
    /// Base class for simple effects
    /// </summary>
    public abstract class BasicEffect : Unique, IEffect
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Spent property
        /// </summary>
        private bool _Spent = false;

        /// <summary>
        /// Has this effect been spent?
        /// </summary>
        public bool Spent
        {
            get { return _Spent; }
            set { ChangeProperty(ref _Spent, value, "Spent"); }
        }

        #endregion


        public abstract bool Apply(IEffectLog log, EffectContext context);
    }
}

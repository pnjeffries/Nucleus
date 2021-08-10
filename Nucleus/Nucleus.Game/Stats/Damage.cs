using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Damage stat
    /// </summary>
    [Serializable]
    public class Damage : Stat
    {
        #region Properties

        private DamageType _DamageType = DamageType.Base;

        /// <summary>
        /// The type of damage
        /// </summary>
        public DamageType DamageType
        {
            get { return _DamageType; }
            set { _DamageType = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Damage()
        {
        }

        /// <summary>
        /// Value constructor.  Base damage type.
        /// </summary>
        /// <param name="value"></param>
        public Damage(double value) : base(value)
        {
        }

        /// <summary>
        /// Value, type constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="damageType"></param>
        public Damage(double value, DamageType damageType) : this(value)
        {
            _DamageType = damageType;
        }

        #endregion

        #region Operators



        #endregion
    }
}

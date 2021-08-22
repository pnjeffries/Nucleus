using Nucleus.Base;
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
    public class Damage : Stat, IFastDuplicatable
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

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        public Damage(Damage other) : this(other.Value, other.DamageType)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a copy of this damage with the specified new value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Damage WithValue(double value)
        {
            return new Damage(value, DamageType);
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new Damage(this);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Multiplication operator
        /// </summary>
        /// <param name="d"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Damage operator * (Damage d, double scalar)
        {
            return d.WithValue(d.Value * scalar);
        }

        #endregion
    }
}

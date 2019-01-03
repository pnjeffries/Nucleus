using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Represents a type of damage
    /// </summary>
    [Serializable]
    public class DamageType : Named
    {
        #region Constants

        /// <summary>
        /// The base damage type
        /// </summary>
        public static readonly DamageType Base = new DamageType("Base");

        #endregion

        #region Constructors

        public DamageType(string name)
        {
            Name = name;
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (obj is DamageType) return Name == ((DamageType)obj).Name;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion
    }
}

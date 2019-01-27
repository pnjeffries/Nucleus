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

        /// <summary>
        /// Get the damage multiplier for the specified element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public double MultiplierFor(Element element)
        {
            DamageVulnerabilities vulns = element?.GetData<DamageVulnerabilities>();
            if (vulns == null) return 1.0;
            else return vulns.VulnerabilityTo(this);
        }

        #endregion
    }
}

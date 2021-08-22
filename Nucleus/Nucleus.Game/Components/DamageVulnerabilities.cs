using Nucleus.Game.Effects.StatusEffects;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Data component to store the vulnerabilities and resistances of elements to different
    /// forms of damage.
    /// </summary>
    [Serializable]
    public class DamageVulnerabilities : IElementDataComponent, IDefense
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the BaseFactor property
        /// </summary>
        private double _BaseFactor = 1.0;

        /// <summary>
        /// The default factor to be applied to damage types not explicitly listed
        /// </summary>
        public double BaseFactor
        {
            get { return _BaseFactor; }
            set { _BaseFactor = value; }
        }

        /// <summary>
        /// Private backing member variable for the Vulnerabilities property
        /// </summary>
        private IDictionary<DamageType, double> _Vulnerabilities = new Dictionary<DamageType, double>();

        /// <summary>
        /// The map of vulnerability factors for different damage types.  The amount of damage 
        /// taken will be multiplied by these factors for the relevant damage type.  Lower than 1
        /// = resistant, greater than 1 = vulnerable.
        /// </summary>
        public IDictionary<DamageType, double> Vulnerabilities
        {
            get { return _Vulnerabilities; }
            set { _Vulnerabilities = value; }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new empty DamageVulnerabilities data component
        /// </summary>
        public DamageVulnerabilities() { }

        /// <summary>
        /// Creates a DamageVulnerabilities component with the specified
        /// default damage multiplier.
        /// </summary>
        /// <param name="baseFactor"></param>
        public DamageVulnerabilities(double baseFactor)
        {
            BaseFactor = baseFactor;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Get the damage multiplier for the specified damage type
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public double VulnerabilityTo(DamageType damage)
        {
            if (Vulnerabilities.ContainsKey(damage)) return Vulnerabilities[damage];
            else return BaseFactor;
        }

        /// <summary>
        /// Add a new vulnerability/immunity
        /// </summary>
        /// <param name="damageType">The damage type</param>
        /// <param name="vulnerability">The damage multiplication factor</param>
        public void Add(DamageType damageType, double vulnerability)
        {
            Vulnerabilities.Add(damageType, vulnerability);
        }

        /// <summary>
        /// Adjust the specified damage value based on this defense
        /// </summary>
        /// <returns></returns>
        public Damage Defend(Damage damage, IActionLog log, EffectContext context)
        {
            return damage * VulnerabilityTo(damage.DamageType);
        }

        #endregion
    }
}

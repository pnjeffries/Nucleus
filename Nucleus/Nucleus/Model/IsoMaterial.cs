using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A material with isotropic properties
    /// </summary>
    [Serializable]
    public class IsoMaterial : Material
    {
        #region Properties

        /// <summary>
        /// Private backing field for E property
        /// </summary>
        private double _E = 205000000000;

        /// <summary>
        /// The Elastic (or, Young's) Modulus of this material,
        /// in N/m²
        /// </summary>
        public double E
        {
            get { return _E; }
            set { ChangeProperty(ref _E, value, "E"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public IsoMaterial() : base() { }

        /// <summary>
        /// Name constructor
        /// </summary>
        /// <param name="name"></param>
        public IsoMaterial(string name) : base(name) { }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

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
            set
            {
                ChangeProperty(ref _E, value, "E");
                if (_G <= 0) NotifyPropertyChanged("G");
            }
        }

        /// <summary>
        /// Private backing field for PoissonsRatio property
        /// </summary>
        private double _PoissonsRatio = 0.3;

        /// <summary>
        /// The Poisson's Ratio of this material.
        /// Unitless.
        /// </summary>
        public double PoissonsRatio
        {
            get { return _PoissonsRatio; }
            set
            {
                ChangeProperty(ref _PoissonsRatio, value, "PoissonsRatio");
                if (_G <= 0) NotifyPropertyChanged("G");
            }
        }

        /// <summary>
        /// Private backing field for G property
        /// </summary>
        private double _G = -1;

        /// <summary>
        /// The Shear Modulus of this material.  In N/m².
        /// If this is set lower or equal to zero, the
        /// return value will be automatically calculated from
        /// the Elastic Modulus and Poisson's Ratio via the formula
        /// E / (2v + 2).
        /// </summary>
        public double G
        {
            get
            {
                if (_G <= 0) return E / (2 * PoissonsRatio + 2);
                else return _G;
            }
            set { ChangeProperty(ref _G, value, "G"); }
        }

        /// <summary>
        /// Private backing field for Alpha property
        /// </summary>
        private double _Alpha = 12.0;

        /// <summary>
        /// The coefficient of thermal expansion of this material,
        /// in /°C
        /// </summary>
        public double Alpha
        {
            get { return _Alpha; }
            set { ChangeProperty(ref _Alpha, value, "Alpha"); }
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

        #region Methods

        /// <summary>
        /// Get the Elastic (or, Young's) Modulus of this material
        /// in the specified direction, in N/m²
        /// </summary>
        public override double GetE(Direction direction)
        {
            return E;
        }

        #endregion
    }
}

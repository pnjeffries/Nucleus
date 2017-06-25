using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A class representing a material with uniform properties
    /// </summary>
    [Serializable]
    public class IsotropicMaterial : Material
    {
        #region Properties

        /// <summary>
        /// Private backing field for Density property
        /// </summary>
        private double _Density = 0;

        /// <summary>
        /// The density, or mass per unit volume, of the material.
        /// In kg/m^3
        /// </summary>
        public double Density
        {
            get { return _Density; }
            set { ChangeProperty(ref _Density, value, "Density"); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.  Initialises a new blank material.
        /// </summary>
        public IsotropicMaterial() : base() { }

        /// <summary>
        /// Initialise a new material with the given name.
        /// </summary>
        /// <param name="name"></param>
        public IsotropicMaterial(string name) : base(name) { }

        #endregion
    }
}

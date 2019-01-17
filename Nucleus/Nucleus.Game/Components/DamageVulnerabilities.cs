using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// Data component to store the vulnerabilities of elements to different
    /// forms of damage.
    /// </summary>
    [Serializable]
    public class DamageVulnerabilities : IElementDataComponent
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
        
        #endregion
    }
}

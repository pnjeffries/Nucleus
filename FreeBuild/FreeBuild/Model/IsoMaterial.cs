using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A material with isotropic properties
    /// </summary>
    [Serializable]
    public class IsoMaterial : Material
    {
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

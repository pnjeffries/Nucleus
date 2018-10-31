using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Structure to represent pairs of angles
    /// </summary>
    public struct AnglePair
    {
        #region Fields

        /// <summary>
        /// The elevation angle
        /// </summary>
        public readonly Angle Elevation;

        /// <summary>
        /// The azimuth angle
        /// </summary>
        public readonly Angle Azimuth;

        #endregion

        #region Constructors

        public AnglePair(Angle elevation, Angle azimuth)
        {
            Elevation = elevation;
            Azimuth = azimuth;
        }

        #endregion
    }
}

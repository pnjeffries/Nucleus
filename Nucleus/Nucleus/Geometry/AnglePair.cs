using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Structure to represent pairs of angles.
    /// Immutable.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Create a new AnglePair
        /// </summary>
        /// <param name="elevation"></param>
        /// <param name="azimuth"></param>
        public AnglePair(Angle elevation, Angle azimuth)
        {
            Elevation = elevation;
            Azimuth = azimuth;
        }

        #endregion
    }
}

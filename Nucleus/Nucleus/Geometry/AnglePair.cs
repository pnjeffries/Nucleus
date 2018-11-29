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
        /// The elevation angle, or latitude
        /// </summary>
        public readonly Angle Elevation;

        /// <summary>
        /// The azimuth angle, or longitude
        /// </summary>
        public readonly Angle Azimuth;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new AnglePair
        /// </summary>
        /// <param name="elevation">The elevation angle, or latitude</param>
        /// <param name="azimuth">The azimuth angle, or longitude</param>
        public AnglePair(Angle elevation, Angle azimuth)
        {
            Elevation = elevation;
            Azimuth = azimuth;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create an angle pair from values expressed in degrees
        /// </summary>
        /// <param name="elevation">The elevation angle, or latitude</param>
        /// <param name="azimuth">The azimuth angle, or longitude</param>
        /// <returns></returns>
        public static AnglePair FromDegrees(double elevation, double azimuth)
        {
            return new AnglePair(Angle.FromDegrees(elevation), Angle.FromDegrees(azimuth));
        }

        #endregion
    }
}

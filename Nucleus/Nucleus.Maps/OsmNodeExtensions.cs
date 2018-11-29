using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Osm = OsmSharp;

namespace Nucleus.Maps
{
    /// <summary>
    /// Extension methods for the OsmSharp Node class
    /// </summary>
    public static class OsmNodeExtensions
    {
        /// <summary>
        /// Get the position of this node in m, relative to the specified origin
        /// </summary>
        /// <param name="node"></param>
        /// <param name="origin">The origin latitude and longitude</param>
        /// <returns></returns>
        public static Vector Position(this Osm.Node node, AnglePair origin)
        {
            return node.Position(origin.Elevation.Degrees, origin.Azimuth.Degrees);
        }

        /// <summary>
        /// Get the position of this node in m, relative to the specified origin
        /// </summary>
        /// <param name="node"></param>
        /// <param name="originLatitude">The origin latitude, in degrees</param>
        /// <param name="originLongitude">The origin longitude, in degrees</param>
        /// <returns></returns>
        public static Vector Position(this Osm.Node node, double originLatitude, double originLongitude)
        {
            return Vector.FromLatitudeAndLongitude(
                Angle.FromDegrees((double)node.Latitude), 
                Angle.FromDegrees((double)node.Longitude), 
                Angle.FromDegrees(originLatitude), 
                Angle.FromDegrees(originLongitude));
        }
    }
}

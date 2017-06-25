using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Osm = OsmSharp;

namespace Nucleus.Maps
{
    public static class OsmNodeExtensions
    {
        /// <summary>
        /// Get the position of this node in m, relative to the specified origin
        /// </summary>
        /// <param name="node"></param>
        /// <param name="originLatitude"></param>
        /// <param name="originLongitude"></param>
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

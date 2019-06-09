using DotSpatial.Data;
using DotSpatial.Topology;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG = Nucleus.Geometry;

namespace Nucleus.Shapefile
{
    /// <summary>
    /// Conversion class to take DotSpatial objects and convert them
    /// to equivalent .Nucleus types.
    /// </summary>
    public static class ToNucleus
    {
        /// <summary>
        /// Convert a DotSpatial coordinate to a .Nucleus vector
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static NG.Vector Convert(Coordinate coordinate)
        {
            return new NG.Vector(coordinate.X, coordinate.Y, coordinate.Z);
        }

        /// <summary>
        /// Convert a list of DotSpatial coordinates to .Nucleus vectors
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static NG.Vector[] Convert(IList<Coordinate> coordinates)
        {
            var result = new NG.Vector[coordinates.Count];
            for (int i = 0; i < coordinates.Count; i++)
            {
                result[i] = Convert(coordinates[i]);
            }
            return result;
        }

        /// <summary>
        /// Convert a DotSpatial feature to equivalent .Nucleus geometry
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static VertexGeometry Convert(IFeature feature)
        {
            var coords = Convert(feature.Coordinates);
            if (coords.Length > 0)
            {
                // Returned geometry type based on FeatureType
                if (feature.FeatureType == FeatureType.Point)
                    return new NG.Point(coords[0]);
                else if (feature.FeatureType == FeatureType.MultiPoint)
                    return new NG.Cloud(coords);
                else if (feature.FeatureType == FeatureType.Line)
                {
                    if (coords.Length == 2) return new NG.Line(coords[0], coords[1]);
                    else return new NG.PolyLine(coords);
                }
                else return new NG.PolyLine(true, coords); //Closed polyline is default
            }
            return null; // Failed...
        }
    }
}

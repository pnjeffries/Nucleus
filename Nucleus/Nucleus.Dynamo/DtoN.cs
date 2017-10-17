using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS = Autodesk.DesignScript.Geometry;

namespace Nucleus.Dynamo
{
    /// <summary>
    /// Converter class to convert Dynamo/DesignScript objects to equivalent Nucleus objects
    /// </summary>
    public static class DtoN
    {
        /// <summary>
        /// Convert a DesignScript point to a Nucleus Vector
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector Convert(DS.Point point)
        {
            return new Vector(point.X, point.Y, point.Z);
        }

        /// <summary>
        /// Convert a DesignScript Vector to a Nucleus Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector Convert(DS.Vector vector)
        {
            return new Vector(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Convert a DesignScript plane to a Nucleus one
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Plane Convert(DS.Plane plane)
        {
            return new Plane(Convert(plane.Origin), Convert(plane.XAxis), Convert(plane.YAxis));
        }

        /// <summary>
        /// Convert a DesignScript line to a Nucleus line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line Convert(DS.Line line)
        {
            return new Line(Convert(line.StartPoint), Convert(line.EndPoint));
        }


    }
}

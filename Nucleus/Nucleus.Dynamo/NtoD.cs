using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS = Autodesk.DesignScript.Geometry;
using Nucleus.Geometry;

namespace Nucleus.Dynamo
{
    /// <summary>
    /// Converter class to convert Nucleus geometry types to Dynamo equivalents
    /// </summary>
    public static class NtoD
    {
        /// <summary>
        /// Convert a Nucleus Vector to a DesignScript Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static DS.Point Convert(Vector pt)
        {
            return DS.Point.ByCoordinates(pt.X, pt.Y, pt.Z);
        }

        /// <summary>
        /// Convert a Nucleus Vector to a DesignScript Vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DS.Vector ConvertVector(Vector v)
        {
            return DS.Vector.ByCoordinates(v.X, v.Y, v.Z);
        }

        /// <summary>
        /// Convert a Nucleus Plane to a DesignScript Plane
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static DS.Plane Convert(Plane plane)
        {
            return DS.Plane.ByOriginNormalXAxis(Convert(plane.Origin),
                ConvertVector(plane.X), ConvertVector(plane.Y));
        }

        /// <summary>
        /// Convert a Nucleus Line to a DesignScript Line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static DS.Line Convert(Line line)
        {
            return DS.Line.ByStartPointEndPoint(Convert(line.StartPoint), Convert(line.EndPoint));
        }

        /// <summary>
        /// Convert a Nucleus Arc to a DesignScript one
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static DS.Arc Convert(Arc arc)
        {
            return DS.Arc.ByCenterPointStartPointEndPoint(Convert(arc.Circle.Origin), Convert(arc.StartPoint), Convert(arc.EndPoint));
        }


    }
}

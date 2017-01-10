using FreeBuild.Geometry;
using FreeBuild.Rendering;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nDE = netDxf.Entities;

namespace FreeBuild.DXF
{
    /// <summary>
    /// A class of static helper functions to convert from FreeBuild datatypes
    /// to netDXF ones
    /// </summary>
    public static class FBtoDXF
    {
        /// <summary>
        /// Get or set the scaling factor to be used when converting DXF entities to FreeBuild ones
        /// </summary>
        public static double ConversionScaling { get; set; } = 1.0;

        /// <summary>
        /// Set the display and other attributes of a netDXF entity from those of an FreeBuild object
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributes"></param>
        public static void SetAttributes(nDE.EntityObject entity, GeometryAttributes attributes)
        {
            if (attributes != null)
            {
                if (attributes.Brush != null) entity.Color = Convert(attributes.Brush.BaseColour);
            }
        }

        /// <summary>
        /// Convert a FreeBuild colour to a netDXF one
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static AciColor Convert(Colour colour)
        {
            return new AciColor(colour.R, colour.G, colour.B);
        }

        /// <summary>
        /// Convert a FreeBuild vector to a netDXF one
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scaling"></param>
        /// <returns></returns>
        public static Vector3 Convert(Vector vector, double scaling = 0)
        {
            if (scaling == 0) scaling = ConversionScaling;
            return new Vector3(vector.X * scaling, vector.Y * scaling, vector.Z * scaling);
        }

        /// <summary>
        /// Convert a FreeBuild vector to a netDXF Vector2
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scaling"></param>
        /// <returns></returns>
        public static Vector2 ConvertToVector2(Vector vector, double scaling = 0)
        {
            if (scaling == 0) scaling = ConversionScaling;
            return new Vector2(vector.X * scaling, vector.Y * scaling);
        }

        /// <summary>
        /// Convert a FreeBuild transformation matrix to a netDXF one
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Matrix3 Convert(Transform t)
        {
            return new Matrix3(
                t[0, 0], t[0, 1], t[0, 2],
                t[1, 0], t[1, 1], t[1, 2],
                t[2, 0], t[2, 1], t[2, 2]);
        }

        /// <summary>
        /// Convert a FreeBuild point object to a netDXF one
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static nDE.Point Convert(Point pt)
        {
            var result = new nDE.Point(Convert(pt.Position));
            SetAttributes(result, pt.Attributes);
            return result;
        }
    }
}

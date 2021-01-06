using Nucleus.Geometry;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using U = UnityEngine;
using N = Nucleus.Geometry;


namespace Nucleus.Unity
{
    /// <summary>
    /// Static helper conversion class to convert Nucleus geometry
    /// to native Unity equivalents.
    /// Note that by convention in .Nucleus the Z-axis is vertical while
    /// in Unity it is the Y-axis.
    /// </summary>
    public static class ToUnity
    {
        /// <summary>
        /// Convert a .Nucleus vector to a 3D Unity equivalent
        /// </summary>
        /// <param name="v">The .Nucleus vector to convert</param>
        /// <returns></returns>
        public static Vector3 Convert(Vector v)
        {
            return new Vector3((float)v.X, (float)v.Z, (float)v.Y);
        }

        /// <summary>
        /// Convert an array of .Nucleus vectors to a 3D Unity equivalent
        /// </summary>
        /// <param name="vs">The array of .Nucleus vectors to convert</param>
        /// <param name="close">If true, the first point will be repeated at the end of the list</param>
        /// <returns></returns>
        public static Vector3[] Convert(Vector[] vs, bool close = false)
        {
            int length = vs.Length;
            if (close) length += 1;
            var result = new Vector3[length];
            for (int i = 0; i < vs.Length; i++)
            {
                result[i] = Convert(vs[i]);
            }
            if (close && vs.Length > 0) result[vs.Length] = Convert(vs[0]);
            return result;
        }

        /// <summary>
        /// Convert a .Nucleus vector to a 2D Unity equivalent
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Convert2D(Vector v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

        /// <summary>
        /// Convert a .Nucleus bounding box to a Unity one
        /// </summary>
        /// <param name="bBox"></param>
        /// <returns></returns>
        public static Bounds Convert(BoundingBox bBox)
        {
            return new Bounds(Convert(bBox.MidPoint), Convert(bBox.SizeVector));
        }

        /// <summary>
        /// Convert a .Nucleus colour to a Unity color
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Color Convert(Colour colour)
        {
            return new Color(
                colour.R / (float)byte.MaxValue,
                colour.G / (float)byte.MaxValue,
                colour.B / (float)byte.MaxValue,
                colour.A / (float)byte.MaxValue);
        }

        /// <summary>
        /// Convert a .Nucleus colour to a Unity color32
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static U.Color32 Convert32(Colour colour)
        {
            return new U.Color32(colour.R, colour.G, colour.B, colour.A);
        }

        /// <summary>
        /// Convert a .Nucleus axis to a Unity ray
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static U.Ray Convert(N.Axis axis)
        {
            return new U.Ray(Convert(axis.Origin), Convert(axis.Direction));
        }

        /// <summary>
        /// Convert a .Nucleus plane to a Unity plane
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static U.Plane Convert(N.Plane plane)
        {
            return new U.Plane(
                Convert(plane.Origin), 
                Convert(plane.LocalToGlobal(0, 1)), 
                Convert(plane.LocalToGlobal(1, 0)));
        }

    }
}

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
        /// Convert a .Nucleus axis to a Unity ray
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static U.Ray Convert(N.Axis axis)
        {
            return new U.Ray(Convert(axis.Origin), Convert(axis.Direction));
        }

    }
}

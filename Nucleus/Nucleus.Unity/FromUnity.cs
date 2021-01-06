using Nucleus.Geometry;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using N = Nucleus.Geometry;
using U = UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Conversion helper class to convert Unity native format geometry
    /// types into their .Nucleus equivalent
    /// </summary>
    public static class FromUnity
    {
        /// <summary>
        /// Convert a 3D Unity Vector to a .Nucleus one
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector Convert(Vector3 v)
        {
            return new Vector(v.x, v.z, v.y);
        }

        /// <summary>
        /// Convert a 2D Unity Vector to a .Nucleus one
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector Convert(Vector2 v)
        {
            return new Vector(v.x, v.y);
        }

        /// <summary>
        /// Convert a Unity bounding box to a .Nucleus one
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static BoundingBox Convert(UnityEngine.Bounds bounds)
        {
            return new BoundingBox(Convert(bounds.min), Convert(bounds.max));
        }

        /// <summary>
        /// Convert a Unity Color to a .Nucleus Colour
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Colour Convert(Color color)
        {
            return new Colour(color.a, color.r, color.g, color.b);
        }

    }
}

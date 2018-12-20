using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U = UnityEngine;
using Nucleus.Geometry;

namespace Nucleus.Unity
{
    /// <summary>
    /// Extension methods for Nucleus Vectors
    /// </summary>
    public static class NucleusVectorExtensions
    {
        /// <summary>
        /// Convert this Nucleus Vector to a 3D Unity equivalent
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <remarks>Shortcut to ToUnity.Convert</remarks>
        public static U.Vector3 ToUnityVector3(this Vector v)
        {
            return ToUnity.Convert(v);
        }

        /// <summary>
        /// Convert this Nucleus Vector to a 2D Unity equivalent
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static U.Vector2 ToUnityVector2(this Vector v)
        {
            return ToUnity.Convert2D(v);
        }
    }
}

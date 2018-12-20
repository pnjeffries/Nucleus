using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U = UnityEngine;
using Nucleus.Geometry;

namespace Nucleus.Unity.Extensions
{
    /// <summary>
    /// Extension methods for Unity's Vector3
    /// </summary>
    public static class UnityVector3Extensions
    {
        /// <summary>
        /// Convert this Vector3 to an equivalent Nucleus Vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <remarks>
        /// Shortcut to FromUnity.Convert
        /// </remarks>
        public static Vector ToNucleusVector(this U.Vector3 v)
        {
            return FromUnity.Convert(v);
        }
    }
}

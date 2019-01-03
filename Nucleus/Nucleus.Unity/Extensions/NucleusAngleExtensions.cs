using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Extension methods for the Nucleus Angle struct
    /// </summary>
    public static class NucleusAngleExtensions
    {
        /// <summary>
        /// Convert this Nucleus Angle to a Unity Quaternion representing a rotation of
        /// that angle around the vertical axis.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quaternion ToUnityQuaternion(this Angle angle)
        {
            return Quaternion.AngleAxis(90f - (float)angle.Degrees, new Vector3(0, 1, 0));
        }
    }
}

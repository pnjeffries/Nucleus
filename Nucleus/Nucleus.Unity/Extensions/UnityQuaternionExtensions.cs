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
    /// Extension methods for the unity quaternion class
    /// </summary>
    public static class UnityQuaternionExtensions
    {
        /// <summary>
        /// Converts this quaternion to a Nucleus angle representing a rotation around the
        /// vertical axis
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Angle ToNucleusAngle(this Quaternion quaternion)
        { 
            return Angle.FromDegrees(-quaternion.eulerAngles.y + 90).NormalizeTo2PI();
        }
    }
}

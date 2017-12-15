using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rhino
{
    public static class Vector3dExtensions
    {
        /// <summary>
        /// Calculate the dot, or scalar, product of this and another vector.
        /// Provides the component of this vector in the direction of (or, the projection onto) the other vector.
        /// </summary>
        /// <param name="other">The vector to project this vector onto.  
        /// If the length of the projection is required, this should be a unit vector.</param>
        /// <returns>The dot product as a double.</returns>
        public static double Dot(this Vector3d v, Vector3d other)
        {
            return v.X * other.X + v.Y * other.Y + v.Z * other.Z;
        }
    }
}

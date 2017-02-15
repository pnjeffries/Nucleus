using FreeBuild.Geometry;
using FreeBuild.Rendering;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.OpenTK
{
    /// <summary>
    /// Static conversion functions to go between FreeBuild and OpenTK data types
    /// </summary>
    public static class FBtoOTK
    {
        /// <summary>
        /// Convert a FreeBuild vector to a OpenTK Vector3
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 Convert(Vector vector)
        {
            return new Vector3((float)vector.X, (float)vector.Y, (float)vector.Z);
        }

        /// <summary>
        /// Convert a FreeBuild colour to a OpenTK Color4
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Color4 Convert(Colour colour)
        {
            return new Color4(colour.R, colour.G, colour.B, colour.A);
        }
    }
}

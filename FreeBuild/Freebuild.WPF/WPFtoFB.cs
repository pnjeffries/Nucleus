using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W = System.Windows;

namespace FreeBuild.WPF
{
    /// <summary>
    /// Static conversion class to translate WPF objects into FreeBuild ones
    /// </summary>
    public static class WPFtoFB
    {
        /// <summary>
        /// Convert a WPF point to a FreeBuild Vector
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector Convert(W.Point point)
        {
            return new Vector(point.X, -point.Y);
        }
    }
}

using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W = System.Windows;

namespace Nucleus.WPF
{
    /// <summary>
    /// Static conversion class to translate WPF objects into Nucleus ones
    /// </summary>
    public static class WPFtoFB
    {
        /// <summary>
        /// Convert a WPF point to a Nucleus Vector
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector Convert(W.Point point)
        {
            return new Vector(point.X, -point.Y);
        }
    }
}

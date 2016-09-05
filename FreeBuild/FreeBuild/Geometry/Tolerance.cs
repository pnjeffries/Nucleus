using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Static class containing mutable tolerance values
    /// </summary>
    public static class Tolerance
    {
        /// <summary>
        /// The current geometric tolerance used to determine coincidence
        /// </summary>
        public static double Geometric { get; set; } = 0.0001;
    }
}

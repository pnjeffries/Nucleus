using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Interface for 'simple' curve types which are atomic
    /// and of only one span, such as line segments and arcs
    /// </summary>
    public interface ISimpleCurve
    {
        /// <summary>
        /// Get the curve representation of this SimpleCurve
        /// </summary>
        Curve Curve { get; }

        /// <summary>
        /// Find the closest point on this curve to a test point, expressed as a
        /// parameter value from 0-1.  This may be a position on the curve or it may
        /// be the start (0) or end (1) of the curve depending on the relative location
        /// of the test point.
        /// </summary>
        /// <param name="toPoint">The test point to find the closest point to</param>
        /// <returns></returns>
        double ClosestParameter(Vector toPoint);
    }
}

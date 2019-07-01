using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Extensions;

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
        /// Get the length of this curve
        /// </summary>
        double Length { get; }

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

    /// <summary>
    /// Extension methods for the ISimpleCurve interface
    /// </summary>
    public static class ISimpleCurveExtensions
    {
        /// <summary>
        /// Get the longest curve in this collection
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static ISimpleCurve Longest(this IList<ISimpleCurve> curves)
        {
            return curves.ItemWithMax(i => i.Length);
        }
    }
}

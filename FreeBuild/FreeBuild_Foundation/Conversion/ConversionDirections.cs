using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Enum representing the possible directions of a data conversion
    /// </summary>
    public enum ConversionDirections
    {
        /// <summary>
        /// Conversions in both directions are possible
        /// </summary>
        Both = 0,
        /// <summary>
        /// This conversion mapping applies from type A to type B, but not the other way
        /// </summary>
        AtoB = 1,
        /// <summary>
        /// This conversion mapping applies from type B to type A, but not the other way
        /// </summary>
        BtoA = 2
    }

    /// <summary>
    /// ConversionDirections extension helper methods
    /// </summary>
    public static class ConversionDirectionsExtensions
    {
        /// <summary>
        /// Invert this conversion direction
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConversionDirections Invert(this ConversionDirections value)
        {
            if (value == ConversionDirections.AtoB) return ConversionDirections.BtoA;
            else if (value == ConversionDirections.BtoA) return ConversionDirections.AtoB;
            else return ConversionDirections.Both;
        }
    }
}

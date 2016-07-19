using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Units
{
    /// <summary>
    /// Enum to represent different 
    /// </summary>
    [Serializable]
    public enum DimensionTypes
    {
        /// <summary>
        /// A quantity to which no physical dimension is applicable.
        /// </summary>
        Dimensionless = 0,

        /// <summary>
        /// A distance, or length, measurement.
        /// </summary>
        Distance = 1,

        /// <summary>
        /// An angle or rotation measurement.
        /// </summary>
        Angle = 2
       
        //TODO: Add more dimension types as and when needed
    }

}

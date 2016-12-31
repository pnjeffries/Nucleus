using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Results
{
    /// <summary>
    /// Enum of node analysis result types
    /// </summary>
    public enum NodeResultTypes
    {
        /// <summary>
        /// The nodal displacement
        /// </summary>
        Displacement = 100,

        /// <summary>
        /// The nodal rotation
        /// </summary>
        Rotation = 110,

        /// <summary>
        /// The reaction forces on the node.
        /// </summary>
        Force = 200,

        /// <summary>
        /// The reaction moments on the node.
        /// </summary>
        Moments = 210,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Enum of standard node analysis result types
    /// </summary>
    [Serializable]
    public enum NodeResultTypes
    {
        /// <summary>
        /// The nodal displacement magnitude
        /// </summary>
        Displacement = 100,
        
        /// <summary>
        /// The X-component of the nodal displacement
        /// </summary>
        Displacement_X = 101,

        /// <summary>
        /// The Y-component of the nodal displacement
        /// </summary>
        Displacement_Y = 102,

        /// <summary>
        /// The Z-component of the nodal displacement
        /// </summary>
        Displacement_Z = 103,

        /// <summary>
        /// The nodal rotation magnitude
        /// </summary>
        Rotation = 110,

        /// <summary>
        /// The component of rotation about the X axis
        /// </summary>
        Rotation_XX = 111,

        /// <summary>
        /// The component of rotation about the Y axis
        /// </summary>
        Rotation_YY = 112,

        /// <summary>
        /// The component of rotation about the Z axis
        /// </summary>
        Rotation_ZZ = 113,

        /// <summary>
        /// The magnitude of reaction forces on the node.
        /// </summary>
        Force = 200,

        /// <summary>
        /// The reaction force component acting in the X-direction
        /// </summary>
        Force_X = 201,

        /// <summary>
        /// The reaction force component acting in the Y-direction
        /// </summary>
        Force_Y = 202,

        /// <summary>
        /// The reaction force componen acting in the Z-direction
        /// </summary>
        Force_Z = 203,

        /// <summary>
        /// The combined reaction moments on the node.
        /// </summary>
        Moments = 210,

        /// <summary>
        /// The reaction moments acting about the X-axis.
        /// </summary>
        Moments_XX = 211,

        /// <summary>
        /// The reaction moments acting about the Y-axis.
        /// </summary>
        Moments_YY = 212,

        /// <summary>
        /// The reaction moments acting about the Z-axis.
        /// </summary>
        Moments_ZZ = 213,
    }
}

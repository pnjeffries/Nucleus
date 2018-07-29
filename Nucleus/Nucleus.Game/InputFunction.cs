using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    public enum InputFunction
    {
        /// <summary>
        /// The input function is not defined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Upwards movement
        /// </summary>
        Up = 1000,

        /// <summary>
        /// Rightwards movement
        /// </summary>
        Right = 1100,

        /// <summary>
        /// Downwards movement
        /// </summary>
        Down = 1200,

        /// <summary>
        /// Leftwards movement
        /// </summary>
        Left = 1300,
        
    }

    /// <summary>
    /// Extension methods for the InputFunction enum
    /// </summary>
    public static class InputFunctionExtensions
    {
        /// <summary>
        /// Get the vector which indicates the direction associated with this input
        /// (if any)
        /// </summary>
        /// <param name="iF"></param>
        /// <returns></returns>
        public static Vector DirectionVector(this InputFunction iF)
        {
            switch (iF)
            {
                case InputFunction.Up:
                    return new Vector(0, 1);
                case InputFunction.Down:
                    return new Vector(0, -1);
                case InputFunction.Left:
                    return new Vector(-1, 0);
                case InputFunction.Right:
                    return new Vector(1, 0);
                default:
                    return Vector.Unset;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Enum to represent a direction either to the left or right
    /// </summary>
    public enum HandSide
    {
        /// <summary>
        /// The side is undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The right-hand side
        /// </summary>
        Right = -1,
        
        /// <summary>
        /// The left-hand side
        /// </summary>
        Left = 1,
    }

    /// <summary>
    /// Extension methods for the HandSide enum
    /// </summary>
    public static class HandSideExtensions
    {
        /// <summary>
        /// Return the opposite side
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public static HandSide Flip(this HandSide side)
        {
            if (side == HandSide.Left) return HandSide.Right;
            else return HandSide.Left;
        }

        /// <summary>
        /// Returns either 1 or -1 depending on whether this side
        /// would typically require a positive or negative curve
        /// offset value.
        /// If the side is undefined, returns 0.
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public static int OffsetSign(this HandSide side)
        {
            if (side == HandSide.Left) return 1;
            else if (side == HandSide.Right) return -1;
            else return 0;
        }
    }
}

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
    }
}

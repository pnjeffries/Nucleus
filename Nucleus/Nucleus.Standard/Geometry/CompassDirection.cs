using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// An enumerated value representing the points of the compass
    /// </summary>
    [Serializable]
    public enum CompassDirection
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    /// <summary>
    /// Extension methods for the CompassDirection enum
    /// </summary>
    public static class CompassDirectionExtensions
    {
        /// <summary>
        /// Find the next compass direction clockwise from the current one
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static CompassDirection TurnClockwise(this CompassDirection direction)
        {
            if (direction == CompassDirection.North) return CompassDirection.East;
            else if (direction == CompassDirection.East) return CompassDirection.South;
            else if (direction == CompassDirection.South) return CompassDirection.West;
            else return CompassDirection.North;
        }

        /// <summary>
        /// Find the next compass direction anticlockwise from the current one
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static CompassDirection TurnAntiClockwise(this CompassDirection direction)
        {
            if (direction == CompassDirection.North) return CompassDirection.West;
            else if (direction == CompassDirection.East) return CompassDirection.North;
            else if (direction == CompassDirection.South) return CompassDirection.East;
            else return CompassDirection.South;
        }

        /// <summary>
        /// Flip this direction to face the opposite way
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static CompassDirection Reverse(this CompassDirection direction)
        {
            if (direction == CompassDirection.North) return CompassDirection.South;
            else if (direction == CompassDirection.East) return CompassDirection.West;
            else if (direction == CompassDirection.South) return CompassDirection.North;
            else return CompassDirection.East;
        }

        /// <summary>
        /// Is this direction 'vertical' (i.e. North/South)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool IsVertical(this CompassDirection direction)
        {
            if (direction == CompassDirection.North || direction == CompassDirection.South) return true;
            else return false;
        }

        /// <summary>
        /// Is this direction 'horizontal' (i.e. East/West)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool IsHorizontal(this CompassDirection direction)
        {
            if (direction == CompassDirection.East || direction == CompassDirection.West) return true;
            else return false;
        }

        /// <summary>
        /// Generate a random direction other than this one
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static CompassDirection RandomOther(this CompassDirection direction, Random rng)
        {
            int rando = rng.Next(0, 3);
            if (rando == 0) return direction.TurnAntiClockwise();
            else if (rando == 1) return direction.TurnClockwise();
            else return direction.Reverse();
        }

        /// <summary>
        /// Generate a random direction at right angles to this one
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static CompassDirection RandomTurn(this CompassDirection direction, Random rng)
        {
            int rando = rng.Next(0, 2);
            if (rando == 0) return direction.TurnAntiClockwise();
            else return direction.TurnClockwise();
        }
    }
}

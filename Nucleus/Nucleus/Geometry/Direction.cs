using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Six-axis dimensional direction, consisting of the standard three
    /// translational dimensions and rotations about those axes.
    /// Used to determine the application of directional loads.
    /// </summary>
    public enum Direction
    {
        X = 1,
        Y = 2,
        Z = 4,
        XX = 8,
        YY = 16,
        ZZ = 32
    }

    /// <summary>
    /// Extension methods for the Direction enum
    /// </summary>
    public static class DirectionExtensions
    {
        /// <summary>
        /// Does this value represent a translation along an axis?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTranslation(this Direction value)
        {
            return (value <= Direction.Z);
        }

        /// <summary>
        /// Does this value represent a rotation about an axis?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsRotation(this Direction value)
        {
            return (value >= Direction.XX);
        }

        /// <summary>
        /// The vector which describes the axis along
        /// or about which the direction applies.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector Vector(this Direction value)
        {
            return new Vector(value);
        }

        /// <summary>
        /// Get the direction of the first direction perpendicular to this one
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Direction FirstPerpendicular (this Direction value)
        {
            if (value == Direction.Z) return Direction.X;
            else if (value == Direction.X) return Direction.Y;
            else if (value == Direction.Y) return Direction.Z;
            else if (value == Direction.ZZ) return Direction.XX;
            else if (value == Direction.XX) return Direction.YY;
            else return Direction.ZZ;
        }

        /// <summary>
        /// Get the direction of the second direction perpendicular to this one
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Direction SecondPerpendicular(this Direction value)
        {
            if (value == Direction.Z) return Direction.Y;
            else if (value == Direction.X) return Direction.Z;
            else if (value == Direction.Y) return Direction.X;
            else if (value == Direction.ZZ) return Direction.YY;
            else if (value == Direction.XX) return Direction.ZZ;
            else return Direction.XX;
        }
    }
}
using Nucleus.Base;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for 
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random floating-point number between 0 and maxValue
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="maxValue">The maximum extent of the random range</param>
        /// <returns></returns>
        public static double NextDouble(this Random rng, double maxValue)
        {
            return rng.NextDouble() * maxValue;
        }

        /// <summary>
        /// Returns a random floating-point number between minValue and maxValue
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="minValue">The minimum extent of the random range</param>
        /// <param name="maxValue">The maximum extent of the random range</param>
        /// <returns></returns>
        public static double NextDouble(this Random rng, double minValue, double maxValue)
        {
            return minValue + rng.NextDouble() * (maxValue - minValue);
        }

        /// <summary>
        /// Returns a random floating-point number within range of origin.
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="origin">The number to generate close to</param>
        /// <param name="range">The difference either side of origin that gives
        /// the acceptable range.</param>
        /// <returns></returns>
        public static double NextDoubleNear(this Random rng, double origin, double range)
        {
            return origin + rng.NextDouble(-range, range);
        }

        /// <summary>
        /// Returns a random angle between 0 and 2*PI radians
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static Angle NextAngle(this Random rng)
        {
            return new Angle(rng.NextDouble(Math.PI));
        }

        /// <summary>
        /// Returns a random cardinal direction
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static CompassDirection NextDirection(this Random rng)
        {
            return (CompassDirection)rng.Next(4);
        }

        /// <summary>
        /// Return a random gender
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static Gender NextGender(this Random rng)
        {
            return rng.NextBoolean() ? Gender.Masculine : Gender.Feminine;
        }

        /// <summary>
        /// Generate a random point within the specified bounding box
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="bounds">The bounding box within which the point
        /// is to be generated.</param>
        /// <returns></returns>
        public static Vector NextPoint(this Random rng, BoundingBox bounds)
        {
            return bounds.RandomPointInside(rng);
        }

        /// <summary>
        /// Generate a random point within the specified range of the given origin
        /// point.
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Vector NextPoint(this Random rng, Vector origin, double range)
        {
            // TODO: square distance to give more even distribution?
            return origin + new Geometry.Vector(rng.NextAngle()) * rng.NextDouble(range);
        }

        /// <summary>
        /// Generate a random boolean value
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static bool NextBoolean(this Random rng)
        {
            return rng.NextDouble() > 0.5;
        }

    }
}

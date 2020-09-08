using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// An interface for objects which posess a bounding box
    /// </summary>
    public interface IBoxBoundable
    {
        /// <summary>
        /// Get the bounding box of this geometry
        /// </summary>
        BoundingBox BoundingBox { get; }
    }

    /// <summary>
    /// Extension methods for IBoxBoundable objects and collections thereof
    /// </summary>
    public static class IBoxBoundableExtensions
    {
        /// <summary>
        /// Create a bounding box which contains all geometry in this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static BoundingBox CreateBoundingBox<T>(this IEnumerable<T> collection)
            where T : IBoxBoundable
        {
            return BoundingBox.CreateBoxAround(collection);
        }
    }
}

using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    /// <summary>
    /// A DDTree used for the spatial partitioning of mesh faces
    /// </summary>
    [Serializable]
    public class MeshFaceDDTree : DDTree<MeshFace>
    {
        #region Constructor

        /// <summary>
        /// Creates a new DDTree populated with the specified collection of objects
        /// </summary>
        /// <param name="items">The objects to include within the tree.</param>
        /// <param name="maxDivisions">The maximum number of cells into which each 
        /// level in the tree should be divided</param>
        /// <param name="minCellSize">The minimum allowable size of a cell.  Once a node
        /// reaches this size it will no longer subdivide regardless of how many items are
        /// contained within it.</param>
        /// <param name="maxLeafPopulation">The maximum population per leaf node.  If the
        /// number of objects within a cell exceeds this number and the minimum cell size 
        /// has not yet been reached, the node will subdivide</param>
        public MeshFaceDDTree(IList<MeshFace> items, int maxDivisions = 10, 
            double minCellSize = 1, int maxLeafPopulation = 4)
            : base(items, maxDivisions, minCellSize, maxLeafPopulation)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Trace a ray through this tree, testing for intersections with item geometry.
        /// Returns information about the first intersection encountered.
        /// </summary>
        /// <param name="ray">The ray to test</param>
        /// <returns></returns>
        public RayHit<MeshFace> RayTrace(Axis ray)
        {
            return RayTrace(ray, HitTest);
        }

        /// <summary>
        /// Trace a ray through this tree, testing for intersections with item geometry.
        /// Returns information about the first intersection encountered within the specified
        /// range.
        /// </summary>
        /// <param name="ray">The ray to test</param>
        /// <param name="maxRange">The maximum range of the ray, expressed as the maximum parameter
        /// on the ray beyond which hits should be ignored.
        /// <returns></returns>
        public RayHit<MeshFace> RayTrace(Axis ray, double maxRange)
        {
            return RayTrace(ray, HitTest, maxRange);
        }

        private double HitTest(MeshFace item, Axis ray)
        {
            return Intersect.RayFace(ray.Origin, ray.Direction, item);
        }

        public override double DistanceSquaredBetween(Vector pt, MeshFace entry)
        {
            return pt.DistanceToSquared(entry.ClosestPoint(pt));
        }

        public override double MaxXOf(MeshFace entry)
        {
            return entry.MaxDelegateValue(i => i.X);
        }

        public override double MaxYOf(MeshFace entry)
        {
            return entry.MaxDelegateValue(i => i.Y);
        }

        public override double MaxZOf(MeshFace entry)
        {
            return entry.MaxDelegateValue(i => i.Z);
        }

        public override double MinXOf(MeshFace entry)
        {
            return entry.MinDelegateValue(i => i.X);
        }

        public override double MinYOf(MeshFace entry)
        {
            return entry.MinDelegateValue(i => i.Y);
        }

        public override double MinZOf(MeshFace entry)
        {
            return entry.MinDelegateValue(i => i.Z);
        }

        public override double PositionInDimension(CoordinateAxis dimension, MeshFace entry)
        {
            return entry.AverageDelegateValue(i => i.Position[dimension]);
        }

        #endregion
    }
}

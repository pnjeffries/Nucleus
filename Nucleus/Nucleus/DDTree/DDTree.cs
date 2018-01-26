// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    /// <summary>
    /// The abstract base class for Divided-Dimension Trees.
    /// To implement a tree to hold a particular type of object, this class should be extended
    /// and the abstract functions overridden to deal with that type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DDTree<T>
    {
        /// <summary>
        /// The root node of the tree
        /// </summary>
	    private DDTreeNode<T> _RootNode;

        /// <summary>
        /// The maximum allowable number of divisions per tree
        /// </summary>
        public int MaxDivisions { get; set; } = 10;

        /// <summary>
        /// The minimum allowable cell size
        /// </summary>
        public double MinCellSize { get; set; } = 1;

        /// <summary>
        /// The requested maximum number of objects per leaf node.
        /// If newly added objects increase the object count beyond this, the node will automatically subdivide.
        /// Note that the MinCellSize limit may mean that some nodes will contain more than this number.
        /// </summary>
        public int MaxLeafPopulation { get; set; } = 4;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        /// <param name="maxDivisions"></param>
        /// <param name="minCellSize"></param>
        protected DDTree(IList<T> items, int maxDivisions = 10, double minCellSize = 1)
        {
            MaxDivisions = maxDivisions;
            MinCellSize = minCellSize;
            _RootNode = new DDTreeNode<T>(this);
            foreach (T item in items)
            {
                _RootNode.Children.Add(item);
            }
            _RootNode.Subdivide();
        }

        /// <summary>
        /// Find the closest item in the tree to the specified point within the specified maximum distance
        /// </summary>
        /// <param name="pt">The point to search from</param>
        /// <param name="maxDistance">The maximum distance</param>
        /// <param name="ignore">Optional.  A value in the tree which is to be ignored.</param>
        /// <returns></returns>
        public T NearestTo(Vector pt, double maxDistance, T ignore = default(T))
        {
            double distanceSquared = maxDistance * maxDistance;
            return _RootNode.NearestTo(pt, ref distanceSquared, ignore);
        }

        /// <summary>
        /// Find all items within the specified bounding box
        /// </summary>
        /// <param name="box">The bounding box to check</param>
        /// <param name="items">A collection to be populated with the items inside the box</param>
        public void ItemsInside(BoundingBox box, ref IList<T> output)
        {
            _RootNode.ItemsInside(box, ref output);
        }

        /// <summary>
        /// Find all items within the specified distance from the given point
        /// </summary>
        /// <param name="pt">The point to check distance to</param>
        /// <param name="maxDistance">The maximum distance within which items will be included</param>
        /// <param name="output">A collection to be populated with all the items close to the specified point</param>
        public void CloseTo(Vector pt, double maxDistance, ref IList<T> output)
        {
            double distanceSquared = maxDistance * maxDistance;
            _RootNode.CloseTo(pt, distanceSquared, ref output);
        }

        /// <summary>
        /// Trace a ray through this tree, testing for intersections with item geometry.
        /// Returns information about the first intersection encountered.
        /// </summary>
        /// <param name="ray">The ray to test</param>
        /// <param name="hitTest">A delegate function to determine whether an item in the
        /// tree has been hit by the ray.  Should take in the object and ray as parameters
        /// and return the ray intersection parameter on a hit or double.NaN on a miss.</param>
        /// <returns></returns>
        public RayHit<T> RayTrace(Axis ray, Func<T, Axis, double> hitTest)
        {
            return _RootNode.RayTrace(ray, hitTest);
        }

        /// <summary>
        /// Trace a ray through this tree, testing for intersections with item geometry.
        /// Returns information about the first intersection encountered.
        /// </summary>
        /// <param name="ray">The ray to test</param>
        /// <param name="hitTest">A delegate function to determine whether an item in the
        /// tree has been hit by the ray.  Should take in the object and ray as parameters
        /// and return the ray intersection parameter on a hit or double.NaN on a miss.</param>
        /// <param name="maxRange">The maximum range of the ray.  Beyond this range hits will
        /// be ignored.</param>
        /// <returns></returns>
        public RayHit<T> RayTrace(Axis ray, Func<T, Axis, double> hitTest, double maxRange)
        {
            double tEnd = ray.ParameterAt(maxRange);
            RayHit<T> hit = _RootNode.RayTrace(ray, hitTest);
            if (hit != null && hit.Parameter < tEnd) return hit;
            else return null;
        }

        /// <summary>
        /// Add an item to the tree
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (!_RootNode.Contains(item)) _RootNode.Add(item);
        }

        /// <summary>
        /// Remove an item from the tree
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            _RootNode.Remove(item);
        }

        /// <summary>
        /// Find the minimum bounding X-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double MinXOf(T entry);

        /// <summary>
        /// Find the maximum bounding X-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double MaxXOf(T entry);

        /// <summary>
        /// Find the minimum bounding Y-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double MinYOf(T entry);

        /// <summary>
        /// Find the maximum bounding Y-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double MaxYOf(T entry);

        /// <summary>
        /// Find the minimum bounding Z-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double MinZOf(T entry);

        /// <summary>
        /// Find the maximum bounding Z-coordinate of the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double MaxZOf(T entry);

        /// <summary>
        /// Returns the minumum squared distance between the specified position in 3D-space and the given entry in the tree.
        /// Should be overridden to deal with the specific tree type.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double DistanceSquaredBetween(Vector pt, T entry);

        /*
        /// <summary>
        /// Returns the minimum squared distance between two entries in the tree.
        /// Should be overridden to deal with the specific tree type
        /// </summary>
        /// <param name="entryA"></param>
        /// <param name="entryB"></param>
        /// <returns></returns>
        public abstract double MinDistanceSquaredBetween(T entryA, T entryB);
        */

        /// <summary>
        /// Get the nominal position of the specified entry in the tree the specified dimensional axis.
        /// Should be overridden to deal with the specific tree type
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract double PositionInDimension(Dimension dimension, T entry);

        /// <summary>
        /// Get the position of the specified point along the specified dimension's axis
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public double PositionInDimension(Dimension dimension, Vector pt)
        {
            switch (dimension)
            {
                case Dimension.X:
                    return pt.X;
                case Dimension.Y:
                    return pt.Y;
                default:
                    return pt.Z;
            }
        }


        /// <summary>
        /// Overridable function which allows filtering of results for
        /// certain sub-types.  Returns true if the specified item
        /// is a valid result from tree search operations, false if
        /// it is not (if, for example, it is marked as deleted)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanReturn(T item)
        {
            return true;
        }
    }
}

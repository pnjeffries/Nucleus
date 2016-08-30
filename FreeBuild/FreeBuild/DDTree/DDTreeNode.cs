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

using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.DDTree
{
    /// <summary>
    /// A node in a D-D Tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DDTreeNode<T>
    {
        private double _Origin; //The origin axis position
        private double _CellSize; //The size of each division along the axis
        private DDTreeNode<T>[] _Branches; //The sub-branches of this node

        private IList<T> _Children;
        /// <summary>
        /// The child objects of this node
        /// </summary>
        public IList<T> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        private DDTree<T> _Tree; //The tree this node belongs to

        /// <summary>
        /// The dimensional axis along which the space in this node is divided.
        /// If this is Undefined, the node is not split.
        /// </summary>
        private Dimension _SplitDimension = Dimension.Undefined;

        /// <summary>
        /// Is this node a leaf node (i.e. is it not divided)
        /// </summary>
        public bool IsLeafNode
        {
            get { return _SplitDimension == Dimension.Undefined; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tree"></param>
        public DDTreeNode(DDTree<T> tree)
        {
            _Tree = tree;
            _Children = new List<T>();
        }

        /// <summary>
        /// Add a new item to this node
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _Children.Add(item);
            if (_SplitDimension != Dimension.Undefined)
            {
                AddToBranch(item);
            }
            else if (_Children.Count > _Tree.MaxLeafPopulation)  //Auto-split when number of children exceeds limit
            {
                Subdivide();
            }

        }

        /// <summary>
        /// Add an item to a branch node of this node
        /// </summary>
        /// <param name="item"></param>
        protected void AddToBranch(T item)
        {
            double value = _Tree.PositionInDimension(_SplitDimension, item);
            int index = (int)Math.Floor((value - _Origin) / _CellSize);
            if (index < 0) index = 0;
            else if (index >= _Branches.Length) index = _Branches.Length - 1;
            if (_Branches[index] == null) _Branches[index] = new DDTreeNode<T>(_Tree);
            _Branches[index].Add(item);
        }

        /// <summary>
        /// Remove an item from this node and all branch nodes that contain it
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            if (_Children.Contains(item))
            {
                _Children.Remove(item);
                if (_SplitDimension != Dimension.Undefined)
                {
                    for (int i = 0; i < _Branches.Length; i++)
                    {
                        DDTreeNode<T> branch = _Branches[i];
                        if (branch != null) branch.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Determine whether this node contains a specified child value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _Children.Contains(item);
        }

        /// <summary>
        /// Find all items within the given distance from the given point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="distanceSquared"></param>
        /// <param name="output"></param>
        public void CloseTo(Vector pt, double distanceSquared, ref IList<T> output)
        {
            if (_SplitDimension != Dimension.Undefined)
            {
                double value = _Tree.PositionInDimension(_SplitDimension, pt);
                //Check starting cell:
                double t = (value - _Origin) / _CellSize;
                int startIndex = (int)Math.Floor(t);
                if (startIndex < 0) startIndex = 0;
                else if (startIndex >= _Branches.Length) startIndex = _Branches.Length - 1;
                DDTreeNode<T> node = _Branches[startIndex];
                if (node != null) node.CloseTo(pt, distanceSquared, ref output);
                //Check surrounding cells within max distance:
                int offsetIndex = 1;
                double cellSizeSquared = _CellSize * _CellSize;
                double t0 = t - startIndex;
                double t1 = -t0;
                t0 -= 1;
                bool increase = true;
                bool decrease = true;
                while (increase || decrease)
                {
                    if (increase)
                    {
                        double pInd = offsetIndex + t1;
                        int i = startIndex + offsetIndex;
                        if (pInd * pInd * cellSizeSquared >= distanceSquared || i >= _Branches.Length) increase = false;
                        else
                        {
                            node = _Branches[i];
                            if (node != null)
                            {
                                node.CloseTo(pt, distanceSquared, ref output);
                            }
                        }
                    }

                    if (decrease)
                    {
                        double pInd = offsetIndex + t0;
                        int i = startIndex - offsetIndex;
                        if (pInd * pInd * cellSizeSquared >= distanceSquared || i < 0) decrease = false;
                        else
                        {
                            node = _Branches[i];
                            if (node != null)
                            {
                                node.CloseTo(pt, distanceSquared, ref output);
                            }
                        }
                    }
                    offsetIndex++;
                }
            }
            else
            {
                foreach (T item in _Children)
                {
                    double distSquaredTo = _Tree.DistanceSquaredBetween(pt, item);
                    if (distSquaredTo < distanceSquared && !output.Contains(item))
                    {
                        output.Add(item);
                    }
                }
            }
        }

        public void ItemsInside(BoundingBox box, ref IList<T> output)
        {
            if (_SplitDimension != Dimension.Undefined)
            {
                double min = box.MinInDimension(_SplitDimension);
                double max = box.MaxInDimension(_SplitDimension);
                double t0 = (min - _Origin) / _CellSize;
                double t1 = (max - _Origin) / _CellSize;
                int startIndex = (int)Math.Max(Math.Floor(t0), 0);
                int endIndex = (int)Math.Min(Math.Floor(t1), _Branches.Length - 1);

                for (int i = startIndex; i <= endIndex; i++)
                {
                    DDTreeNode<T> node = _Branches[i];
                    if (node != null)
                    {
                        node.ItemsInside(box, ref output);
                    }
                }
            }
            else
            {
                foreach (T item in _Children)
                {
                    if (_Tree.MaxXOf(item) >= box.MinX && _Tree.MinXOf(item) <= box.MaxX &&
                        _Tree.MaxYOf(item) >= box.MinY && _Tree.MinYOf(item) <= box.MaxY &&
                        _Tree.MaxZOf(item) >= box.MinZ && _Tree.MinZOf(item) <= box.MaxZ &&
                        !output.Contains(item))
                    {
                        output.Add(item);
                    }

                }
            }
        }

        /// <summary>
        /// Find the closest object to the specified point within the specified distance
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="distanceSquared"></param>
        /// <returns></returns>
        public T NearestTo(Vector pt, ref double distanceSquared, T ignore)
        {
            T result = default(T);

            if (_SplitDimension != Dimension.Undefined)
            {
                double value = _Tree.PositionInDimension(_SplitDimension, pt);
                //Check starting cell:
                double t = (value - _Origin) / _CellSize;
                int startIndex = (int)Math.Floor(t);
                if (startIndex < 0) startIndex = 0;
                else if (startIndex >= _Branches.Length) startIndex = _Branches.Length - 1;
                DDTreeNode<T> node = _Branches[startIndex];
                if (node != null) result = node.NearestTo(pt, ref distanceSquared, ignore);
                //Check surrounding cells within max distance:
                int offsetIndex = 1;
                double cellSizeSquared = _CellSize * _CellSize;
                double t0 = t - startIndex;
                double t1 = -t0;
                t0 -= 1;
                bool increase = true;
                bool decrease = true;
                while (increase || decrease)
                {
                    if (increase)
                    {
                        double pInd = offsetIndex + t1;
                        int i = startIndex + offsetIndex;
                        if (pInd * pInd * cellSizeSquared >= distanceSquared || i >= _Branches.Length) increase = false;
                        else
                        {
                            node = _Branches[i];
                            if (node != null)
                            {
                                T result2 = default(T);
                                result2 = node.NearestTo(pt, ref distanceSquared, ignore);
                                if (result2 != null) result = result2;
                            }
                        }
                    }

                    if (decrease)
                    {
                        double pInd = offsetIndex + t0;
                        int i = startIndex - offsetIndex;
                        if (pInd * pInd * cellSizeSquared >= distanceSquared || i < 0) decrease = false;
                        else
                        {
                            node = _Branches[i];
                            if (node != null)
                            {
                                T result2 = default(T);
                                result2 = node.NearestTo(pt, ref distanceSquared, ignore);
                                if (result2 != null) result = result2;
                            }
                        }
                    }
                    offsetIndex++;
                }
            }
            else
            {
                foreach (T item in _Children)
                {
                    double distSquaredTo = _Tree.DistanceSquaredBetween(pt, item);
                    if (distSquaredTo < distanceSquared && !object.ReferenceEquals(item, ignore))
                    {
                        distanceSquared = distSquaredTo;
                        result = item;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Find the largest dimensional axis in the specified collection of objects
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        protected Dimension LargestDimension(IList<T> collection, out double min, out double max)
        {
            if (collection.Count > 0)
            {
                T first = collection[0];
                double minX = _Tree.MinXOf(first);
                double maxX = _Tree.MaxXOf(first);
                double minY = _Tree.MinYOf(first);
                double maxY = _Tree.MaxYOf(first);
                double minZ = _Tree.MinZOf(first);
                double maxZ = _Tree.MaxZOf(first);

                for (int i = 1; i < collection.Count; i++)
                {
                    T entry = collection[i];
                    if (_Tree.MinXOf(entry) < minX) minX = _Tree.MinXOf(entry);
                    if (_Tree.MaxXOf(entry) > maxX) maxX = _Tree.MaxXOf(entry);
                    if (_Tree.MinYOf(entry) < minY) minY = _Tree.MinYOf(entry);
                    if (_Tree.MaxYOf(entry) > maxY) maxY = _Tree.MaxYOf(entry);
                    if (_Tree.MinZOf(entry) < minZ) minZ = _Tree.MinZOf(entry);
                    if (_Tree.MaxZOf(entry) > maxZ) maxZ = _Tree.MaxZOf(entry);
                }

                double dX = maxX - minX;
                double dY = maxY - minY;
                double dZ = maxZ - minZ;

                if (dX > dY && dX > dZ)
                {
                    min = minX;
                    max = maxX;
                    return Dimension.X;
                }
                else if (dY > dZ)
                {
                    min = minY;
                    max = maxY;
                    return Dimension.Y;
                }
                else
                {
                    min = minZ;
                    max = maxZ;
                    return Dimension.Z;
                }
            }
            min = 0;
            max = 0;
            return Dimension.Undefined;
        }

        /// <summary>
        /// Subdivide this node along the principal axis of its child objects
        /// </summary>
        public void Subdivide()
        {
            if (_Children != null && _Children.Count > 1)
            {
                double max;
                double min;
                Dimension largest = LargestDimension(_Children, out min, out max);

                int divisions = Math.Min(_Children.Count, Math.Min(_Tree.MaxDivisions, (int)Math.Floor(max - min / _Tree.MinCellSize) + 1));
                if (divisions > 1)
                {
                    _CellSize = (max - min) / (divisions - 1);
                    _Origin = min - _CellSize / 2;
                    _SplitDimension = largest;
                    _Branches = new DDTreeNode<T>[divisions];
                    //Add all children to branches:
                    foreach (T child in _Children)
                    {
                        AddToBranch(child);
                    }
                    //Subdivide children:
                    for (int i = 0; i < _Branches.Length; i++)
                    {
                        DDTreeNode<T> node = _Branches[i];
                        if (node != null) node.Subdivide();
                    }
                }
            }
        }


    }
}

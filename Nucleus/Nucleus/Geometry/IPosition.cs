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

using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Interface for entities which occupy a position in space
    /// represented by a vector - nodes, vertices etc.
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Get the position vector of this object
        /// </summary>
        Vector Position { get; }
    }

    /// <summary>
    /// Extension methods for IPosition objects
    /// </summary>
    public static class IPositionExtensions
    {
        /// <summary>
        /// Find the distance between the position of this object and the
        /// position of another IPosition-implementing object
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double DistanceTo(this IPosition thisIPos, IPosition other)
        {
            return thisIPos.Position.DistanceTo(other.Position);
        }

        /// <summary>
        /// Find the squared distance between the position of this object and
        /// the position of another IPosition-implementing object.
        /// This operation will be more efficient that the DistanceTo alternative
        /// as it does not involve a (slow) square-root operation.
        /// </summary>
        /// <param name="thisIPos"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double DistanceToSquared(this IPosition thisIPos, IPosition other)
        {
            return thisIPos.Position.DistanceToSquared(other.Position);
        }

        /// <summary>
        /// Find the distance between the position of this object and the
        /// position of another IPosition-implementing object
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double DistanceTo(this IPosition thisIPos, Vector point)
        {
            return thisIPos.Position.DistanceTo(point);
        }

        /// <summary>
        /// Find the squared distance between the position of this object and
        /// the position of another IPosition-implementing object.
        /// This operation will be more efficient that the DistanceTo alternative
        /// as it does not involve a (slow) square-root operation.
        /// </summary>
        /// <param name="thisIPos"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double DistanceToSquared(this IPosition thisIPos, Vector point)
        {
            return thisIPos.Position.DistanceToSquared(point);
        }

        /// <summary>
        /// Check for containment of a point within a polygon with these vertices on the XY plane
        /// </summary>
        /// <param name="point">The point to test for containment</param>
        /// <returns>True if the point is inside (or on) the polygon, else false.</returns>
        public static bool PolygonContainmentXY<T>(this IList<T> polygon, Vector point) where T:IPosition
        {
            if (polygon.Count > 2)
            {
                int count = 0;
                Vector lastPoint = polygon[0].Position;
                bool onLine = false;
                for (int i = 1; i < polygon.Count; i++)
                {
                    Vector nextPoint = polygon[i].Position;
                    if (Intersect.XRayLineSegmentXYCheck(ref point, ref lastPoint, ref nextPoint, out onLine))
                        count++;
                    if (onLine) return true; //TODO: Review
                    lastPoint = nextPoint;
                }
                Vector startPoint = polygon[0].Position;
                if (Intersect.XRayLineSegmentXYCheck(ref point, ref lastPoint, ref startPoint, out onLine))
                    count++;
                if (onLine) return true; //TODO: Review
                return count.IsOdd();
            }
            else return false;
        }

        /// <summary>
        /// Find the averate position vector of a this collection of points
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Vector AveragePoint<T>(this IList<T> points) where T:IPosition
        {
            if (points.Count > 0)
            {
                Vector result = points[0].Position;
                for (int i = 1; i < points.Count; i++)
                {
                    result += points[i].Position;
                }
                return result / points.Count;
            }
            else return Vector.Unset;
        }

        /// <summary>
        /// Get a point along the edge of the polygon represented by this set of points,
        /// denoted by a parameter matching the indices of the edge points.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="points"></param>
        /// <param name="t">The parameter from which to extract the point.  Note that
        /// this is not unitised to the length of the polygon edge but instead corresponds to
        /// the vertex indices - i.e. 0-1 will be between the first and second indices,
        /// 1-2 will be between the second and third and so on.</param>
        /// <returns></returns>
        public static Vector PolygonEdgePointAt<T>(this IList<T> points, double t) where T:IPosition
        {
            if (points.Count > 0)
            {
                int index = (int)Math.Floor(t);
                Vector pt0 = points.GetWrapped(index).Position;
                Vector pt1 = points.GetWrapped(index + 1).Position;
                double spanT = t % 1.0;
                return pt0.Interpolate(pt1, spanT);
            }
            else return Vector.Unset;
        }

        /// <summary>
        /// Remove from this list of points any which are within tolerance of the preceeding point
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="polygon"></param>
        /// <param name="tolerance">The limit within which points will be removed</param>
        public static void CleanTinyEdges<T>(this IList<T> polygon, double tolerance = 0.0001)
            where T:IPosition
        {
            double limit = tolerance * tolerance;
            for (int i = polygon.Count - 1; i > 0; i--)
            {
                if (polygon[i].DistanceToSquared(polygon[i - 1]) <= limit) polygon.RemoveAt(i);
            }
            // Final check - start and end:
            if (polygon.Count > 1 && polygon[0].DistanceToSquared(polygon.Last()) < limit)
            {
                polygon.RemoveAt(polygon.Count - 1);
            }
        }

        /// <summary>
        /// Find the furthest object in this set from the specified position
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="positions"></param>
        /// <param name="fromThis"></param>
        /// <returns></returns>
        public static T FindFurthest<T>(this IEnumerable<T> positions, Vector fromThis)
            where T:class, IPosition
        {
            T result = null;
            double maxDist = 0;
            foreach (T position in positions)
            {
                double dist = position.DistanceToSquared(fromThis);
                if (result == null || dist > maxDist)
                {
                    result = position;
                    maxDist = dist;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the closest object in this set to the specified position that lies in the specified direction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="positions"></param>
        /// <param name="toPoint"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static T FindFurthestInDirection<T>(this IEnumerable<T> positions, Vector toPoint, Vector direction)
            where T : class, IPosition
        {
            T result = null;
            double maxDist = 0;
            foreach (T position in positions)
            {
                if (position.Position.IsInDirection(direction, toPoint))
                {
                    double dist = position.DistanceToSquared(toPoint);
                    if (result == null || dist > maxDist)
                    {
                        result = position;
                        maxDist = dist;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Find the closest object in this enumerable to the specified position
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="positions"></param>
        /// <param name="toThis"></param>
        /// <returns></returns>
        public static T FindClosest<T>(this IEnumerable<T> positions, IPosition toThis)
            where T:class, IPosition
        {
            T result = null;
            double minDist = 0;
            foreach (T position in positions)
            {
                double dist = position.DistanceToSquared(toThis);
                if (result == null || dist < minDist)
                {
                    result = position;
                    minDist = dist;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the closest object in this set to the specified position that lies in the specified direction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="positions"></param>
        /// <param name="toPoint"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static T FindClosestInDirection<T>(this IEnumerable<T> positions, Vector toPoint, Vector direction)
            where T:class, IPosition
        {
            T result = null;
            double minDist = 0;
            foreach (T position in positions)
            {
                if (position.Position.IsInDirection(direction, toPoint))
                {
                    double dist = position.DistanceToSquared(toPoint);
                    if (result == null || dist < minDist)
                    {
                        result = position;
                        minDist = dist;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Find the distance to the closest object in this enumerable to the specified position
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="positions"></param>
        /// <param name="toThis"></param>
        /// <returns></returns>
        public static double ClosestDistanceSquared<T>(this IEnumerable<T> positions, IPosition toThis)
              where T:class, IPosition
        {
            T result = null;
            double minDist = 0;
            foreach (T position in positions)
            {
                double dist = position.DistanceToSquared(toThis);
                if (result == null || dist < minDist)
                {
                    result = position;
                    minDist = dist;
                }
            }
            return minDist;
        }

        /// <summary>
        /// Get the vector from the position at the specified index to the
        /// position after it in this list
        /// </summary>
        /// <param name="v"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Vector VectorToNext<TPosition>(this IList<TPosition> v, int index)
            where TPosition : IPosition
        {
            return v.GetWrapped(index).Position - v[index].Position;
        }

        /// <summary>
        /// Get the position vectors of all items in this collection
        /// </summary>
        /// <typeparam name="TPosition"></typeparam>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector[] GetPositions<TPosition>(this IList<TPosition> v)
            where TPosition : IPosition
        {
            var result = new Vector[v.Count];
            for (int i = 0; i < v.Count; i++)
            {
                result[i] = v[i].Position;
            }
            return result;
        }

        /// <summary>
        /// Determine the side of an infinite line on which the positions of the objects in this
        /// collection lie.
        /// Will return right, left, or undefined if the positions lie across the line.
        /// </summary>
        /// <typeparam name="TPosition"></typeparam>
        /// <param name="v"></param>
        /// <param name="lineOrigin">A point which lies on the line</param>
        /// <param name="lineDir">The direction vector of the line</param>
        /// <param name="tolerance">A tolerance distance (as a multiple of the
        /// magnitude of the line direction) to either side of the line within
        /// which the point is taken to lie on the line.</param>
        /// <returns></returns>
        public static HandSide SideOf<TPosition>(this IList<TPosition> v, Vector lineOrigin, Vector lineDir, double tolerance = 0)
            where TPosition : IPosition
        {
            HandSide result = HandSide.Undefined;
            for (int i = 0; i < v.Count; i++)
            {
                Vector position = v[i].Position;
                HandSide side = position.SideOf(lineOrigin, lineDir, tolerance, result);
                if (side != result)
                {
                    if (result == HandSide.Undefined) result = side;
                    else return HandSide.Undefined;
                }
            }
            return result;
        }

    }
}

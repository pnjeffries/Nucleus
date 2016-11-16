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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
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

    }
}

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
    /// Interface for objects that define a local coordinate system.
    /// </summary>
    public interface ICoordinateSystem
    {
        /// <summary>
        /// Convert a vector defined in the global coordinate system into 
        /// one defined in local coordinates of this coordinate system.
        /// </summary>
        /// <param name="vector">A vector in the global coordinate system.</param>
        /// <returns>A vector in local coordinates</returns>
        Vector GlobalToLocal(Vector vector);

        /// <summary>
        /// Convert a vector defined in the local coordinate system into
        /// one defined in global coordinates
        /// </summary>
        /// <param name="vector">A vector in the local coordinate system.</param>
        /// <returns>A vector in global coordinates</returns>
        Vector LocalToGlobal(Vector vector);

        /// <summary>
        /// Convert a set of coordinates defined in the local coordinate system
        /// into one defined in global coordinates
        /// </summary>
        /// <param name="c0">The first coordinate.</param>
        /// <param name="c1">The second coordinate.</param>
        /// <param name="c2">The third coordinate.</param>
        /// <returns>A vector representing a position in the global cartesian coordinate system.</returns>
        Vector LocalToGlobal(double c0, double c1, double c2 = 0);
    }
}

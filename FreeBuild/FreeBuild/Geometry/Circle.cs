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
    /// Immutable geometric primitive representing a planar region within a
    /// set radius of an origin point.
    /// </summary>
    public class Circle : CylindricalCoordinateSystem
    {
        #region Fields

        /// <summary>
        /// The radius of the circle
        /// </summary>
        public readonly double Radius;

        #endregion

        #region Constructors

        /// <summary>
        /// Radius constructor
        /// Creates a disk centred on the origin on the global XY plane
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        public Circle(double radius) : base()
        {
            Radius = radius;
        }

        /// <summary>
        /// Radius, coordinatesystem constructor.
        /// Creates a disk lying on the reference plane and centred on the origin
        /// of the specified coordinate system.
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="cSystem">The coordinate system on which the circle will be placed</param>
        public Circle(double radius, CylindricalCoordinateSystem cSystem) : base(cSystem)
        {
            Radius = radius;
        }

        /// <summary>
        /// Create a Disk of the specified radius about the specified centrepoint.
        /// The disk will be oriented to the global XY plane.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="centre"></param>
        public Circle(double radius, Vector centre) : base(centre)
        {
            Radius = radius;
        }

        /// <summary>
        /// Create a Disk of the specified radius about the specified centrepoint
        /// lying on a plane perpendicular to the given normal direction
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="centre"></param>
        /// <param name="normal"></param>
        public Circle(double radius, Vector centre, Vector normal) : base(centre, normal)
        {
            Radius = radius;
        }

        #endregion
    }
}

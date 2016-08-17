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

using FreeBuild.Base;
using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Represents an infinite plane described in terms of an origin point and a local coordinate system,
    /// where X and Y axes lie on the plane and the Z axis is normal to it.
    /// The coordinate system of the plane is right-handed.
    /// Immutable geometric primitive.
    /// </summary>
    [Serializable]
    public class Plane : 
        CartesianCoordinateSystem,
        IDuplicatable
    {
        #region Constants

        /// <summary>
        /// A preset plane representing the global XY plane, centered on the global origin.
        /// </summary>
        public static readonly Plane GlobalXY = new Plane();

        /// <summary>
        /// A preset plane representing the global YZ plane, centred on the global origin.
        /// </summary>
        public static readonly Plane GlobalYZ = new Plane(Vector.Zero, Vector.UnitY, Vector.UnitZ);

        /// <summary>
        /// A present plane representing the global XZ plane, centred on the global origin
        /// </summary>
        public static readonly Plane GlobalXZ = new Plane(Vector.Zero, Vector.UnitX, Vector.UnitZ);

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Plane is initialised as a global XY plane.
        /// </summary>
        public Plane() : base() { }

        /// <summary>
        /// Constructor creating a global XY plane with its origin at the specified point
        /// </summary>
        /// <param name="origin">The origin point of the new plane</param>
        public Plane(Vector origin) : base(origin) { }

        /// <summary>
        /// Constructor creating a plane defined by an origin point and a normal vector.
        /// The X- and Y-axes will be generated perpendicular to the normal vector and with the x-axis orientated as closely as possible to the global x-axis.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="normal">The normal vector to the plane.  Will become the plane's local Z-axis.
        /// Should be a unit vector if consistent scaling is required.</param>
        public Plane(Vector origin, Vector normal) : base(origin, normal) { }

        /// <summary>
        /// Constructor creating a plane defined by an origin and two vectors on the plane.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="xAxis">The first vector that lies on the plane.  Will form the plane local x-axis.</param>
        /// <param name="xyVector">A second vector that lies on the plane but that is not coincident with the first.</param>
        public Plane(Vector origin, Vector xAxis, Vector xyVector) : base(origin, xAxis, xyVector) { }

        /// <summary>
        /// Constructor creating a plane from the XY plane of the specified coordinate system
        /// </summary>
        /// <param name="cSystem"></param>
        public Plane(CartesianCoordinateSystem cSystem) : base(cSystem) { }

        #endregion

        
    }
}

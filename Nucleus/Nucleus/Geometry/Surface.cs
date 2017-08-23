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

namespace Nucleus.Geometry
{
    /// <summary>
    /// Abstract base class for surfaces - infinitely thin membrane geometries
    /// </summary>
    [Serializable]
    public abstract class Surface : VertexGeometry
    {
        #region Properties

        /// <summary>
        /// Get the number of faces (or, subsurfaces) that this surface posesses
        /// </summary>
        public virtual int FaceCount
        {
            get
            {
                return 1;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculate the surface area (and centroid) of this surface
        /// </summary>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public abstract double CalculateArea(out Vector centroid);

        /// <summary>
        /// Calculate the surface area of this surface
        /// </summary>
        /// <returns></returns>
        public double CalculateArea()
        {
            Vector centroid;
            return CalculateArea(out centroid);
        }

        public override string ToString()
        {
            return "Surface";
        }

        /// <summary>
        /// Evaluate the local coordinate system of this surface.
        /// By convention, the z-axis of the local coordinate system will point normal to the
        /// surafce and the x-axis will be orientated as closely as possible to global X, unless
        /// the x-axis lies within a certain angular limit of z, in which case the global Y axis
        /// will be used instead.
        /// </summary>
        ///  /// <param name="i">The index of the face on which to evaluate the local coordinate system</param>
        /// <param name="u">A normalised parameter defining the first coordinate of a point on this surface.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// /// <param name="v">A normalised parameter defining the first coordinate of a point on this surface.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the X and Y axes of the coordinate 
        /// system around the Z axis, relative to default reference orientation.</param>
        /// <param name="xLimit">The angular limit within which if the local Z and global X approach each other,
        /// local X will be aligned towards global Y rather than global X.  By default, this is 1 degree.</param></param>
        /// <returns></returns>
        public abstract CartesianCoordinateSystem LocalCoordinateSystem(int i, double u, double v, Angle orientation, Angle xLimit);

        /// <summary>
        /// Evaluate the local coordinate system of this surface.
        /// By convention, the z-axis of the local coordinate system will point normal to the
        /// surafce and the x-axis will be orientated as closely as possible to global X, unless
        /// the x-axis lies within a certain angular limit of z, in which case the global Y axis
        /// will be used instead.
        /// </summary>
        /// <param name="i">The index of the face on which to evaluate the local coordinate system</param>
        /// <param name="u">A normalised parameter defining the first coordinate of a point on this surface.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// /// <param name="v">A normalised parameter defining the first coordinate of a point on this surface.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the X and Y axes of the coordinate 
        /// system around the Z axis, relative to default reference orientation.</param>
        /// <returns></returns>
        public CartesianCoordinateSystem LocalCoordinateSystem(int i, double u, double v, Angle orientation)
        {
            return LocalCoordinateSystem(i, u, v, orientation, Angle.FromDegrees(1));
        }

        /// <summary>
        /// Evaluate the local coordinate system of this surface.
        /// By convention, the z-axis of the local coordinate system will point normal to the
        /// surafce and the x-axis will be orientated as closely as possible to global X, unless
        /// the x-axis lies within a certain angular limit of z, in which case the global Y axis
        /// will be used instead.
        /// </summary>
        /// <param name="orientation">The orientation angle.  The rotation of the X and Y axes of the coordinate 
        /// system around the Z axis, relative to default reference orientation.</param>
        /// <returns></returns>
        public CartesianCoordinateSystem LocalCoordinateSystem(Angle orientation)
        {
            return LocalCoordinateSystem(0, 0.5, 0.5, orientation, Angle.FromDegrees(1));
        }

        #endregion
    }
}

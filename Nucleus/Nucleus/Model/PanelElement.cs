﻿// Copyright (c) 2016 Paul Jeffries
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
using Nucleus.Extensions;
using Nucleus.Geometry;

namespace Nucleus.Model
{
    /// <summary>
    /// A 2D element the geometry of which is determined by a
    /// set-out surface geometry and a face property which describes
    /// the cross-thickness properties.
    /// </summary>
    [Serializable]
    public class PanelElement : Element<Surface, BuildUpFamily>
    {
        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a panel element with no data.
        /// </summary>
        public PanelElement() { }

        /// <summary>
        /// Initialises a panel element with the specified set-out
        /// geometry.
        /// </summary>
        /// <param name="geometry">The set-out geometry which defines the shape of
        /// the element.</param>
        public PanelElement(Surface geometry)
        {
            Geometry = geometry;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Orientate this element such that the local X axis at the centre of the element
        /// will point as closely as possible towards the given vector.
        /// </summary>
        /// <param name="vector"></param>
        public override void OrientateToVector(Vector vector)
        {
            var coordSys = Geometry.LocalCoordinateSystem(0, 0.5, 0.5, Angle.Zero);
            Orientation = coordSys.GlobalToLocal(vector, true).Angle;
        }

        /// <summary>
        /// Get the local coordinate system of this element
        /// </summary>
        /// <param name="t">The normalised parameter along the element at which to retrieve the
        /// coordinate system.  By default this will be 0 (i.e. the start of the element)</param>
        /// <returns></returns>
        public CartesianCoordinateSystem LocalCoordinateSystem()
        {
            return Geometry?.LocalCoordinateSystem(Orientation);
        }

        /// <summary>
        /// Get a point in space which nominally describes the position of this element,
        /// to be used for display attachments and the like.
        /// </summary>
        /// <returns></returns>
        public override Vector GetNominalPosition()
        {
            if (Geometry != null)
                return Geometry.Vertices.AveragePoint();
            else return Vector.Unset;
        }

        /// <summary>
        /// Calculate the volume of this element or of a particular material within this element
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public override double CalculateVolume(Material material = null)
        {
            if (Family == null || Geometry == null)
            {
                return 0;
            }

            return Family.GetThickness(material) * Geometry.CalculateArea().Abs();
        }

        #endregion

    }
}

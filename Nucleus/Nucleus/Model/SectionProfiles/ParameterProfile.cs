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

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract base class for profiles which are defined by numerical
    /// parameters and have their geometry automatically generated based
    /// on them.
    /// </summary>
    [Serializable]
    public abstract class ParameterProfile : SectionProfile
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Perimeter property
        /// </summary>
        [NonSerialized]
        private Curve _Perimeter = null;

        /// <summary>
        /// The (generated) shape of the outer perimeter of this profile
        /// </summary>
        public override Curve Perimeter
        {
            get
            {
                if (_Perimeter == null) GenerateGeometry();
                return _Perimeter;
            }
        }

        /// <summary>
        /// Private backing field for Voids property
        /// </summary>
        [NonSerialized]
        private CurveCollection _Voids = null;

        /// <summary>
        /// The collection of curves which denote the edges of internal voids
        /// within this profile
        /// </summary>
        public override CurveCollection Voids
        {
            get
            {
                if (_Voids == null) GenerateGeometry();
                return _Voids;
            }
        }

        /// <summary>
        /// Private backing field for MidPointOffset
        /// </summary>
        [NonSerialized]
        private Vector _OriginOffset;
        
        /// <summary>
        /// Get the offset of the origin (/mid-point) of the section profile
        /// from the set-out location
        /// </summary>
        public Vector OriginOffset
        {
            get
            {
                if (_Perimeter == null) GenerateGeometry();
                return _OriginOffset;
            }
        }

        /// <summary>
        /// Private backing field for CentroidOffset property
        /// </summary>
        [NonSerialized]
        private Vector _CentroidOffset;

        /// <summary>
        /// Get the offset of the area centroid of the section profile from
        /// the origin.  The total offset of the centroid from the set-out
        /// point will be OriginOffset + CentroidOffset.
        /// </summary>
        public Vector CentroidOffset
        {
            get
            {
                if (_Perimeter == null) GenerateGeometry();
                return _CentroidOffset;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate the profile's perimeter
        /// </summary>
        /// <returns></returns>
        protected abstract Curve GeneratePerimeter();

        /// <summary>
        /// Generate the edge curves of the internal voids.
        /// </summary>
        /// <returns></returns>
        protected abstract CurveCollection GenerateVoids();

        /// <summary>
        /// Calculate the total offset of the mid-point of this profile
        /// from the set-out point of the section with the specified horizontal
        /// and vertical set-out
        /// </summary>
        /// <param name="centroid">The centroid of the section profile</param>
        /// <returns></returns>
        protected Vector CalculateOriginOffset(Vector centroid)
        {
            return CalculateOriginOffset(centroid, HorizontalSetOut, VerticalSetOut);
        }

        /// <summary>
        /// Calculate the total offset of the mid-point of this profile
        /// from the set-out point of the section with the specified horizontal
        /// and vertical set-out
        /// </summary>
        /// <param name="centroid">The centroid of the section profile</param>
        /// <returns></returns>
        protected Vector CalculateOriginOffset(Vector centroid, HorizontalSetOut horizontalSetOut, VerticalSetOut verticalSetOut)
        {
            Vector result = Offset;

            if (verticalSetOut == VerticalSetOut.Centroid)
                result = result.AddY(-centroid.Y);
            else if (verticalSetOut == VerticalSetOut.Bottom)
                result = result.AddY(OverallDepth / 2);
            else if (verticalSetOut == VerticalSetOut.Top)
                result = result.AddY(-OverallDepth / 2);

            if (horizontalSetOut == HorizontalSetOut.Centroid)
                result = result.AddX(-centroid.X);
            else if (horizontalSetOut == HorizontalSetOut.Left)
                result = result.AddX(OverallWidth / 2);
            else if (horizontalSetOut == HorizontalSetOut.Right)
                result = result.AddX(-OverallWidth / 2);

            return result;
        }

        public override Vector GetTotalOffset(HorizontalSetOut toHorizontal = HorizontalSetOut.Centroid, VerticalSetOut toVertical = VerticalSetOut.Centroid)
        {
            return CalculateOriginOffset(_CentroidOffset, toHorizontal, toVertical) - _OriginOffset;
        }

        /// <summary>
        /// Update the stored geometry properties of this profile
        /// </summary>
        public void GenerateGeometry()
        {
            //Generate initial geometry:
            _Perimeter = GeneratePerimeter();
            _Voids = GenerateVoids();
            if (_Perimeter != null)
            {
                _CentroidOffset = CalculateCentroidOffset(_Perimeter, _Voids);
                _OriginOffset = CalculateOriginOffset(_CentroidOffset);

                _Perimeter.Move(_OriginOffset);
                foreach (Curve voidCrv in _Voids)
                {
                    voidCrv.Move(_OriginOffset);
                }
            }
        }

        /// <summary>
        /// Calculate the offset of the profile centroid from the origin (mid) point
        /// </summary>
        /// <returns></returns>
        /// <remarks>This basic version calculates the centroid from the perimeter and voids.
        /// This can be overridden for the sake of efficiency in derived classes to save having
        /// to calculate the centroid for symmetrical types where the offset is always 0,0</remarks>
        protected virtual Vector CalculateCentroidOffset(Curve perimeter, CurveCollection voids)
        {
            Vector centroid;
            if (perimeter != null)
            {
                perimeter.CalculateEnclosedArea(out centroid, voids);
            }
            else centroid = new Vector();
            return centroid;
        }

        /// <summary>
        /// Invalidate the stored generated geometry 
        /// </summary>
        public override void InvalidateCachedGeometry()
        {
            _Perimeter = null;
            _Voids = null;
            NotifyPropertyChanged("Perimeter");
            NotifyPropertyChanged("Voids");
            if (Section != null) Section.NotifyProfileChanged(this);
        }

        #endregion
    }
}

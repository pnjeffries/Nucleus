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
        //[NonSerialized]
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
        /// Private 
        /// </summary>
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
        /// Update the stored geometry properties of this profile
        /// </summary>
        public void GenerateGeometry()
        {
            //Generate initial geometry:
            _Perimeter = GeneratePerimeter();
            _Voids = GenerateVoids();
            if (_Perimeter != null)
            {
                Vector centroid;
                _Perimeter.CalculateEnclosedArea(out centroid, _Voids);
                Vector offset = Offset;

                if (VerticalSetOut == VerticalSetOut.Centroid)
                    offset = offset.AddY(-centroid.Y);
                else if (VerticalSetOut == VerticalSetOut.Bottom)
                    offset = offset.AddY(OverallDepth / 2);
                else if (VerticalSetOut == VerticalSetOut.Top)
                    offset = offset.AddY(-OverallDepth / 2);

                if (HorizontalSetOut == HorizontalSetOut.Centroid)
                    offset = offset.AddX(-centroid.X);
                else if (HorizontalSetOut == HorizontalSetOut.Left)
                    offset = offset.AddX(OverallWidth / 2);
                else if (HorizontalSetOut == HorizontalSetOut.Right)
                    offset = offset.AddX(-OverallWidth / 2);

                _Perimeter.Move(offset);
                foreach (Curve voidCrv in _Voids)
                {
                    voidCrv.Move(offset);
                }
            }
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

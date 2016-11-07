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
using FreeBuild.Geometry;
using FreeBuild.Units;

namespace FreeBuild.Model
{
    /// <summary>
    /// Parametric profile type to represent rectangular profiles and
    /// to act as a base class for section types which have a broadly
    /// rectangular shape and posess width and height dimensions.
    /// </summary>
    [Serializable]
    public class RectangularProfile : ParameterProfile
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Depth property
        /// </summary>
        private double _Depth;

        /// <summary>
        /// The depth of the section
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double Depth
        {
            get { return _Depth; }
            set
            {
                _Depth = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Depth");
            }
        }

        /// <summary>
        /// Private backing member variable for the Width property
        /// </summary>
        private double _Width;

        /// <summary>
        /// The width of the section
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Width");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RectangularProfile() { }

        /// <summary>
        /// Initialises a Rectangular profile with the specified depth and width
        /// </summary>
        /// <param name="depth">The depth of the section</param>
        /// <param name="width">The width of the section</param>
        public RectangularProfile(double depth, double width)
        {
            Depth = depth;
            Width = width;
        }

        #endregion

        #region Methods

        protected override Curve GeneratePerimeter()
        {
            return PolyCurve.Rectangle(Depth, Width);
        }

        protected override CurveCollection GenerateVoids()
        {
            return new CurveCollection();
        }

        #endregion
    }
}

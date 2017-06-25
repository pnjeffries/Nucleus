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
using Nucleus.Geometry;
using Nucleus.Units;
using Nucleus.Extensions;

namespace Nucleus.Model
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
                CatalogueName = null;
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
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Width");
            }
        }

        /// <summary>
        /// Private backing field for CornerRadius property
        /// </summary>
        private double _CornerRadius = 0.0;

        /// <summary>
        /// The radius of the corners of the section
        /// </summary>
        public double CornerRadius
        {
            get { return _CornerRadius; }
            set
            {
                _CornerRadius = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("CornerRadius");
            }
        }

        /// <summary>
        /// Get the overall depth of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the profile
        /// (i.e. the depth between extreme points).
        /// </summary>
        public override double OverallDepth { get { return _Depth; } }

        /// <summary>
        /// Get the overall width of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the profile
        /// (i.e. the width between extreme points).
        /// </summary>
        public override double OverallWidth { get { return _Width; } }

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
        public RectangularProfile(double depth, double width, double cornerRadius = 0.0)
        {
            Depth = depth;
            Width = width;
            CornerRadius = cornerRadius;
        }

        /// <summary>
        /// Initialise a rectangular profile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width,
        /// Corner Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        public RectangularProfile(string dimensionString)
        {
            string[] tokens = TokeniseDimensionString(dimensionString);
            if (tokens.Length > 0) Depth = tokens[0].ToDouble(0) / 1000;
            if (tokens.Length > 1) Width = tokens[1].ToDouble(0) / 1000;
            if (tokens.Length > 2) CornerRadius = tokens[2].ToDouble(0) / 1000;
        }

        #endregion

        #region Methods

        protected override Curve GeneratePerimeter()
        {
            return PolyCurve.Rectangle(Depth, Width, CornerRadius);
        }

        protected override CurveCollection GenerateVoids()
        {
            return new CurveCollection();
        }

        public override string GenerateDescription()
        {
            return string.Format("Rect {0:0.##}×{1:0.##}",
                Depth * 1000, Width * 1000);
        }

        #endregion
    }
}

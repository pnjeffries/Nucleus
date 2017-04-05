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
using FreeBuild.Extensions;

namespace FreeBuild.Model
{
    /// <summary>
    /// Parametric profile type to represent circular profiles and
    /// to act as a base class for section types which have a broadly
    /// circular shape and posess a diameter dimension.
    /// </summary>
    [Serializable]
    public class CircularProfile : ParameterProfile
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Depth property
        /// </summary>
        private double _Diameter;

        /// <summary>
        /// The depth of the section
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double Diameter
        {
            get { return _Diameter; }
            set
            {
                _Diameter = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Diameter");
            }
        }

        /// <summary>
        /// Get the overall depth of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the profile
        /// (i.e. the depth between extreme points).
        /// </summary>
        public override double OverallDepth { get { return _Diameter; } }

        /// <summary>
        /// Get the overall width of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the profile
        /// (i.e. the width between extreme points).
        /// </summary>
        public override double OverallWidth { get { return _Diameter; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new Circular profile without any set parameters.
        /// </summary>
        public CircularProfile() { }

        /// <summary>
        /// Initialises a new Circlular profile with the specified diameter
        /// </summary>
        /// <param name="diameter"></param>
        public CircularProfile(double diameter)
        {
            Diameter = diameter;
        }

        // <summary>
        /// Initialise a CircularProfile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width,
        /// Flange Thickness, Web Thickness, Root Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        public CircularProfile(string dimensionString)
        {
            string[] tokens = dimensionString.Split('x', '×', ' ');
            if (tokens.Length > 0) Diameter = tokens[0].ToDouble(0) / 1000;
        }

        #endregion

        #region Methods

        protected override Curve GeneratePerimeter()
        {
            return new Arc(new Circle(Diameter / 2));
        }

        protected override CurveCollection GenerateVoids()
        {
            return new CurveCollection();
        }

        public override string GenerateDescription()
        {
            return string.Format("Circ {0:0.##}",
                Diameter * 1000);
        }

        #endregion
    }
}

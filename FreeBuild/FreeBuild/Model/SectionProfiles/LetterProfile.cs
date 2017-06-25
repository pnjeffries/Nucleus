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

using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using Nucleus.Extensions;

namespace Nucleus.Model
{
    /// <summary>
    /// An abstract base class for I-, T-, C- and L- shape profiles, which
    /// consist of a single web and one or two flanges in various arrangements
    /// </summary>
    [Serializable]
    public abstract class LetterProfile : ParameterProfile
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

        /// <summary>
        /// Private backing field for FlangeThickness property
        /// </summary>
        private double _FlangeThickness;

        /// <summary>
        /// The thickness of the flange
        /// of the section.
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double FlangeThickness
        {
            get { return _FlangeThickness; }
            set
            {
                _FlangeThickness = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("FlangeThickness");
            }
        }

        /// <summary>
        /// Private backing field for WebThickness property
        /// </summary>
        private double _WebThickness;

        /// <summary>
        /// The thickness of the web
        /// of the section
        /// </summary>
        public double WebThickness
        {
            get { return _WebThickness; }
            set
            {
                _WebThickness = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("WebThickness");
            }
        }

        /// <summary>
        /// Private backing member variable for the Depth property
        /// </summary>
        private double _RootRadius;

        /// <summary>
        /// The root radius of the fillet between web and flange of this profile
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double RootRadius
        {
            get { return _RootRadius; }
            set
            {
                _RootRadius = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("RootRadius");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected LetterProfile() : base() { }

        /// <summary>
        /// Initialises a profile with the specified dimensions
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="width"></param>
        /// <param name="flangeThickness"></param>
        /// <param name="webThickness"></param>
        protected LetterProfile(double depth, double width, double flangeThickness, double webThickness, double rootRadius) : base()
        {
            Depth = depth;
            Width = width;
            FlangeThickness = flangeThickness;
            WebThickness = webThickness;
            RootRadius = rootRadius;
        }

        // <summary>
        /// Initialise a LetterProfile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width,
        /// Flange Thickness, Web Thickness, Root Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        protected LetterProfile(string dimensionString)
        {
            string[] tokens = dimensionString.Split('x', '×', ' ');
            if (tokens.Length > 0) Depth = tokens[0].ToDouble(0) / 1000;
            if (tokens.Length > 1) Width = tokens[1].ToDouble(0) / 1000;
            if (tokens.Length > 2) FlangeThickness = tokens[2].ToDouble(0) / 1000;
            if (tokens.Length > 3) WebThickness = tokens[3].ToDouble(0) / 1000;
            if (tokens.Length > 4) RootRadius = tokens[4].ToDouble(0) / 1000;
        }


        #endregion

        #region Methods

        protected override CurveCollection GenerateVoids()
        {
            return new CurveCollection();
        }

        #endregion
    }
}

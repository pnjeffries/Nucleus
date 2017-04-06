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

using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;
using FreeBuild.Extensions;

namespace FreeBuild.Model
{
    /// <summary>
    /// A profile which is rectangular and hollow, consisting
    /// of two webs and two flanges
    /// </summary>
    [Serializable]
    public class RectangularHollowProfile : RectangularProfile
    {
        #region Properties

        /// <summary>
        /// Private backing field for FlangeThickness property
        /// </summary>
        private double _FlangeThickness;

        /// <summary>
        /// The thickness of the top and bottom flange plates
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
        /// The thickness of the left and right web plates
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
        /// Does this profile (potentially) have voids?
        /// </summary>
        public override bool HasVoids { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RectangularHollowProfile() : base() { }

        /// <summary>
        /// Initialises a rectangular hollow section with the specified dimensions
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="width"></param>
        /// <param name="flangeThickness"></param>
        /// <param name="webThickness"></param>
        public RectangularHollowProfile(double depth, double width, double flangeThickness, double webThickness, double cornerRadius = 0.0)
            :base(depth, width, cornerRadius)
        {
            FlangeThickness = flangeThickness;
            WebThickness = webThickness;
        }

        /// <summary>
        /// Initialise a rectangular hollow profile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width, Flange Thickness,
        /// Web Thickness, Corner Radius
        /// Corner Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        public RectangularHollowProfile(string dimensionString)
        {
            string[] tokens = TokeniseDimensionString(dimensionString);
            if (tokens.Length > 0) Depth = tokens[0].ToDouble(0) / 1000;
            if (tokens.Length > 1) Width = tokens[1].ToDouble(0) / 1000;
            if (tokens.Length > 2) FlangeThickness = tokens[2].ToDouble(0) / 1000;
            if (tokens.Length > 3) WebThickness = tokens[3].ToDouble(0) / 1000;
            if (tokens.Length > 4) CornerRadius = tokens[4].ToDouble(0) / 1000;
        }

        #endregion

        #region Methods

        protected override CurveCollection GenerateVoids()
        {
            CurveCollection result = new CurveCollection();
            Curve voidCrv = 
                PolyCurve.Rectangle(
                    Depth - 2 * FlangeThickness, 
                    Width - 2 * WebThickness,
                    CornerRadius - Math.Max(FlangeThickness, WebThickness));
            if (voidCrv != null) result.Add(voidCrv);
            return result;
        }

        public override string GenerateDescription()
        {
            return string.Format("RHS {0:0.##}×{1:0.##}×{2:0.##}×{3:0.##}",
                Depth * 1000, Width * 1000, FlangeThickness * 1000, WebThickness * 1000);
        }

        #endregion
    }
}

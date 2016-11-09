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
        public RectangularHollowProfile(double depth, double width, double flangeThickness, double webThickness)
            :base(depth, width)
        {

        }

        #endregion

        #region Methods

        protected override CurveCollection GenerateVoids()
        {
            CurveCollection result = new CurveCollection();
            Curve voidCrv = PolyCurve.Rectangle(Depth - 2 * FlangeThickness, Width - 2 * WebThickness);
            if (voidCrv != null) result.Add(voidCrv);
            return result;
        }

        #endregion
    }
}

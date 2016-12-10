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

namespace FreeBuild.Model
{
    /// <summary>
    /// A circular hollow section profile with a constant wall thickness
    /// </summary>
    [Serializable]
    public class CircularHollowProfile : CircularProfile
    {
        #region Properties

        /// <summary>
        /// Private backing field for WallThickness property
        /// </summary>
        private double _WallThickness;

        /// <summary>
        /// The thickness of the tube walls of the section
        /// </summary>
        public double WallThickness
        {
            get { return _WallThickness; }
            set {
                _WallThickness = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("WallThickness");
            }
        }

        /// <summary>
        /// Does this profile (potentially) have voids?
        /// </summary>
        public override bool HasVoids { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a Circular Hollow Profile with no parameters set.
        /// </summary>
        public CircularHollowProfile() { }

        /// <summary>
        /// Initialises a Circular Hollow Profile with the specified diameter and wall thickness.
        /// </summary>
        /// <param name="diameter"></param>
        /// <param name="wallThickness"></param>
        public CircularHollowProfile(double diameter, double wallThickness)
        {
            Diameter = diameter;
            WallThickness = wallThickness;
        }

        #endregion

        #region Methods

        protected override CurveCollection GenerateVoids()
        {
            CurveCollection result = new CurveCollection();
            if (Diameter / 2 > WallThickness)
            {
                Curve voidCrv = new Arc(new Circle(Diameter / 2 - WallThickness));
                if (voidCrv != null) result.Add(voidCrv);
            }
            return result;
        }

        #endregion
    }
}

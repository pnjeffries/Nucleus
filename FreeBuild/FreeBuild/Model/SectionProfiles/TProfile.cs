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
    /// Represents profiles shaped like a captial 'T' consisting of a top flange only
    /// and web.
    /// </summary>
    [Serializable]
    public class TProfile : LetterProfile
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TProfile() : base(){ }

        /// <summary>
        /// Initialises a T-profile with the specified parameters
        /// </summary>
        /// <param name="depth">The depth of the section</param>
        /// <param name="width">The width of the section</param>
        /// <param name="flangeThickness">The thickness of the flange</param>
        /// <param name="webThickness">The thickness of the web</param>
        /// <param name="rootRadius">The fillet root radius between web and flange</param>
        public TProfile(double depth, double width, double flangeThickness, double webThickness, double rootRadius = 0)
            : base(depth, width, flangeThickness, webThickness, rootRadius) { }

        /// <summary>
        /// Initialise a T-profile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width,
        /// Flange Thickness, Web Thickness, Root Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        public TProfile(string dimensionString) : base(dimensionString) { }

        #endregion

        #region Methods

        protected override Curve GeneratePerimeter()
        {
            double xF = Width / 2;
            double xW = WebThickness / 2;
            double yF = Depth / 2;
            double yW = yF - FlangeThickness;
            double fR = RootRadius.Limit(0, Math.Min(xF - xW, Depth - WebThickness));
            double xR = xW + fR;
            double yR = yW - fR;

            PolyCurve result = new PolyCurve(new Line(xF, yF, -xF, yF)); //Top ---
            result.AddLine(-xF, yW); //Top flange left |
            result.AddLine(-xR, yW); //Top left fillet start _
            if (fR > 0) result.AddArcTangent(Vector.UnitX, new Vector(-xW, yR));  //Top left fillet end ¬
            result.AddLine(-xW, -yF); //Web left |
            result.AddLine(xW, -yF); //Web bottom _
            result.AddLine(xW, yR); //Web right |
            if (fR > 0) result.AddArcTangent(Vector.UnitY, new Vector(xR, yW)); //Top Right Fillet r
            result.AddLine(xF, yW); //Top flange bottom right -
            result.AddLine(xF, yF); //Top flange right |

            return result;
        }

        public override string GenerateDescription()
        {
            return string.Format("T {0:0.##}×{1:0.##}×{2:0.##}×{3:0.##}",
                Depth * 1000, Width * 1000, FlangeThickness * 1000, WebThickness * 1000);
        }

        #endregion
    }
}

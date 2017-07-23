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
    /// An 'L'-shaped angle profile consisting of one flange and one web
    /// </summary>
    [Serializable]
    public class AngleProfile : LetterProfile
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AngleProfile() : base() { }

        /// <summary>
        /// Initialises a profile with the specified dimensions
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="width"></param>
        /// <param name="flangeThickness"></param>
        /// <param name="webThickness"></param>
        public AngleProfile(double depth, double width, double flangeThickness, double webThickness, double rootRadius = 0) 
            : base(depth, width, flangeThickness, webThickness, rootRadius) { }

        /// <summary>
        /// Initialise an angle profile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width,
        /// Flange Thickness, Web Thickness, Root Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        public AngleProfile(string dimensionString) : base(dimensionString) { }

        #endregion

        protected override Curve GeneratePerimeter()
        {
            double xF = Width / 2;
            double xW = xF - WebThickness;
            double yF = Depth / 2;
            double yW = yF - FlangeThickness;
            double fR = RootRadius.Limit(0, Math.Min(Width - WebThickness, Depth - FlangeThickness));
            double xR = xW - fR;
            double yR = yW - fR;

            PolyCurve result = new PolyCurve(new Line(-xW, yF, -xF, yF)); //Top ---
            result.AddLine(-xF, -yF); // Left |
            result.AddLine(xF, -yF); // Bottom ---
            result.AddLine(xF, -yW); // Bottom flange right |
            //TODO: Toe Radius?
            result.AddLine(-xR, -yW); // Bottom flange top ---
            if (fR > 0) result.AddArcTangent(-Vector.UnitX, -xW, -yR); // Bottom fillet
            result.AddLine(-xW, yF); // Web Left |

            //TODO: Toe radius

            return result;
        }

        public override string GenerateDescription()
        {
            return string.Format("A {0:0.##}×{1:0.##}×{2:0.##}×{3:0.##}",
                Depth * 1000, Width * 1000, FlangeThickness * 1000, WebThickness * 1000);
        }
    }
}

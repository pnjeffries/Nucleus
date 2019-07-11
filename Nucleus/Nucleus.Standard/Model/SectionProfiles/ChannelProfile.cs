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
    /// Represents profiles shaped like a 'C' (or '[') with two flanges
    /// and a web on the left-hand side.
    /// </summary>
    [Serializable]
    public class ChannelProfile : LetterProfile
    {
        #region Constructors

        /// <summary>
        /// Initialise a new blank channel profile
        /// </summary>
        public ChannelProfile() : base() { }

        /// <summary>
        /// Initialise a new channel profile with the specified parameters
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="width"></param>
        /// <param name="flangeThickness"></param>
        /// <param name="webThickness"></param>
        /// <param name="rootRadius"></param>
        public ChannelProfile(double depth, double width, double flangeThickness, double webThickness, double rootRadius = 0)
            : base(depth, width, flangeThickness, webThickness, rootRadius) { }

        /// <summary>
        /// Initialise a C-profile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width,
        /// Flange Thickness, Web Thickness, Root Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        public ChannelProfile(string dimensionsString) : base(dimensionsString) { }

        #endregion

        public override string GenerateDescription()
        {
            return string.Format("C {0:0.##}×{1:0.##}×{2:0.##}×{3:0.##}",
                Depth * 1000, Width * 1000, FlangeThickness * 1000, WebThickness * 1000);
        }

        protected override Curve GeneratePerimeter()
        {
            double xF = Width / 2;
            double xW = xF - WebThickness;
            double yF = Depth / 2;
            double yW = yF - FlangeThickness;
            double fR = RootRadius.Limit(0, Math.Min(Width - WebThickness, yW));
            double xR = xW - fR;
            double yR = yW - fR;

            PolyCurve result = new PolyCurve(new Line(xF, yF, -xF, yF)); //Top ---
            result.AddLine(-xF, -yF); // Left |
            result.AddLine(xF, -yF); // Bottom ---
            result.AddLine(xF, -yW); // Bottom flange right |
            //TODO: Toe Radius?
            result.AddLine(-xR, -yW); // Bottom flange top ---
            if (fR > 0) result.AddArcTangent(-Vector.UnitX, -xW, -yR); // Bottom fillet
            result.AddLine(-xW, yR); // Web Left |
            if (fR > 0) result.AddArcTangent(Vector.UnitY, -xR, yW); // Top fillet
            result.AddLine(xF, yW); // Top flange bottom
            result.AddLine(xF, yF); // Top flange right

            return result;
        }
    }
}

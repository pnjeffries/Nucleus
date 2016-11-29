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
    public class TProfile : LetterProfile
    {
        #region Properties

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
            : base(depth, width, flangeThickness, webThickness)
        {
            RootRadius = rootRadius;
        }

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

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;

namespace FreeBuild.Model
{
    /// <summary>
    /// Represents profiles shaped like a capital 'I' with a single web and
    /// top and bottom flanges of the same width.
    /// </summary>
    public class SymmetricIProfile : LetterProfile
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SymmetricIProfile() : base() { }

        /// <summary>
        /// Initialises an I-profile
        /// </summary>
        /// <param name="depth">The depth of the section</param>
        /// <param name="width">The width of the section</param>
        /// <param name="flangeThickness">The thickness of the top and bottom flange plates</param>
        /// <param name="webThickness">The thiskness of the web</param>
        public SymmetricIProfile(double depth, double width, double flangeThickness, double webThickness)
            : base(depth, width, flangeThickness, webThickness) { }

        #endregion

        #region Methods

        protected override Curve GeneratePerimeter()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

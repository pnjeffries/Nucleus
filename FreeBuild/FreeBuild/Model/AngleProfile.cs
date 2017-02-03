using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;

namespace FreeBuild.Model
{
    /// <summary>
    /// An 'L'-shaped angle profile consisting of one flange and one web
    /// </summary>
    public class AngleProfile : LetterProfile
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected AngleProfile() : base() { }

        /// <summary>
        /// Initialises a profile with the specified dimensions
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="width"></param>
        /// <param name="flangeThickness"></param>
        /// <param name="webThickness"></param>
        protected AngleProfile(double depth, double width, double flangeThickness, double webThickness, double rootRadius = 0) 
            : base(depth, width, flangeThickness, webThickness, rootRadius) { }

        #endregion

        protected override Curve GeneratePerimeter()
        {
            // TODO!
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;
using FreeBuild.Units;

namespace FreeBuild.Model
{
    /// <summary>
    /// Parametric profile type to represent rectangular profiles and
    /// to act as a base class for section types which have a broadly
    /// rectangular shape and posess width and height dimensions.
    /// </summary>
    [Serializable]
    public class RectangularProfile : ParameterProfile
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
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Width");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RectangularProfile() { }

        /// <summary>
        /// Initialises a Rectangular profile with the specified depth and width
        /// </summary>
        /// <param name="depth">The depth of the section</param>
        /// <param name="width">The width of the section</param>
        public RectangularProfile(double depth, double width)
        {
            Depth = depth;
            Width = width;
        }

        #endregion

        #region Methods

        protected override Curve GeneratePerimeter()
        {
            return PolyLine.Rectangle(Depth, Width);
        }

        protected override CurveCollection GenerateVoids()
        {
            return new CurveCollection();
        }

        #endregion
    }
}

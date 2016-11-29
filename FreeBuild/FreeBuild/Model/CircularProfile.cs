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
    /// Parametric profile type to represent circular profiles and
    /// to act as a base class for section types which have a broadly
    /// circular shape and posess a diameter dimension.
    /// </summary>
    [Serializable]
    public class CircularProfile : ParameterProfile
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Depth property
        /// </summary>
        private double _Diameter;

        /// <summary>
        /// The depth of the section
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double Diameter
        {
            get { return _Diameter; }
            set
            {
                _Diameter = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Diameter");
            }
        }

        /// <summary>
        /// Get the overall depth of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the profile
        /// (i.e. the depth between extreme points).
        /// </summary>
        public override double OverallDepth { get { return _Diameter; } }

        /// <summary>
        /// Get the overall width of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the profile
        /// (i.e. the width between extreme points).
        /// </summary>
        public override double OverallWidth { get { return _Diameter; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new Circular profile without any set parameters.
        /// </summary>
        public CircularProfile() { }

        /// <summary>
        /// Initialises a new Circlular profile with the specified diameter
        /// </summary>
        /// <param name="diameter"></param>
        public CircularProfile(double diameter)
        {
            Diameter = diameter;
        }

        #endregion

        #region Methods

        protected override Curve GeneratePerimeter()
        {
            return new Arc(new Circle(Diameter / 2));
        }

        protected override CurveCollection GenerateVoids()
        {
            return new CurveCollection();
        }

        #endregion
    }
}

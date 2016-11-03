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
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Diameter");
            }
        }

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

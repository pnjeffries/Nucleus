using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            set { _WallThickness = value;  NotifyPropertyChanged("WallThickness"); }
        }

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
    }
}

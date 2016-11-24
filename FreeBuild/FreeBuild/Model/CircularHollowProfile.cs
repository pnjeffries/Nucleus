using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;

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
            set {
                _WallThickness = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("WallThickness");
            }
        }

        /// <summary>
        /// Does this profile (potentially) have voids?
        /// </summary>
        public override bool HasVoids { get { return true; } }

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

        #region Methods

        protected override CurveCollection GenerateVoids()
        {
            CurveCollection result = new CurveCollection();
            if (Diameter / 2 > WallThickness)
            {
                Curve voidCrv = new Arc(new Circle(Diameter / 2 - WallThickness));
                if (voidCrv != null) result.Add(voidCrv);
            }
            return result;
        }

        #endregion
    }
}

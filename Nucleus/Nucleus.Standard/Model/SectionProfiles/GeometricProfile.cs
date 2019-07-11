using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

namespace Nucleus.Model.SectionProfiles
{
    /// <summary>
    /// A section profile that is defined explicitly by user-set cross-sectional geometry.
    /// </summary>
    [Serializable]
    public class GeometricProfile : SectionProfile
    {
        #region Properties

        /// <summary>
        /// Private backing field for Geometry property
        /// </summary>
        private PlanarRegion _Geometry;

        /// <summary>
        /// The geometry of the section profile
        /// </summary>
        public PlanarRegion Geometry
        {
            get { return _Geometry; }
            set
            {
                ChangeProperty(ref _Geometry, value, "Geometry");
                InvalidateCachedGeometry();
            }
        }

        public override double OverallDepth
        {
            get
            {
                if (_Geometry != null)
                    return _Geometry.BoundingBox.SizeY;
                else return 0;
            }
        }

        public override double OverallWidth
        {
            get
            {
                if (_Geometry != null)
                    return _Geometry.BoundingBox.SizeX;
                else return 0;
            }
        }

        public override Curve Perimeter
        {
            get
            {
                return Geometry?.Perimeter;
            }
        }

        public override CurveCollection Voids
        {
            get
            {
                return Geometry?.Voids ?? new CurveCollection();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank GeometricProfile with no geometry.
        /// This profile will not be valid until the Geometry property
        /// is set.
        /// </summary>
        public GeometricProfile()
        {

        }

        /// <summary>
        /// Initialise a new GeometricProfile with the specified geometry.
        /// </summary>
        /// <param name="geometry"></param>
        public GeometricProfile(PlanarRegion geometry)
        {
            Geometry = geometry;
        }

        #endregion

        #region Methods

        public override string GenerateDescription()
        {
            throw new NotImplementedException();
        }

        public override Vector GetTotalOffset(HorizontalSetOut toHorizontal = HorizontalSetOut.Centroid, VerticalSetOut toVertical = VerticalSetOut.Centroid)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

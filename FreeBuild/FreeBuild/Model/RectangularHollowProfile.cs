using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;

namespace FreeBuild.Model
{
    /// <summary>
    /// A profile which is rectangular and hollow, consisting
    /// of two webs and two flanges
    /// </summary>
    [Serializable]
    public class RectangularHollowProfile : RectangularProfile
    {
        #region Properties

        /// <summary>
        /// Private backing field for FlangeThickness property
        /// </summary>
        private double _FlangeThickness;

        /// <summary>
        /// The thickness of the top and bottom flange plates
        /// of the section.
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double FlangeThickness
        {
            get { return _FlangeThickness; }
            set
            {
                _FlangeThickness = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("FlangeThickness");
            }
        }

        /// <summary>
        /// Private backing field for WebThickness property
        /// </summary>
        private double _WebThickness;

        /// <summary>
        /// The thickness of the left and right web plates
        /// of the section
        /// </summary>
        public double WebThickness
        {
            get { return _WebThickness; }
            set
            {
                _WebThickness = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("WebThickness");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RectangularHollowProfile() : base() { }

        /// <summary>
        /// Initialises a rectangular hollow section with the specified dimensions
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="width"></param>
        /// <param name="flangeThickness"></param>
        /// <param name="webThickness"></param>
        public RectangularHollowProfile(double depth, double width, double flangeThickness, double webThickness)
            :base(depth, width)
        {

        }

        #endregion

        #region Methods

        protected override CurveCollection GenerateVoids()
        {
            CurveCollection result = new CurveCollection();
            Curve voidCrv = PolyLine.Rectangle(Depth - 2 * FlangeThickness, Width - 2 * WebThickness);
            if (voidCrv != null) result.Add(voidCrv);
            return result;
        }

        #endregion
    }
}

using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using Nucleus.Extensions;

namespace Nucleus.Model.SectionProfiles
{
    /*public class TrapezoidHollowProfile : TrapezoidProfile
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
                CatalogueName = null;
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
        [Dimension(DimensionType.Distance)]
        public double WebThickness
        {
            get { return _WebThickness; }
            set
            {
                _WebThickness = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("WebThickness");
            }
        }

        /// <summary>
        /// Does this profile (potentially) have voids?
        /// </summary>
        public override bool HasVoids { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new TrapezoidHollowProfile without populated parameters
        /// </summary>
        public TrapezoidHollowProfile() : base() { }

        /// <summary>
        /// Initialise a new TrapezoidHollowProfile with the specified dimensions
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="topWidth"></param>
        /// <param name="baseWidth"></param>
        /// <param name="flangeThickness"></param>
        /// <param name="webThickness"></param>
        public TrapezoidHollowProfile(double depth, double topWidth, double baseWidth, double flangeThickness, double webThickness)
            :base(depth, topWidth, baseWidth)
        {
            FlangeThickness = flangeThickness;
            WebThickness = webThickness;
        }

        #endregion

        #region Methods

        protected override CurveCollection GenerateVoids()
        {
            var result = new CurveCollection();
            double a = (BaseWidth - TopWidth).Abs() / 2;
            double tWx = WebThickness * Math.Sqrt(a.Squared() + Depth.Squared())/Depth;
            double xF = a*(FlangeThickness - WebThickness * Math.Sqrt(a.Squared() + Depth.Squared())/a) / Depth;
              // \\
              //   \\
              //     \\
            var voidCrv = PolyCurve.Trapezoid(Depth - 2 * FlangeThickness, TopWidth - 2 * (tWx - xF), BaseWidth - 2 * (tWx + xF));
            if (voidCrv != null) result.Add(voidCrv);
            return result;
        }
         
        #endregion
    }*/
}

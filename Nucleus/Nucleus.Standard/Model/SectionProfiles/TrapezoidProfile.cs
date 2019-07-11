using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A profile in the shape of a solid trapezoid, with a different width
    /// at the top to the bottom
    /// </summary>
    [Serializable]
    public class TrapezoidProfile : ParameterProfile
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
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Depth");
            }
        }

        /// <summary>
        /// Private backing member variable for the TopWidth property
        /// </summary>
        private double _TopWidth;

        /// <summary>
        /// The width of the top of the section
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double TopWidth
        {
            get { return _TopWidth; }
            set
            {
                _TopWidth = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("TopWidth");
            }
        }

        /// <summary>
        /// Private backing member variable for the BaseWidth property
        /// </summary>
        private double _BaseWidth;

        /// <summary>
        /// The width of the base of the section
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double BaseWidth
        {
            get { return _BaseWidth; }
            set
            {
                _BaseWidth = value;
                CatalogueName = null;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("BaseWidth");
            }
        }

        public override double OverallDepth
        {
            get
            {
                return Depth;
            }
        }

        public override double OverallWidth
        {
            get
            {
                return Math.Max(TopWidth, BaseWidth);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank trapezoid profile with the parameters not set
        /// </summary>
        public TrapezoidProfile() { }

        public TrapezoidProfile(double depth, double topWidth, double baseWidth)
        {
            Depth = depth;
            TopWidth = topWidth;
            BaseWidth = baseWidth;
        }

        /// <summary>
        /// Initialise a TrapexoidProfile based on dimensions specified by a string.
        /// The string should consist of numeric values in mm separated by spaces,
        /// x's or the multiplication sign '×' and in the order Depth, Width,
        /// Flange Thickness, Web Thickness, Root Radius
        /// </summary>
        /// <param name="dimensionString"></param>
        protected TrapezoidProfile(string dimensionString)
        {
            string[] tokens = dimensionString.Split('x', '×', ' ');
            if (tokens.Length > 0) Depth = tokens[0].ToDouble(0) / 1000;
            if (tokens.Length > 1) TopWidth = tokens[1].ToDouble(0) / 1000;
            if (tokens.Length > 2) BaseWidth = tokens[2].ToDouble(0) / 1000;
        }

        #endregion

        public override string GenerateDescription()
        {
            return string.Format("Trap {0:0.##}×{1:0.##}x{2:0.##}",
                Depth * 1000, TopWidth * 1000, BaseWidth * 1000);
        }

        protected override Curve GeneratePerimeter()
        {
            return PolyCurve.Trapezoid(Depth, TopWidth, BaseWidth);
        }

        protected override CurveCollection GenerateVoids()
        {
            return new CurveCollection();
        }
    }
}

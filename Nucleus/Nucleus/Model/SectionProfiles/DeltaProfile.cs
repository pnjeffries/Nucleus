using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

namespace Nucleus.Model
{
    public class DeltaProfile : ParameterProfile
    {
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
                throw new NotImplementedException();
            }
        }

        public override string GenerateDescription()
        {
            throw new NotImplementedException();
        }

        protected override Curve GeneratePerimeter()
        {
            throw new NotImplementedException();
        }

        protected override CurveCollection GenerateVoids()
        {
            throw new NotImplementedException();
        }
    }
}

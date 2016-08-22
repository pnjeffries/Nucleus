using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for profiles which are defined by numerical
    /// parameters and have their geometry automatically generated based
    /// on them.
    /// </summary>
    [Serializable]
    public abstract class ParameterProfile : Profile
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Perimeter property
        /// </summary>
        private Curve _Perimeter = null;

        /// <summary>
        /// The (generated) shape of the outer perimeter of this profile
        /// </summary>
        public override Curve Perimeter
        {
            get
            {
                if (_Perimeter == null) _Perimeter = GeneratePerimeter();
                return _Perimeter;
            }
        }

        /// <summary>
        /// Private 
        /// </summary>
        private CurveCollection _Voids = null;

        /// <summary>
        /// The collection of curves which denote the edges of internal voids
        /// within this profile
        /// </summary>
        public override CurveCollection Voids
        {
            get
            {
                if (_Voids == null) _Voids = GenerateVoids();
                return _Voids;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate the profile's perimeter
        /// </summary>
        /// <returns></returns>
        protected abstract Curve GeneratePerimeter();

        /// <summary>
        /// Generate the edge curves of the internal voids.
        /// </summary>
        /// <returns></returns>
        protected abstract CurveCollection GenerateVoids();

        /// <summary>
        /// Invalidate the stored generated geometry 
        /// </summary>
        public virtual void InvalidateCachedGeometry()
        {
            _Perimeter = null;
        }

        #endregion
    }
}

using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A set of settings and parameters which define how curve offsetting
    /// operations should be performed
    /// </summary>
    [Serializable]
    public class CurveOffsetParameters : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Tidy property
        /// </summary>
        private bool _Tidy = true;

        /// <summary>
        /// Offset tidying toggle.  If true, automatic post-processing operations to 
        /// 'tidy' the offset curve by removing collapsed regions will be performed.
        /// </summary>
        public bool Tidy
        {
            get { return _Tidy; }
            set { ChangeProperty(ref _Tidy, value); }
        }

        /// <summary>
        /// Private backing member variable for the CopyAttributes property
        /// </summary>
        private bool _CopyAttributes = true;

        /// <summary>
        /// Copy attributes toggle.  If true, the offset curve segments will attempt 
        /// to copy the attributes of the original curve segments on which they are based.
        /// </summary>
        public bool CopyAttributes
        {
            get { return _CopyAttributes; }
            set { ChangeProperty(ref _CopyAttributes, value); }
        }

        /// <summary>
        /// Private backing member variable for the CollapseInvertedSegments property
        /// </summary>
        private bool _CollapseInvertedSegments = false;

        /// <summary>
        /// Collapse inverted segments toggle.  If true, segments which would be inverted 
        /// by the offset operation will be removed and connection will be attempted by 
        /// the adjacent segments instead.
        /// </summary>
        public bool CollapseInvertedSegments
        {
            get { return _CollapseInvertedSegments; }
            set { ChangeProperty(ref _CollapseInvertedSegments, value); }
        }

        /// <summary>
        /// Private backing member variable for the CornerType property
        /// </summary>
        private CurveOffsetCornerType _CornerType = CurveOffsetCornerType.Sharp;

        /// <summary>
        /// The approach used to resolve discontinuities in the offset curve.
        /// </summary>
        public CurveOffsetCornerType CornerType
        {
            get { return _CornerType; }
            set { ChangeProperty(ref _CornerType, value); }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Create a new set of curve offset parameters
        /// </summary>
        /// <param name="tidy"></param>
        /// <param name="copyAttributes"></param>
        public CurveOffsetParameters(bool tidy = true, bool copyAttributes = true)
        {
            _Tidy = tidy;
            _CopyAttributes = copyAttributes;
        }

        #endregion
    }
}

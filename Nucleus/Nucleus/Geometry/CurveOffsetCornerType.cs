using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Enumerated value used to describe the approach to be taken to resolving
    /// discontinuities in curves during offsetting.
    /// </summary>
    public enum CurveOffsetCornerType
    {
        /// <summary>
        /// Sharp - the default option.  Offset segments are extended to
        /// meet the 
        /// </summary>
        Sharp = 0
    }
}

using Nucleus.Geometry;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Node results for a specific results case, keyed by type
    /// </summary>
    [Serializable]
    public class CaseNodeResults : CaseResults<NodeResultTypes, Interval>
    {
        /// <summary>
        /// Get the maximum displacement vector from this set of results
        /// </summary>
        /// <returns></returns>
        public Vector GetMaxDisplacement()
        {
            return new Vector
                (
                    SafeGet(NodeResultTypes.Displacement_X).AbsMax,
                    SafeGet(NodeResultTypes.Displacement_Y).AbsMax,
                    SafeGet(NodeResultTypes.Displacement_Z).AbsMax
                );
        }
    }
}

using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Result storage table for elements, keyed by result type
    /// </summary>
    [Serializable]
    public class LinearElementResults : ModelObjectResults<LinearElementResultTypes, LinearIntervalDataSet>
    {
        #region Properties

        /// <summary>
        /// Get the result envelope of the specified type at the specified position
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Interval this[LinearElementResultTypes type, double position]
        {
            get
            {
                var graph = this[type];
                if (graph != null) return graph.ValueAt(position);
                else return Interval.Unset;
            }
        }

        #endregion
    }
}

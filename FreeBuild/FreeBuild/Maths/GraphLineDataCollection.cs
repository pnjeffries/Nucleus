using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// A collection of line graph data
    /// </summary>
    public class GraphLineDataCollection : UniquesCollection<GraphLineData>
    {
        /// <summary>
        /// Get the range of keys contained within this collection
        /// </summary>
        public Interval KeyRange
        {
            get
            {
                Interval result = Interval.Unset;
                foreach (var data in this)
                {
                    if (data.Data != null)
                    {
                        if (!result.IsValid) result = data.Data.KeyRange;
                        else result = result.Union(data.Data.KeyRange);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Get the range of values contained within this collection
        /// </summary>
        public Interval ValueRange
        {
            get
            {
                Interval result = Interval.Unset;
                foreach (var data in this)
                {
                    if (data.Data != null)
                    {
                        if (!result.IsValid) result = data.Data.ValueRange;
                        else result = result.Union(data.Data.ValueRange);
                    }
                }
                return result;
            }
        }
    }
}

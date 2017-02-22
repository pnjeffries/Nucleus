using FreeBuild.Base;
using FreeBuild.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maths
{
    /// <summary>
    /// Wrapper for graph line data,
    /// used to present it in a graph
    /// </summary>
    public class GraphLineData : Named
    {
        #region Properties

        /// <summary>
        /// Private backing field for Data property
        /// </summary>
        private LinearIntervalGraph _Data;

        /// <summary>
        /// The line data
        /// </summary>
        public LinearIntervalGraph Data
        {
            get { return _Data; }
            set { ChangeProperty(ref _Data, value, "Data"); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise a new GraphLineData
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public GraphLineData(string name, LinearIntervalGraph data)
        {
            Name = name;
            Data = data;
        }

        #endregion
    }
}

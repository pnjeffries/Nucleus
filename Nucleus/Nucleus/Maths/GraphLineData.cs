﻿using Nucleus.Base;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{

    /// <summary>
    /// Wrapper for graph line data,
    /// used to present it in a graph
    /// </summary>
    [Serializable]
    public class GraphLineData : Named
    {
        #region Properties

        /// <summary>
        /// Private backing field for Data property
        /// </summary>
        private LinearIntervalDataSet _Data;

        /// <summary>
        /// The line data
        /// </summary>
        public LinearIntervalDataSet Data
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
        public GraphLineData(string name, LinearIntervalDataSet data)
        {
            Name = name;
            Data = data;
        }

        #endregion
    }
}
